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
        [SerializeField] protected Element element = Element.Neutral;
        public Element Element { get{ return element; } }
        [SerializeField] protected DamageType damageFocus = DamageType.None;
        public DamageType DamageFocus { get{ return damageFocus; } }
        public override Color DialogueColor { get { return color; } }
        public override string DialogueName { get { return (name == null)? "" : name; } }
        [SerializeField] protected TextAsset enemyTable = null;
        [SerializeField] protected DynamicTable table = null;
        protected DynamicTable Table {
            get {
                if(table == null && enemyTable != null)
                    table = DynamicTable.Create(enemyTable);
                else
                    table = null;
                return table;
            }
        }
        protected int curTableRow;
        public override long SkillExp { get { return BaseExp/2; } } // Quanta exp o inimigo dá pra skill quando ela é usada nele
        public long BaseExp { get; protected set; } // Quanta exp o inimigo dá pra party ao final da batalha

        void Awake(){
            if(Table != null && Table.Rows.Count > 0){
                table = DynamicTable.Create(enemyTable);
                
                name = Table.Rows[0].Field<string>("Rank");
                level = Table.Rows[0].Field<int>("Level");
                hpMax = Table.Rows[0].Field<int>("HP");
                baseDmg = Table.Rows[0].Field<int>("Damage");
                baseDef = Table.Rows[0].Field<int>("Defense");
                baseMagicDef = Table.Rows[0].Field<int>("Resistance");
                baseSpeed = Table.Rows[0].Field<int>("Speed");
                BaseExp = Table.Rows[0].Field<int>("XP");
                for(int i = 0; i < elementalResistance.Length; i++){
                    elementalResistance[i] = Table.Rows[0].Field<float>(System.Enum.GetNames(typeof(Element))[i] + "Resistance");
                }
                color = Table.Rows[0].Field<Color>("Color");
                curTableRow = 0;
            }
        }

        // Funcao atualmente desnecessaria
        public int GetSkillLevel(EnemySkill skill){
            int skillIndex = skills.IndexOf(skill);
            if(skillIndex >= 0)
                return Table.Rows[curTableRow].Field<int>("LevelSkill" + skillIndex);
            return 0;
        }

        // Função que atualiza os status do inimigo para um novo level e seta o nível das skills
        public void LevelEnemy(int newLevel){
            if(Table == null){
                Debug.Log("This enemy(" + name + ") has no table to load");
                return;
            }

            level = Mathf.Clamp(newLevel, Table.Rows[0].Field<int>("Level"), Table.Rows[Table.Rows.Count-1].Field<int>("Level"));

            int i = -1;
            do{
                i++;
            }while(i < Table.Rows.Count-1 && Table.Rows[i+1].Field<int>("Level") <= newLevel);

            if(i != curTableRow){
                curTableRow = i;
                name = Table.Rows[i].Field<string>("Rank");
                level = Table.Rows[i].Field<int>("Level");
                hpMax = Table.Rows[i].Field<int>("HP");
                baseDmg = Table.Rows[i].Field<int>("Damage");
                baseDef = Table.Rows[i].Field<int>("Defense");
                baseMagicDef = Table.Rows[i].Field<int>("Resistance");
                baseSpeed = Table.Rows[i].Field<int>("Speed");
                BaseExp = Table.Rows[i].Field<int>("XP");
                for(int j = 0; j < elementalResistance.Length; j++){
                    elementalResistance[j] = Table.Rows[i].Field<float>(System.Enum.GetNames(typeof(Element))[j] + "Resistance");
                }
                color = Table.Rows[i].Field<Color>("Color");
            }

            for(int j = 0; j < skills.Count; j++){
                skills[j].Level = Table.Rows[curTableRow].Field<int>("LevelSkill" + j);
            }
        }

        //funcao que escolhe o alvo de um ataque baseado na ameaca que herois representam
        public virtual int TargetDecision(List<BattleUnit> team){
            float sumTotal = 0.0f;
            List<float> percentual = new List<float>();

            //soma a ameaca de todos os herois
            foreach (BattleUnit unit in team){
                sumTotal += unit.aggro;
            }
        
            //calcula a porcentagem que cada heroi representa da soma total das ameacas
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
            
            return team.Count-1;
        }

        //funcao que escolhe o ataque a ser utilizado
        public virtual Skill AttackDecision(){
            return attackSkill; //decide usar ataque basico
        }

        //funcao que escolhe qual acao sera feita no proprio turno
        public virtual Skill SkillDecision(float percentageNotDefense){
            float rand = Random.Range(0.0f, 1.0f); //gera um numero aleatorio entre 0 e 1

            if(rand < percentageNotDefense)
                return AttackDecision(); //decide atacar
            
            return defenseSkill; //decide defender
        }

        //inteligencia artificial do inimigo na batalha
        public virtual void AIEnemy(){
            Skill skill;
            List<BattleUnit> team = BattleManager.instance.GetTeam(UnitType.Enemy);
            float average = 0.0f;
            float percentualHP;

            //calcula a media de vida do grupo dos inimigos
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
                    targets.Add(team[Random.Range(0, team.Count-1)]);
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
