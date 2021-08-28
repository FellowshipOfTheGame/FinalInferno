using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System.Data;

namespace FinalInferno{
    public class RECalculator : MonoBehaviour, IOverworldSkillListener
    {
        public static bool encountersEnabled = true;
        [SerializeField] private Transform playerObj = null;
        // Tabela de encontros aleatorios pra este mapa
        [SerializeField] private TextAsset encounterTable = null;
        [SerializeField] private MapEncounterList mapEncounterList = null;
        [SerializeField] private DynamicTable table;
        private DynamicTable Table {
            get {
                if(table == null)
                    table = DynamicTable.Create(encounterTable);
                return table;
            }
        }
        [Range(0, 4)]
        [SerializeField] private int minNumberEnemies = 0;
        [Range(0, 4)]
        [SerializeField] private int maxNumberEnemies = 0;
        [SerializeField] private EncounterRate encounterRate = null;
        [Space]
        [SerializeField] private OverworldSkill encounterIncreaseSkill = null;
        [SerializeField] private FloatVariable encounterIncDistWalked = null;
        private float EncounterIncDistWalked{
            get => encounterIncDistWalked?.Value ?? 0;
            set => encounterIncDistWalked?.UpdateValue(value);
        }
        private float encounterIncDist = 0;
        [SerializeField] private OverworldSkill encounterDecreaseSkill = null;
        [SerializeField] private FloatVariable encounterDecDistWalked = null;
        private float EncounterDecDistWalked{
            get => encounterDecDistWalked?.Value ?? 0;
            set => encounterDecDistWalked?.UpdateValue(value);
        }
        private float encounterDecDist = 0;
        private float skillModifier = 0;
        [Space]
        [SerializeField] public Sprite battleBG = null;
        [SerializeField] private AudioClip battleBGM = null;
        [SerializeField] private AudioClip overworldBGM = null;

        private float baseEncounterRate = 0f;
        private float rateIncreaseValue = 0f;
        private Fog.Dialogue.Agent agent = null;
        private float curEncounterRate = 0f;
        private Vector2 lastCheckPosition = Vector2.zero;
        private Vector2 lastPosition = Vector2.zero;
        private float distanceWalked = 0f;
        private bool isSafeArea = false;

        [Header("Expected value = TriggerChangeScene")]
        [SerializeField] private FinalInferno.UI.FSM.ButtonClickDecision decision;

        // Start is called before the first frame update
        void Start()
        {
            #if UNITY_EDITOR
            table = DynamicTable.Create(encounterTable);
            #endif

            playerObj = CharacterOW.MainOWCharacter?.transform;
            if(playerObj)
                lastCheckPosition = new Vector2(playerObj.position.x, playerObj.position.y);
            else
                lastCheckPosition = Vector2.zero;
            lastPosition = lastCheckPosition;

            if(agent == null && CharacterOW.MainOWCharacter != null){
                agent = CharacterOW.MainOWCharacter.GetComponent<Fog.Dialogue.Agent>();
            }

            distanceWalked = 0f;
            if(encounterRate != null){
                baseEncounterRate = encounterRate.BaseEncounterRate;
                rateIncreaseValue = encounterRate.RateIncreaseValue;
                distanceWalked = encounterRate.MinFreeWalkDistance - encounterRate.FreeWalkDistance;
            }

            StaticReferences.BGM.PlaySong(overworldBGM);
            curEncounterRate = baseEncounterRate;

            encounterIncDist = encounterIncreaseSkill?.effects[0].value2 ?? 0;
            EncounterIncDistWalked = encounterIncDist;
            encounterDecDist = encounterDecreaseSkill?.effects[0].value2 ?? 0;
            EncounterDecDistWalked = encounterDecDist;
            skillModifier = 1.0f;

            // Se certifica que não vai fazer nada no update quando a taxa de encontro é 0
            // ou quando a tabela não existir
            // ou quando o numero de inimigos por encontro for 0
            if((curEncounterRate < float.Epsilon && rateIncreaseValue < float.Epsilon)
                || mapEncounterList == null
                || (minNumberEnemies == 0 && maxNumberEnemies == 0)){
                curEncounterRate = 0;
                rateIncreaseValue = 0;
                isSafeArea = true;
            }
        }

        public void ReloadTable(){
            table = DynamicTable.Create(encounterTable);
        }

