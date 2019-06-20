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
        // TO DO: Revisão de tabelas
        public long XpCumulative{ get{ return ( (table == null)? 0 : (xp + table.Rows[level].Field<long>("XPAcumulada")) ); } }
        public string currentMap;
        public List<Character> characters = new List<Character>(); //lista dos personagens que compoe a equipe 
        [SerializeField] private TextAsset PartyXP;
        [SerializeField] private DynamicTable table;
        private DynamicTable Table {
            get {
                if(table == null)
                    table = DynamicTable.Create(PartyXP);
                return table;
            }
        }

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
        }

        //faz todos os persoangens subirem de nivel
        public void LevelUp(){
            foreach (Character character in characters){
                character.LevelUp(level);
            }
        }

        // Função auxiliar para preview de level baseado na informação do save file
        public int GetLevel(long cumulativeExp){
            level = 0;
            while(cumulativeExp > Table.Rows[level].Field<long>("XPAcumulada")){
                level++;
            }
            return level;
        }

        //Adiciona os pontos de experiência conquistado pelo jogador
        public bool GiveExp(long value){
            table = DynamicTable.Create(PartyXP);
            bool up = false;
            
            xp += value;
            Debug.Log("Deu xp");

            //testa se os persoangens subiram de nivel
            Debug.Log(xp + ">=" + xpNext + "?");
            if(xp >= xpNext){
                Debug.Log("upo ne");

                while(xp >= xpNext && level < Table.Rows.Count-1){
                    Debug.Log("claro que upo");
                    // TO DO: Revisão de tabelas (level tem que ser user friendly)
                    xp -= xpNext;
                    level++;
                    xpNext = Table.Rows[level].Field<long>("XPProximoNivel");
                    Debug.Log("agora xp pro proximo level eh: " + xpNext);
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
