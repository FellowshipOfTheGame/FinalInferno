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
        public string currentMap = "StartingArea00";
        public int level; //nivel da equipe(todos os personagens tem sempre o mesmo nivel)
        public long xp; //experiencia da equipe(todos os personagens tem sempre a mesma experiencia)
        public long xpNext; //experiencia necessaria para avancar de nivel
        public List<Character> characters; //lista dos personagens que compoe a equipe 
        [SerializeField] private TextAsset XPTable;
        private DynamicTable table;

        public void Awake(){
            if(!instance)
                instance = this;

            //table = DynamicTable.Create(XPTable);
            level = 1;
            xp = 0;
            //xpNext = table.Rows[0].Field<long>("XP para proximo nivel");
        }

        //faz todos os persoangens subirem de nivel
        public void LevelUp(){
            foreach (Character character in characters){
                character.LevelUp(level);
            }
        }

        //Adiciona os pontos de experiência conquistado pelo jogador
        public bool GiveExp(int value){
            xp += value;

            //testa se os persoangens subiram de nivel
            if(xp >= xpNext){
                //xpNext = table.Rows[level].Field<long>("XP para próximo nível");
                level++;
                LevelUp();
            
                return true;
            }
            return false;
        }

        //faz todos os persoangens subirem de nivel
        /*public void LevelUp(){

        }*/

        //salva o jogo do jogador
        /*public void Save(){

        }*/

        //carrega o jogo do jogador
        /*public void Load(){

        }*/
    }
}
