using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//representa a equipe inteira do jogador
[CreateAssetMenu(menuName = "SOs/Party")]
public class Party : ScriptableObject{
    //public type questInfo; //informacoes sobre as missões da equipe
    //public List<Quest> quests; //lista de missões do jogador
    public int level; //nivel da equipe(todos os personagens tem sempre o mesmo nivel)
    public long xp; //experiencia da equipe(todos os personagens tem sempre a mesma experiencia)
    public long xpNext; //experiencia necessaria para avancar de nivel
    public List<Hero> heroes; //lista dos personagens que compoe a equipe 
    
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
