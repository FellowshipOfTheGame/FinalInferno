using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;


namespace FinalInferno{
    //representa a equipe inteira do jogador
    [CreateAssetMenu(fileName = "Party", menuName = "ScriptableObject/Party", order = 0)]
    public class Party : ScriptableObject{
        private static Party instance = null;
        public static Party Instance{
            get{
                if(!instance)
                    instance = AssetManager.LoadAsset<Party>("Party");
                
                return instance;
            }
        }
        //public type questInfo; //informacoes sobre as missões da equipe
        //public List<Quest> quests; //lista de missões do jogador
        public int level; //nivel da equipe(todos os personagens tem sempre o mesmo nivel)
        public long xp; //experiencia da equipe(todos os personagens tem sempre a mesma experiencia)
        public long xpNext; //experiencia necessaria para avancar de nivel
        public List<Character> characters = new List<Character>(); //lista dos personagens que compoe a equipe 
        [SerializeField] private TextAsset PartyXP;
        private DynamicTable table;

        public void Awake(){
            Debug.Log("sera que tem awake?");
            if(!instance)
                instance = this;
            
            Debug.Log("parece que tem!");

            table = DynamicTable.Create(PartyXP);
            level = 0;
            xp = 0;
            xpNext = 0;
            Debug.Log("Iniciou");
            GiveExp(0);
        }

        //faz todos os persoangens subirem de nivel
        public void LevelUp(){
            foreach (Character character in characters){
                character.LevelUp(level);
            }
        }

        //Adiciona os pontos de experiência conquistado pelo jogador
        public bool GiveExp(long value){
            bool up = false;
            
            xp += value;
            Debug.Log("Deu xp");

            //testa se os persoangens subiram de nivel
            Debug.Log(xp + ">=" + xpNext + "?");
            if(xp >= xpNext){
                while(xp >= xpNext){
                    xpNext = table.Rows[level].Field<long>("XPAcumulada");
                    Debug.Log("agora xp pro proximo level eh: " + xpNext);
                    level++;
                }
                
                Debug.Log("passou pro level: " + level);
                LevelUp();
            
                up = true;
            }

            return up;
        }

        //salva o jogo do jogador
        /*public void Save(){

        }*/

        //carrega o jogo do jogador
        /*public void Load(){

        }*/
    }
}