        // A checagem precisa acontecer no LateUpdate para evitar conflito com o update que o sistema de dialogo usa
        void LateUpdate(){
            if (encountersEnabled && CharacterOW.PartyCanMove && (agent == null || agent.canInteract)) {
                // Calcula a distancia entre a posicao atual e a distancia no ultimo LateUpdate
                float distance = CalculateDistanceWalked();
                // Incrementa a distancia total entre checagens
                distanceWalked += distance;
                if (distanceWalked >= 1.0f) {
                    // Caso o player tenha se movido ao menos uma unidade, verifica se encontrou batalha
                    if (CheckEncounter(distanceWalked)) return;
                    // Atualiza distancia das skills
                    EncounterIncDistWalked += ( (EncounterIncDistWalked < (float.MaxValue - distanceWalked))?  distanceWalked : 0);
                    EncounterDecDistWalked += ( (EncounterDecDistWalked < (float.MaxValue - distanceWalked))?  distanceWalked : 0);
                    // Atualiza lastCheckPosition
                    lastCheckPosition = new Vector2(playerObj.position.x, playerObj.position.y);
                    distanceWalked = 0f;
                }
                // Atualiza o lastPosition
                lastPosition = new Vector2(playerObj.position.x, playerObj.position.y);
            }
        }

        private float CalculateDistanceWalked(){
            if(playerObj == null) return 0f;
            return Vector2.Distance(lastPosition, new Vector2(playerObj.position.x, playerObj.position.y));
        }

        // A chamada da função espera que o valor de distance seja 1.0f
        // Se houver frame drop e o jogador andar distancias maiores do que deveria sem checar,
        // A proxima checagem de batalha terá uma chance maior pois o incremento é linear
        private bool CheckEncounter(float distance) {
            if (isSafeArea || !mapEncounterList.HasEncounterGroup) return false;

            if (Random.Range(0.0f, 100.0f) <= (curEncounterRate * skillModifier)) {
                // Caso nao encontre uma batalha
                // Aumenta a chance de encontro linearmente com a distancia percorrida
                curEncounterRate += rateIncreaseValue * distance;
                return false;
            }

            // Quando encontrar uma batalha
            // Impede que o player se movimente e interaja
            CharacterOW.PartyCanMove = false;
            if(agent != null){
                agent.canInteract = false;
            }

            // Usar a tabela de encontros aleatorios para definir a lista de inimigos
            int partyLevel = Party.Instance.ScaledLevel;
            // Pega o dicionario com as chances de cada group (a soma é 100)
            ReadOnlyDictionary<EncounterGroup, float> chanceDict = mapEncounterList.GetChancesForLevel(partyLevel);
            // Monta uma lista em ordem crescente de dificuldade
            List<EncounterGroup> encounterGroups = new List<EncounterGroup>(chanceDict.Keys);
            encounterGroups.Sort((first, second) => first.DifficultyRating.CompareTo(second.DifficultyRating));

            // Calcula qual grupo foi encontrado
            EncounterGroup result;
            float roll = Random.Range(0f, 100.0f);
            result = CalculateRoll(roll, encounterGroups, chanceDict);

            // Calculo de level foi movido para Enemy.cs e Party.cs
            // A função é chamada no script de preview
            // Assets/Scripts/UI/Menus/LoadEnemiesPreview.cs

            encounterDecreaseSkill?.Deactivate();
            encounterIncreaseSkill?.Deactivate();
            FinalInferno.UI.ChangeSceneUI.isBattle = true;
            FinalInferno.UI.ChangeSceneUI.battleBG = battleBG;
            FinalInferno.UI.ChangeSceneUI.battleBGM = battleBGM;
            FinalInferno.UI.ChangeSceneUI.battleEnemies = result.GetEnemies();

            decision.Click();
            return true;
        }

        private EncounterGroup CalculateRoll(float roll, List<EncounterGroup> list, ReadOnlyDictionary<EncounterGroup, float> chanceDict){
            EncounterGroup result = list[0];
            float cummulativeChance = 0;
            for(int i = 0; i < list.Count && roll > cummulativeChance; i++){
                result = list[i];
                cummulativeChance += chanceDict[result];
            }
            return result;
        }

        public void OnEnable(){
            encounterIncreaseSkill?.AddActivationListener(this);
            encounterDecreaseSkill?.AddActivationListener(this);
        }

        public void OnDisable(){
            encounterIncreaseSkill?.RemoveActivationListener(this);
            encounterDecreaseSkill?.RemoveActivationListener(this);
        }

		public void ActivatedSkill(OverworldSkill skill){
            if(skill == null) return;
            if(skill == encounterIncreaseSkill){
                encounterDecreaseSkill?.Deactivate();
                EncounterIncDistWalked = 0;
                skillModifier = skill.effects[0].value1;
            }else if(skill == encounterDecreaseSkill){
                encounterIncreaseSkill?.Deactivate();
                EncounterDecDistWalked = 0;
                skillModifier = skill.effects[0].value1;
            }
		}

		public void DeactivatedSkill(OverworldSkill skill){
            if(skill == encounterDecreaseSkill || skill == encounterIncreaseSkill){
                skillModifier = 1.0f;
                if(skill == encounterDecreaseSkill){
                    EncounterDecDistWalked = encounterDecDist;
                }else{
                    EncounterIncDistWalked = encounterIncDist;
                }
            }
		}
	}
}
