using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;
using FinalInferno.UI.Battle;
using System.Data;


namespace FinalInferno{
    //engloba os inimigos do jogador
    [CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObject/Enemy/Basic", order = 0)]
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

        //funcao que escolhe o alvo de um ataque baseado na ameaca que herois representam
        public int TargetDecision(List<BattleUnit> team){
            return Random.Range(0, team.Count);

            /*
            float sumTotal = 0.0f;
            Vector<float> percentual;

            //soma a ameca de todos os herois
            foreach (BattleUnit unit in team){
                sumTotal += unit.aggro;
            }
        
            //calcula a porcentagem que cada heroi represent da soma total das ameacas
            foreach (BattleUnit unit in team){
                percentual.Add(unit.aggro/sumTotal);
            }

            //gera um numero aleatorio entre 0 e 1
            float rand = Random.Range(0.0f, 1.0f);

            //escolhe o alvo com probabilidades baseadas na porcentagem que cada heroi representa da soma total das ameacas
            for(int i = 0; i < team.Count; i++){
                if(rand <= percentual[i])
                    return i; //decide atacar o heroi i
                
                rand -= percentual[i];
            }
            
            */
        }

        //funcao que escolhe o ataque a ser utilizado
        public virtual Skill AttackDecision(){
            return attackSkill; //decide usar ataque basico
        }

        //funcao que escolhe qual acao sera feita no proprio turno
        public virtual Skill SkillDecision(float percentageNotDefense){
            float rand = Random.Range(0.0f, 1.0f); //gera um numero aleatorio entre 0 e 1

            if(rand < percentageNotDefense);
                return AttackDecision(); //decide atacar
            
            return defenseSkill; //decide defender
        }

        //inteligencia atificial do inimigo na batalha
        public virtual void AIEnemy(){
            Skill skill;
            List<BattleUnit> team = BattleManager.instance.GetTeam(UnitType.Enemy);
            float average = 0.0f;
            float percentualHP;

            //calcula a media de vida do grupo de inimigos
            foreach (BattleUnit unit in team){
                average += unit.CurHP;
            }
            average /= team.Count;

            //calcula quanto porecento de vida o inimigo atual tem em relacao a media de vida do grupo de inimigos
            percentualHP = BattleManager.instance.currentUnit.CurHP/average;
            
            skill = SkillDecision(Mathf.Sqrt(percentualHP)+0.05f*percentualHP); //parametro passado calcula o complementar da porcentagem do inimigo defender, baseado no percentual de vida
            
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
