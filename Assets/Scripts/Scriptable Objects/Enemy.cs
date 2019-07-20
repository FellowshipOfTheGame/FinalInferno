using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;
using FinalInferno.UI.Battle;
using System.Data;


namespace FinalInferno{
    //engloba os inimigos do jogador
    [CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObject/Enemy", order = 3)]
    public class Enemy : Unit{
        [SerializeField] private Element element = Element.Neutral;
        public Element Element { get{ return element; } }
        [SerializeField] private DamageType damageFocus = DamageType.None;
        public DamageType DamageFocus { get{ return damageFocus; } }
        public override Color DialogueColor { get { return color; } }
        public override string DialogueName { get { return (name == null)? "" : name; } }
        [SerializeField] private TextAsset enemyTable;
        [SerializeField] private DynamicTable table;
        private DynamicTable Table {
            get {
                if(table == null)
                    table = DynamicTable.Create(enemyTable);
                return table;
            }
        }
        public override long SkillExp { get { return BaseExp/2; } } // Quanta exp o inimigo dá pra skill quando ela é usada nele
        public long BaseExp { get; protected set; } // Quanta exp o inimigo dá pra party ao final da batalha

        void Awake(){
            table = DynamicTable.Create(enemyTable);
            
            level = Table.Rows[0].Field<int>("Level");
            hpMax = Table.Rows[0].Field<int>("HP");
            baseDmg = Table.Rows[0].Field<int>("Damage");
            baseDef = Table.Rows[0].Field<int>("Defense");
            baseMagicDef = Table.Rows[0].Field<int>("Resistance");
            baseSpeed = Table.Rows[0].Field<int>("Speed");
            BaseExp = Table.Rows[0].Field<int>("XP");
            for(int i = 0; i < elementalResistance.Length; i++){
                //elementalResistance[i] = 1.0f;

                elementalResistance[i] = Table.Rows[0].Field<float>(System.Enum.GetNames(typeof(Element))[i] + "Resistance");
            }
            color = Table.Rows[0].Field<Color>("Color");
            for(int i = 0; i < skills.Count; i++){
                skills[i].Level = Table.Rows[0].Field<int>("LevelSkill" + i);
            }
        }

        public int GetSkillLevel(EnemySkill skill){
            int skillIndex = skills.IndexOf(skill);
            if(skillIndex >= 0)
                return Table.Rows[level/5].Field<int>("LevelSkill" + skillIndex);
            return 0;
        }

        //TO DO: Função que atualiza os status do inimigo para um novo level e seta o nível das skills

        //inteligencia atificial do inimigo na batalha
        public virtual void AIEnemy(){
            Skill skill;
            
            int rand = Random.Range(0, BattleManager.instance.currentUnit.ActiveSkills.Count);

            if(rand == 0)
                skill = attackSkill;
            else
                skill = BattleManager.instance.currentUnit.ActiveSkills[rand-1];
            
            BattleSkillManager.currentSkill = skill;
            BattleSkillManager.currentUser = BattleManager.instance.currentUnit;
            BattleSkillManager.currentTargets = GetTargets(skill.target);
        }

        protected virtual List<BattleUnit> GetTargets(TargetType type)
        {
            List<BattleUnit> targets = new List<BattleUnit>();
            List<BattleUnit> team = new List<BattleUnit>();

            switch (type)
            {
                case TargetType.Self:
                    targets.Add(BattleManager.instance.currentUnit);
                    break;
                case TargetType.MultiAlly:
                    targets = BattleManager.instance.GetTeam(UnitType.Enemy);
                    break;
                case TargetType.MultiEnemy:
                    targets = BattleManager.instance.GetTeam(UnitType.Hero);
                    break;
                case TargetType.SingleAlly:
                    team = BattleManager.instance.GetTeam(UnitType.Enemy);
                    targets.Add(team[Random.Range(0, team.Count-1)]);
                    break;
                case TargetType.SingleEnemy:
                    team = BattleManager.instance.GetTeam(UnitType.Hero);
                    targets.Add(team[Random.Range(0, team.Count-1)]);
                    break;
            }

            return targets;
        }
    }
}
