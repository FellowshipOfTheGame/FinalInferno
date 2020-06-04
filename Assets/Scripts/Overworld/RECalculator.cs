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
        public static List<PlayerSkill> encounterSkils;
        [SerializeField] private Transform playerObj;
        // Tabela de encontros aleatorios pra este mapa
        [SerializeField] private TextAsset encounterTable;
        [SerializeField] private DynamicTable table = null;
        private DynamicTable Table {
            get {
                if(table == null)
                    table = DynamicTable.Create(encounterTable);
                return table;
            }
        }
        [Range(0, 4)]
        [SerializeField] private int minNumberEnemies;
        [Range(0, 4)]
        [SerializeField] private int maxNumberEnemies;
        [SerializeField] public Sprite battleBG;
        [SerializeField] private AudioClip battleBGM;
        [SerializeField] private AudioClip overworldBGM;
        [Range(0, 100)]
        [SerializeField] private float baseEncounterRate = 5.0f;
        [Range(0, 100)]
        [SerializeField] private float rateIncreaseFactor = 1f;
        private float curEncounterRate;
        private float time;
        private float checkCooldown = 0.25f;
        private float gracePeriod = 1f;
        private Vector2 lastPosition;

        [SerializeField] private FinalInferno.UI.FSM.ButtonClickDecision decision;

        // Start is called before the first frame update
        void Start()
        {
            table = null;
            table = DynamicTable.Create(encounterTable);
            playerObj = CharacterOW.MainOWCharacter?.transform;
            if(playerObj)
                lastPosition = new Vector2(playerObj.position.x, playerObj.position.y);
            else
                lastPosition = Vector2.zero;
            StaticReferences.BGM.PlaySong(overworldBGM);
            curEncounterRate = baseEncounterRate;
            time = -gracePeriod;
        }

        // Update is called once per frame
        void Update()
        {
            if(time > checkCooldown + float.Epsilon){
                time -= checkCooldown;

                if (encountersEnabled && CharacterOW.PartyCanMove) {
                    // Calcula a distancia entre a posicao atual e a distance no ultimo update
                    float distance = Vector2.Distance(lastPosition, new Vector2(playerObj.position.x, playerObj.position.y));
                    if (distance > float.Epsilon) {
                        // Caso o player tenha se movido, verifica se encontrou batalha
                        CheckEncounter(distance);
                        // Atualiza lastPosition
                        lastPosition = new Vector2(playerObj.position.x, playerObj.position.y);
                    }
                }
            }else if(time < -float.Epsilon){
                if(playerObj){
                    lastPosition = new Vector2(playerObj.position.x, playerObj.position.y);
                }
            }

            if(CharacterOW.PartyCanMove){
                time += Time.deltaTime;
            }
        }

        private void CheckEncounter(float distance) {
            if (Random.Range(0.0f, 100.0f) < curEncounterRate) {
                // Quando encontrar uma batalha
                //Debug.Log("Found random encounter");
                // Impede que o player se movimente
                CharacterOW.PartyCanMove = false;
                time = float.MinValue;
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
                // Calculo de level foi movido para a criação da preview de batalha
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
                curEncounterRate += rateIncreaseFactor * distance;
            }
        }
    }
}
