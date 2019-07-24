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

        public int TargetDecision(List<BattleUnit> team){
            return Random.Range(0, team.Count);

            /*float sumTotal = 0.0f;
            Vector<float> percentual;

            foreach (BattleUnit unit in team){
                sumTotal += unit.aggro;
            }
        
            foreach (BattleUnit unit in team){
                percentual.Add(unit.aggro/sumTotal);
            }

            float rand = Random.Range(0.0f, 1.0f);

            for(int i = 0; i < team.Count; i++){
                if(rand <= percentual[i])
                    return i;
                
                rand -= percentual[i];
            }*/
        }

        public virtual Skill AttackDecision(){
            return attackSkill;
        }

        public virtual Skill SkillDecision(float percentualDefense){
            float rand = Random.Range(0.0f, 1.0f);

            if(rand < percentualDefense);
                return AttackDecision();
            
            return defenseSkill;
        }

        //inteligencia atificial do inimigo na batalha
        public virtual void AIEnemy(){
            Skill skill;
            List<BattleUnit> team = BattleManager.instance.GetTeam(UnitType.Enemy);
            float average = 0.0f;
            float percentualHP;

            foreach (BattleUnit unit in team){
                average += unit.CurHP;
            }
            average /= team.Count;

            percentualHP = BattleManager.instance.currentUnit.CurHP/average;
            
            skill = SkillDecision(Mathf.Sqrt(percentualHP)+0.05f*percentualHP);
            
            BattleSkillManager.currentSkill = skill;
            BattleSkillManager.currentUser = BattleManager.instance.currentUnit;
            BattleSkillManager.currentTargets = GetTargets(skill.target);

            // BattleSkillManager.UseSkill();

        }

        protected virtual List<BattleUnit> GetTargets(TargetType type){
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
                    targets.Add(team[Random.Range(0, team.Count)]);
                    break;
                case TargetType.SingleEnemy:
                    team = BattleManager.instance.GetTeam(UnitType.Hero);
                    targets.Add(team[TargetDecision(team)]);
                    break;
            }

            return targets;
        }
    }
}
