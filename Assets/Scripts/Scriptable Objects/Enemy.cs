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
            //damageType/element = able.Rows[0].Field(int)("DamageType");
            color = Table.Rows[0].Field<Color>("Color");

        }

        //inteligencia atificial do inimigo na batalha
        public void AIEnemy(){
            Skill skill;
            
            float rand = Random.Range(0, 2);

            if(rand == 0)
                skill = attackSkill;
            else
                skill = defenseSkill;
            
            BattleSkillManager.currentSkill = attackSkill;
            BattleSkillManager.currentTargets = GetTargets(skill.target);

            // BattleSkillManager.UseSkill();
        }

        private List<BattleUnit> GetTargets(TargetType type)
        {
            List<BattleUnit> targets = new List<BattleUnit>();
            List<BattleUnit> team = new List<BattleUnit>();

            switch (type)
            {
                case TargetType.Self:
                    targets.Add(BattleManager.instance.currentUnit);
                    break;
                case TargetType.MultiAlly:
                    targets = team = BattleManager.instance.GetTeam(UnitType.Enemy);
                    break;
                case TargetType.MultiEnemy:
                    targets = team = BattleManager.instance.GetTeam(UnitType.Hero);
                    break;
                case TargetType.SingleAlly:
                    team = BattleManager.instance.GetTeam(UnitType.Enemy);
                    targets.Add(team[Random.Range(0, team.Count)]);
                    break;
                case TargetType.SingleEnemy:
                    team = BattleManager.instance.GetTeam(UnitType.Hero);
                    targets.Add(team[Random.Range(0, team.Count)]);
                    break;
            }

            return targets;
        }
    }
}
