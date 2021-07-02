using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno{
    //engloba os tipos/classes de heroi, personagem do jogador
    [CreateAssetMenu(fileName = "Hero", menuName = "ScriptableObject/Hero", order = 2)]
    public class Hero : Unit, IDatabaseItem{
        public Sprite spriteOW; //"sprite" do heroi no "Over Wolrd"
        public RuntimeAnimatorController animatorOW; //"animator" do "Over World"
        public Sprite skillBG; //"sprite" de fundo da arvore de "skills"  
        [Space(10)]
        [Header("Hero Info")]
        [SerializeField] private List<PlayerSkill> InitialsSkills = new List<PlayerSkill>();
        public List<PlayerSkill> skillsToUpdate; //lista de skills que podem ser destravadas com o level do personagem
        public override long SkillExp {
            get{
                if(BattleManager.instance){
                    long expValue = 0;
                    int nEnemies = 0;
                    List<Unit> units = BattleManager.instance.units;
                    foreach(Unit u in units){
                        if(!u.IsHero){
                            expValue += u.SkillExp;
                            nEnemies++;
                        }
                    }
                    if(nEnemies > 0){
                        expValue /= nEnemies;
                        return expValue;
                    }
                }
                return Mathf.Max(10, (Mathf.FloorToInt(Mathf.Sqrt(Party.Instance.XpCumulative))));
            }
        }
        [Space(10)]
        [Header("Table")]
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

        public void LoadTables(){
            table = DynamicTable.Create(heroTable);
        }

        public void Preload(){
            elementalResistances.Clear();
            skillsToUpdate = new List<PlayerSkill>(InitialsSkills);
            LevelUp(-1, true);
        }

        //funcao que ajusta todos os atributos e "skills" do persoangem quando sobe de nivel
        public int LevelUp(int newLevel, bool ignoreSkills = false){
            //Debug.Log(name + " subiu mesmo de level!");

            level = Mathf.Clamp(newLevel, 1, Table.Rows.Count);
            hpMax = Table.Rows[level-1].Field<int>("HP");
            baseDmg = Table.Rows[level-1].Field<int>("Damage");
            baseDef = Table.Rows[level-1].Field<int>("Defense");
            baseMagicDef = Table.Rows[level-1].Field<int>("Resistance");
            baseSpeed = Table.Rows[level-1].Field<int>("Speed");

            if(!ignoreSkills)
                UnlockSkills();

            return hpMax;
        }
        
        //verifica se todas as skills que tem como pre requisito o level do heroi para destravar e tem todas as skills pai destravadas, podem ser destravdas
        public void UnlockSkills(){
            foreach(PlayerSkill skill in skillsToUpdate.ToArray()){
                
                //se a skill for destrava esta eh removida da lista e suas skills filhas sao adicionadas
                if(skill.CheckUnlock(level)){  
                    //skills filhas sao adicionadas a lista
                    foreach(PlayerSkill child in skill.skillsToUpdate){
                        if(!skillsToUpdate.Contains(child)){
                            skillsToUpdate.Add(child);
                        }
                    }
                    
                    skillsToUpdate.Remove(skill); //skill eh removida da lista
                }
            }
        }

        public void ResetHero(){
            foreach(Skill skill in skills){
                skill.ResetSkill();
            }

            skillsToUpdate = new List<PlayerSkill>(InitialsSkills);
            // foreach(PlayerSkill skill in InitialsSkills){
            //     skill.CheckUnlock(1);
            // }
            
            Debug.Log("Hero resetado");
        }
        
        public override Color DialogueColor { get { return color; } }
        public override string DialogueName { get { return (name == null)? "" : name; } }
    }

    #if UNITY_EDITOR
    [CustomPreview(typeof(Hero))]
    public class HeroPreview : UnitPreview{
        public override bool HasPreviewGUI(){
            return base.HasPreviewGUI();
        }
        public override void OnInteractivePreviewGUI(Rect r, GUIStyle background){
            base.OnInteractivePreviewGUI(r, background);
        }
    }
    #endif
}
