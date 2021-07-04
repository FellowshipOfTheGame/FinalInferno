using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

namespace FinalInferno{
    public class RECalculator : MonoBehaviour, IOverworldSkillListener
    {
        public static bool encountersEnabled = true;
        [SerializeField] private Transform playerObj = null;
        // Tabela de encontros aleatorios pra este mapa
        [SerializeField] private TextAsset encounterTable = null;
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
                || table == null
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
        void LateUpdate()
        {
            if (encountersEnabled && CharacterOW.PartyCanMove && (agent == null || agent.canInteract)) {
                // Calcula a distancia entre a posicao atual e a distancia no ultimo LateUpdate
                float distance = CalculateDistanceWalked();
                // Incrementa a distancia total entre checagens
                distanceWalked += distance;
                if (distanceWalked >= 1.0f) {
                    // Caso o player tenha se movido ao menos uma unidade, verifica se encontrou batalha
                    CheckEncounter(distanceWalked);
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
        // A proxima checagem de batalha terá uma chance maior
        private void CheckEncounter(float distance) {
            if (isSafeArea) return;

            if (Random.Range(0.0f, 100.0f) < (curEncounterRate * skillModifier)) {
                // Quando encontrar uma batalha
                //Debug.Log("Found random encounter");
                // Impede que o player se movimente e interaja
                CharacterOW.PartyCanMove = false;
                if(agent != null){
                    agent.canInteract = false;
                }
                // Usar a tabela de encontros aleatorios para definir a lista de inimigos
                int partyLevel = Party.Instance.ScaledLevel;
                bool isPowerSpike = partyLevel % 5 == 1;
                int scaledMinEnemies = (!isPowerSpike)? minNumberEnemies : Mathf.Max(minNumberEnemies-1, 1);
                int scaledMaxEnemies = (!isPowerSpike)? maxNumberEnemies : Mathf.Max(scaledMinEnemies, maxNumberEnemies-1);
                Enemy[] enemies= new Enemy[Random.Range(scaledMinEnemies, scaledMaxEnemies+1)];
                //Debug.Log("About to fight " + enemies.Length + " enemies");
                for(int i = 0; i < enemies.Length; i++){
                    enemies[i] = null;

                    for(int j = 0; j < Table.Rows.Count && enemies[i] == null; j++){
                        float roll = Random.Range(0f, 100.0f);
                        float baseChance = Table.Rows[j].Field<float>("Chance");
                        int index = (partyLevel-1) % 5;
                        float chance = (baseChance / 2) + ((baseChance / 2) * (index / 4.0f));
                        //Debug.Log("Rolled a " + roll + " for " + Table.Rows[j].Field<Enemy>("Enemy") + " with chance of " + chance);
                        if(roll <= chance){
                            enemies[i] = Table.Rows[j].Field<Enemy>("Enemy");
                        }
                    }

                    if(enemies[i] == null){
                        // O ultimo inimigo da tabela deve sempre ser o inimigo mais comum com 100% de encounter
                        // Essa condicional é uma precaução para garantir que isso aconteça
                        enemies[i] = Table.Rows[Table.Rows.Count-1].Field<Enemy>("Enemy");
                    }

                    //Debug.Log(enemies[i]);
                }
                // Calculo de level foi movido para Enemy.cs e Party.cs
                // A função é chamada no script de preview
                // Assets/Scripts/UI/Menus/LoadEnemiesPreview.cs

                encounterDecreaseSkill?.Deactivate();
                encounterIncreaseSkill?.Deactivate();
                FinalInferno.UI.ChangeSceneUI.isBattle = true;
                FinalInferno.UI.ChangeSceneUI.battleBG = battleBG;
                FinalInferno.UI.ChangeSceneUI.battleBGM = battleBGM;
                FinalInferno.UI.ChangeSceneUI.battleEnemies = (Enemy[])enemies.Clone();

                decision.Click();
            } else {
                // Caso nao encontre uma batalha
                // Aumenta a chance de encontro linearmente com a distancia percorrida
                curEncounterRate += rateIncreaseValue * distance;
            }
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
