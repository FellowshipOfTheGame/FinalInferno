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
        public Transform playerObj;
        // Tabela de encontros aleatorios pra este mapa
        [SerializeField] private TextAsset encounterTable;
        private DynamicTable table;
        [SerializeField] private int minNumberEnemies;
        [SerializeField] private int maxNumberEnemies;
        [SerializeField] public Sprite BattleBG;
        [SerializeField] private AudioClip BattleBGM;
        [SerializeField] private float baseEncounterRate = 5.0f;
        [SerializeField] private float rateIncreaseFactor = 0.05f;
        private float curEncounterRate;
        private Vector2 lastPosition;

        // Start is called before the first frame update
        void Start()
        {
            table = DynamicTable.Create(encounterTable);
            lastPosition = new Vector2(playerObj.position.x, playerObj.position.y);
        }

        // Update is called once per frame
        void Update()
        {
            if (encountersEnabled) {
                // Calcula a distancia entre a posicao atual e a distance no ultimo update
                float distance = Vector2.Distance(lastPosition, new Vector2(playerObj.position.x, playerObj.position.y));
                if (distance > 0) {
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
                // Diminui a taxa de encontro para metade do valor base
                // Isso reduz a chance de batalhas consecutivos (atualmente isso n serve pra nada)
                curEncounterRate = baseEncounterRate/2;
                // Impede que o player se movimente
                playerObj.GetComponent<Movable>().CanMove = false;
                // Salvar a posição atual de cada player dentro do seu respectivo SO
                // Usar a tabela de encontros aleatorios para definir a lista de inimigos
                // To do
                Enemy[] enemies= new Enemy[Random.Range(minNumberEnemies, maxNumberEnemies+1)];
                //Debug.Log("About to fight " + enemies.Length + " enemies");
                for(int i = 0; i < enemies.Length; i++){
                    float roll = Random.Range(0f, 100.0f);
                    for(int j = 0; j < table.Rows.Count; j++){
                        //Debug.Log("Rolled a " + roll + " for " + (Enemy)table.Rows[j]["Enemy"] + " with chance of " + (float)table.Rows[j]["Chance"]);
                        if(roll <= (float)table.Rows[j]["Chance"]){
                            enemies[i] = (Enemy)table.Rows[j]["Enemy"];
                            break;
                        }
                    }
                    //Debug.Log(enemies[i]);
                }
                SceneLoader.LoadBattleScene(enemies, BattleBG, BattleBGM);
            } else {
                // Caso nao encontre uma batalha
                //Debug.Log("Did not find random encounter");
                // Aumenta a chance de encontro linearmente
                curEncounterRate += rateIncreaseFactor;
            }
        }
    }
}
