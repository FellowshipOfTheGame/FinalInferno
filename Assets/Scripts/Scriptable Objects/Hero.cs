﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;
using System.Data;

//engloba os tipos/classes de heroi, personagem do jogador
[CreateAssetMenu(fileName = "Hero", menuName = "ScriptableObject/Hero", order = 2)]
public class Hero : Unit{
    public Sprite spriteOW; //"sprite" do heroi no "Over Wolrd"
    public RuntimeAnimatorController animatorOW; //"animator" do "Over World"
    public Sprite skillBG; //"sprite" de fundo da arvore de "skills"  
    public List<PlayerSkill> skillsToUpdate; //lista de skills que podem ser destravadas com o level do personagem
    [SerializeField] private TextAsset heroTable;
    private DynamicTable table;

    void Awake(){
        table = DynamicTable.Create(heroTable);
    }

    //funcao que ajusta todos os atributos e "skills" do persoangem quando sobe de nivel
    public int LevelUp(int level){
        Debug.Log(name + " subiu mesmo de level!");

        this.level = level;
        hpMax = table.Rows[level-1].Field<int>("HP");
        baseDmg = table.Rows[level-1].Field<int>("Dano");
        baseDef = table.Rows[level-1].Field<int>("Defesa");
        baseMagicDef = table.Rows[level-1].Field<int>("Resistência");
        baseSpeed = table.Rows[level-1].Field<int>("Speed");

        UnlockSkills();

        return hpMax;
    }
    
    //verifica se todas as skills que tem como pre requisito o level do heroi para destravar e tem todas as skills pai destravadas, podem ser destravdas
    public void UnlockSkills(){
        //foreach (PlayerSkill skill in skillsToUpdate)
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
    
    public override Color DialogueColor { get { return color; } }
    public override string DialogueName { get { return (name == null)? "" : name; } }
}
