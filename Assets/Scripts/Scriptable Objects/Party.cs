using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//representa a equipe inteira do jogador
[CreateAssetMenu(fileName = "Party", menuName = "ScriptableObject/Party", order = 0)]
public class Party : ScriptableObject{
    //public type questInfo; //informacoes sobre as missões da equipe
    //public List<Quest> quests; //lista de missões do jogador
    public int level; //nivel da equipe(todos os personagens tem sempre o mesmo nivel)
    public long xp; //experiencia da equipe(todos os personagens tem sempre a mesma experiencia)
    public long xpNext; //experiencia necessaria para avancar de nivel
    public List<Character> characters; //lista dos personagens que compoe a equipe 
    
    public static Party FindParty(){
        string[] resultsFound = UnityEditor.AssetDatabase.FindAssets(" t:" + typeof(Party).ToString());
        if(resultsFound.Length > 0){
            return (Party)UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(resultsFound[0]), typeof(Party));
        }
        return null;
    }
    // Essa funcao com parametros serve apenas para debug
    public static Party FindParty(string partyName){
        string[] resultsFound = UnityEditor.AssetDatabase.FindAssets(partyName + " t:" + typeof(Party).ToString());
        if(resultsFound.Length > 0){
            return (Party)UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(resultsFound[0]), typeof(Party));
        }
        return null;
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
