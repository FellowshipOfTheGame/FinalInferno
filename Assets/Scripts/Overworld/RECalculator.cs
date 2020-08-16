using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

namespace FinalInferno{
    public class RECalculator : MonoBehaviour
    {
        public static bool encountersEnabled = true;
        // To do
        // Por ser estatico nao da pra setar no inspetor, mas n faz sentido setar isso pra toda instancia de RECalculator
        // Ou talvez faça referencias na prefab mesmo (ou um SO) e isso deixe de ser estatico
        public static List<PlayerSkill> encounterSkils = new List<PlayerSkill>();
        [SerializeField] private Transform playerObj = null;
        // Tabela de encontros aleatorios pra este mapa
        [SerializeField] private TextAsset encounterTable = null;
        [SerializeField] private DynamicTable table = null;
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

        [Header("Expected value = TriggerChangeScene")]
        [SerializeField] private FinalInferno.UI.FSM.ButtonClickDecision decision;

        // Start is called before the first frame update
        void Start()
        {
            table = null;
            table = DynamicTable.Create(encounterTable);

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

            // Se certifica que não vai fazer nada no update quando a taxa de encontro é 0
            // ou quando a tabela não existir
            if((curEncounterRate < float.Epsilon && rateIncreaseValue < float.Epsilon) || table == null){
                playerObj = null;
            }
        }

        void LateUpdate()
        {
            if (encountersEnabled && (playerObj != null) && CharacterOW.PartyCanMove && (agent == null || agent.canInteract)) {
                // Calcula a distancia entre a posicao atual e a distancia no ultimo LateUpdate
                float distance = Vector2.Distance(lastPosition, new Vector2(playerObj.position.x, playerObj.position.y));
                // Incrementa a distancia total entre checagens
                distanceWalked += distance;
                if (distanceWalked > 1.0f) {
                    // Caso o player tenha se movido ao menos uma unidade, verifica se encontrou batalha
                    CheckEncounter(distanceWalked);
                    // Atualiza lastCheckPosition
                    lastCheckPosition = new Vector2(playerObj.position.x, playerObj.position.y);
                    distanceWalked = 0f;
                }
                // Atualiza o lastPosition
                lastPosition = new Vector2(playerObj.position.x, playerObj.position.y);
            }
        }

        // A chamada da função espera que o valor de distance seja 1.0f
        // Se tiver frame drop e o jogador andar distancias maiores do que deveria sem checar, a chance de ter batalha é maior
        private void CheckEncounter(float distance) {
            if (Random.Range(0.0f, 100.0f) < curEncounterRate) {
                // Quando encontrar uma batalha
                //Debug.Log("Found random encounter");
                // Impede que o player se movimente
                CharacterOW.PartyCanMove = false;
                if(agent != null){
                    agent.canInteract = false;
                }
                // Usar a tabela de encontros aleatorios para definir a lista de inimigos
                bool isPowerSpike = Party.Instance.level % 5 == 1;
                int scaledMinEnemies = (!isPowerSpike)? minNumberEnemies : Mathf.Max(minNumberEnemies-1, 1);
                int scaledMaxEnemies = (!isPowerSpike)? maxNumberEnemies : Mathf.Max(scaledMinEnemies, maxNumberEnemies-1);
                Enemy[] enemies= new Enemy[Random.Range(scaledMinEnemies, scaledMaxEnemies+1)];
                //Debug.Log("About to fight " + enemies.Length + " enemies");
                for(int i = 0; i < enemies.Length; i++){
                    enemies[i] = null;

                    for(int j = 0; j < Table.Rows.Count && enemies[i] == null; j++){
                        float roll = Random.Range(0f, 100.0f);
                        float baseChance = Table.Rows[j].Field<float>("Chance");
                        int index = (Party.Instance.level-1) % 5;
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
                // Calculo de level foi movido para Enemy.cs
                // A função é chamada no script de preview
                // Assets/Scripts/UI/Menus/LoadEnemiesPreview.cs

                FinalInferno.UI.ChangeSceneUI.isBattle = true;
                FinalInferno.UI.ChangeSceneUI.battleBG = battleBG;
                FinalInferno.UI.ChangeSceneUI.battleBGM = battleBGM;
                FinalInferno.UI.ChangeSceneUI.battleEnemies = (Enemy[])enemies.Clone();

                decision.Click();
            } else {
                // Caso nao encontre uma batalha
                //Debug.Log("Did not find random encounter");
                // Aumenta a chance de encontro linearmente com a distancia percorrida
                curEncounterRate += rateIncreaseValue * distance;
            }
        }
    }
}
