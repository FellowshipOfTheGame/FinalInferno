﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

namespace FinalInferno{
    //engloba os tipos/classes de heroi, personagem do jogador
    [CreateAssetMenu(fileName = "Hero", menuName = "ScriptableObject/Hero", order = 2)]
    public class Hero : Unit{
        public Sprite spriteOW; //"sprite" do heroi no "Over Wolrd"
        public RuntimeAnimatorController animatorOW; //"animator" do "Over World"
        public Sprite skillBG; //"sprite" de fundo da arvore de "skills"  
        [SerializeField] private List<PlayerSkill> InitialsSkills = new List<PlayerSkill>();
        public List<PlayerSkill> skillsToUpdate; //lista de skills que podem ser destravadas com o level do personagem
        public override long SkillExp {
            get{
                if(BattleManager.instance){
                    long expValue = 0;
                    List<BattleUnit> enemies = BattleManager.instance.GetTeam(UnitType.Enemy, true);
                    if(enemies.Count > 0){
                        foreach(BattleUnit bUnit in enemies){
                            expValue += bUnit.unit.SkillExp;
                        }
                        expValue /= enemies.Count;
                        return expValue;
                    }
                }
                return Mathf.Max(10, (Mathf.FloorToInt(Mathf.Sqrt(Party.Instance.XpCumulative))));
            }
        }
        [SerializeField] private TextAsset heroTable;
        [SerializeField] private DynamicTable table;
        private DynamicTable Table {
            get {
                if(table == null)
                    table = DynamicTable.Create(heroTable);
                return table;
            }
        }
        public override bool IsHero{ get{ return true; } }

        void Awake(){
            table = DynamicTable.Create(heroTable);
            for(int i = 0; i < elementalResistance.Length; i++){
                elementalResistance[i] = 1.0f;
            }
        }

        //funcao que ajusta todos os atributos e "skills" do persoangem quando sobe de nivel
        public int LevelUp(int newLevel){
            //Debug.Log(name + " subiu mesmo de level!");

            level = newLevel;
            hpMax = Table.Rows[level-1].Field<int>("HP");
            baseDmg = Table.Rows[level-1].Field<int>("Damage");
            baseDef = Table.Rows[level-1].Field<int>("Defense");
            baseMagicDef = Table.Rows[level-1].Field<int>("Resistance");
            baseSpeed = Table.Rows[level-1].Field<int>("Speed");

            UnlockSkills();

            return hpMax;
        }
        
        //verifica se todas as skills que tem como pre requisito o level do heroi para destravar e tem todas as skills pai destravadas, podem ser destravdas
        public void UnlockSkills(){
            //foreach (PlayerSkill skill in skillsToUpdate)
            foreach(PlayerSkill skill in skillsToUpdate.ToArray()){
                
                //se a skill for destrava esta eh removida da lista e suas skills filhas sao adicionadas
                if(skill.CheckUnlock(level)){  
                    //skills filhas sao adicionadas a lista
                    foreach(PlayerSkill child in skill.skillsToUpdate){
                        skillsToUpdate.Add(child);
                    }
                    // Ativa a skill que foi liberada
                    skill.active = true;
                    
                    skillsToUpdate.Remove(skill); //skill eh removida da lista
                }
            }
        }

        public void ResetHero(){
            foreach(Skill skill in skills){
                skill.ResetSkill();
            }

            skillsToUpdate = new List<PlayerSkill>(InitialsSkills);
            foreach(Skill skill in InitialsSkills){
                skill.active = true;
            }
            
            level = 1;
            Debug.Log("Hero resetado");
        }
        
        public override Color DialogueColor { get { return color; } }
        public override string DialogueName { get { return (name == null)? "" : name; } }
    }
}
