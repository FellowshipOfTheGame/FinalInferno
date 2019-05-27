using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public List<Character> characters; //lista dos personagens que compoe a equipe 
        
        public void Awake(){
            if(!instance)
                instance = this;
        }

        public void GiveExp(int value){
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
