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
        [SerializeField] private int minNumberEnemies;
        [SerializeField] private int maxNumberEnemies;
        [SerializeField] public Sprite battleBG;
        [SerializeField] private AudioClip battleBGM;
        [SerializeField] private AudioClip overworldBGM;
        [SerializeField] private float baseEncounterRate = 5.0f;
        [SerializeField] private float rateIncreaseFactor = 0.05f;
        private float curEncounterRate;
        private Vector2 lastPosition;

        [SerializeField] private FinalInferno.UI.FSM.ButtonClickDecision decision;

        // Start is called before the first frame update
        void Start()
        {
            table = DynamicTable.Create(encounterTable);
            playerObj = CharacterOW.MainOWCharacter?.transform;
            if(playerObj)
                lastPosition = new Vector2(playerObj.position.x, playerObj.position.y);
            else
                lastPosition = Vector2.zero;
            StaticReferences.instance.BGM.PlaySong(overworldBGM);
        }

        // Update is called once per frame
        void Update()
        {
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
        }

        private void CheckEncounter(float distance) {
            // A distancia percorrida e usada para aumentar/diminuir a chance de encontro
            if (Random.Range(0.0f, 100.0f) < curEncounterRate * (distance)) {
                // Quando encontrar uma batalha
                //Debug.Log("Found random encounter");
                // Impede que o player se movimente
                CharacterOW.PartyCanMove = false;
                // Diminui a taxa de encontro para metade do valor base
                // Isso reduz a chance de batalhas consecutivos (atualmente isso n serve pra nada)
                curEncounterRate = baseEncounterRate/2;
                // Usar a tabela de encontros aleatorios para definir a lista de inimigos
                Enemy[] enemies= new Enemy[Random.Range(minNumberEnemies, maxNumberEnemies+1)];
                //Debug.Log("About to fight " + enemies.Length + " enemies");
                for(int i = 0; i < enemies.Length; i++){
                    enemies[i] = null;

                    for(int j = 0; j < Table.Rows.Count && enemies[i] == null; j++){
                        float roll = Random.Range(0f, 100.0f);
                        //Debug.Log("Rolled a " + roll + " for " + (Enemy)Table.Rows[j]["Enemy"] + " with chance of " + (float)Table.Rows[j]["Chance"]);
                        if(roll <= Table.Rows[j].Field<float>("Chance")){
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
                // // Calcula o level dos inimigos
                // // Avalia os parametros das quests
                // int questParam = 0;
                // if(AssetManager.LoadAsset<Quest>("MainQuest").PartyReference.events["CerberusDead"]) questParam++;

                // int enemyLevel = questParam * 10;
                // if(Mathf.Clamp(Party.Instance.level - (questParam * 10), 0, 10) > 5)
                //     enemyLevel += 5;

                // // Cria um hashshet para ignorar repetições e aplica o nível dos inimigos
                // //Debug.Log("Nível dos inimigos calculado(unclamped) = " + enemyLevel);
                // foreach(Enemy enemy in (new HashSet<Enemy>(enemies)) ){
                //     enemy.LevelEnemy(enemyLevel);
                // }

                FinalInferno.UI.ChangeSceneUI.isBattle = true;
                FinalInferno.UI.ChangeSceneUI.battleBG = battleBG;
                FinalInferno.UI.ChangeSceneUI.battleBGM = battleBGM;
                FinalInferno.UI.ChangeSceneUI.battleEnemies = (Enemy[])enemies.Clone();

                decision.Click();
            } else {
                // Caso nao encontre uma batalha
                //Debug.Log("Did not find random encounter");
                // Aumenta a chance de encontro linearmente
                curEncounterRate += rateIncreaseFactor;
            }
        }
    }
}
