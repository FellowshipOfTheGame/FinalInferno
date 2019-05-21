﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//engloba os tipos/classes de heroi, personagem do jogador
[CreateAssetMenu(fileName = "Hero", menuName = "ScriptableObject/Hero", order = 2)]
public class Hero : Unit{
    public Sprite spriteOW; //"sprite" do heroi no "Over Wolrd"
    public Sprite skillBG; //"sprite" de fundo da arvore de "skills"  
    public List<PlayerSkill> skillsToUpdate; //lista de skills que podem upar com o level do personagem
    
    public void UnlockSkills(){
        foreach (PlayerSkill skill in skillsToUpdate){
            skill.Unlock();
        }
    }

    public override Color DialogueColor { get { return color; } }
    public override string DialogueName { get { return (name == null)? "" : name; } }
}
