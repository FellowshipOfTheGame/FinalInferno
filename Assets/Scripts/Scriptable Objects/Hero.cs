using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//engloba os tipos/classes de heroi, personagem do jogador
[CreateAssetMenu(fileName = "Hero", menuName = "ScriptableObject/Hero", order = 2)]
public class Hero : Unit{
    public Sprite spriteOW; //"sprite" do heroi no "Over Wolrd"
    public Sprite skillBG; //"sprite" de fundo da arvore de "skills" 
    public Sprite portrait; //
    
    public static Hero FindHero(string heroName){
        string[] resultsFound = UnityEditor.AssetDatabase.FindAssets(heroName + " t:" + typeof(Hero).ToString());
        if(resultsFound.Length > 0){
            return (Hero)UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(resultsFound[0]), typeof(Hero));
        }
        return null;
    }

    public override Color DialogueColor { get { return color; } }
    public override string DialogueName { get { return (name == null)? "" : name; } }
}
