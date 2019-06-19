using System.Collections;
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
        public List<PlayerSkill> skillsToUpdate; //lista de skills que podem ser destravadas com o level do personagem
        public override long SkillExp {get { return Mathf.Max(10, (Mathf.FloorToInt(Mathf.Sqrt(Party.Instance.XpCumulative)))); } }
        [SerializeField] private TextAsset heroTable;
        [SerializeField] private DynamicTable table;
        private DynamicTable Table {
            get {
                if(table == null)
                    table = DynamicTable.Create(heroTable);
                return table;
            }
        }

        void Awake(){
            table = DynamicTable.Create(heroTable);
        }

        //funcao que ajusta todos os atributos e "skills" do persoangem quando sobe de nivel
        public int LevelUp(int newLevel){
            Debug.Log(name + " subiu mesmo de level!");

            level = newLevel;
            hpMax = Table.Rows[level-1].Field<int>("HP");
            baseDmg = Table.Rows[level-1].Field<int>("Dano");
            baseDef = Table.Rows[level-1].Field<int>("Defesa");
            baseMagicDef = Table.Rows[level-1].Field<int>("Resistência");
            baseSpeed = Table.Rows[level-1].Field<int>("Speed");

            UnlockSkills();

            return hpMax;
        }
        
        //verifica se todas as skills que tem como pre requisito o level do heroi para destravar e tem todas as skills pai destravadas, podem ser destravdas
        public void UnlockSkills(){
            //foreach (PlayerSkill skill in skillsToUpdate)
            for(int i = 0; i < skillsToUpdate.Count; i++){
                
                //se a skill for destrava esta eh removida da lista e suas skills filhas sao adicionadas
                if(skillsToUpdate[i].CheckUnlock(level)){  
                    //skills filhas sao adicionadas a lista
                    foreach(PlayerSkill child in skillsToUpdate[i].skillsToUpdate){
                        skillsToUpdate.Add(child);
                    }
                    
                    skillsToUpdate.RemoveAt(i); //skill eh removida da lista
                    i--;
                }
            }
        }
        
        public override Color DialogueColor { get { return color; } }
        public override string DialogueName { get { return (name == null)? "" : name; } }
    }
}
