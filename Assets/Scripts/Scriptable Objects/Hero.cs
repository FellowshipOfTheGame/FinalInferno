using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//engloba os tipos/classes de heroi, personagem do jogador
[CreateAssetMenu(fileName = "Hero", menuName = "ScriptableObject/Hero", order = 2)]
public class Hero : Unit{
    public Sprite spriteOW; //"sprite" do heroi no "Over Wolrd"
    public Animator animatorOW; //"animator" do "Over World"
    public Sprite skillBG; //"sprite" de fundo da arvore de "skills"  
    public List<PlayerSkill> skillsToUpdate; //lista de skills que podem ser destravadas com o level do personagem
    
    public static Hero FindHero(string heroName){
        string[] resultsFound = UnityEditor.AssetDatabase.FindAssets(heroName + " t:" + typeof(Hero).ToString());
        if(resultsFound.Length > 0){
            return (Hero)UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(resultsFound[0]), typeof(Hero));
        }
        return null;
    }
    
    //verifica se todas as skills que tem como pre requisito o level do heroi para destravar e tem todas as skills pai destravadas, podem ser destravdas
    public void UnlockSkills(){
        foreach (PlayerSkill skill in skillsToUpdate)
        for(int i = 0; i < skillsToUpdate.Count; i++){
            
            //se a skill for destrava esta eh removida da lista e suas skills filhas sao adicionadas
            if(skillsToUpdate[i].CheckUnlock(level)){  
                skillsToUpdate.RemoveAt(i); //skill eh removida da lista
                i--;

                //skills filhas sao adicionadas a lista
                foreach(PlayerSkill child in skillsToUpdate[i].skillsToUpdate){
                    skillsToUpdate.Add(child);
                }
            }
        }
    }

    //checa se o level do heroi cumpre um pre requisito, i.e., eh maior ou igual a um certo valor
    public bool CheckLevel(int prerequisite){
        return (level >= prerequisite);
    }

    public override Color DialogueColor { get { return color; } }
    public override string DialogueName { get { return (name == null)? "" : name; } }
}
