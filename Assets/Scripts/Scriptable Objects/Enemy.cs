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
        [SerializeField] protected Element element = Element.Neutral;
        public Element Element { get{ return element; } }
        [SerializeField] protected DamageType damageFocus = DamageType.None;
        public DamageType DamageFocus { get{ return damageFocus; } }
        public override Color DialogueColor { get { return color; } }
        public override string DialogueName { get { return (name == null)? "" : name; } }
        [SerializeField] protected TextAsset enemyTable;
        [SerializeField] protected DynamicTable table;
        protected DynamicTable Table {
            get {
                if(table == null)
                    table = DynamicTable.Create(enemyTable);
                return table;
            }
        }
        protected int curTableRow;
        public override long SkillExp { get { return BaseExp/2; } } // Quanta exp o inimigo dá pra skill quando ela é usada nele
        public long BaseExp { get; protected set; } // Quanta exp o inimigo dá pra party ao final da batalha

        void Awake(){
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
                //elementalResistance[i] = 1.0f;

                elementalResistance[i] = Table.Rows[0].Field<float>(System.Enum.GetNames(typeof(Element))[i] + "Resistance");
            }
            color = Table.Rows[0].Field<Color>("Color");
            curTableRow = 0;
        }

        // Funcao atualmente desnecessaria desnecessaria
        public int GetSkillLevel(EnemySkill skill){
            int skillIndex = skills.IndexOf(skill);
            if(skillIndex >= 0)
                return Table.Rows[curTableRow].Field<int>("LevelSkill" + skillIndex);
            return 0;
        }

        // Função que atualiza os status do inimigo para um novo level e seta o nível das skills
        public void LevelEnemy(int newLevel){
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

        //inteligencia atificial do inimigo na batalha
        public virtual void AIEnemy(){
            Skill skill = attackSkill;
            
            // Se rolar a chance de usar skill, usa a skill
            if(Random.Range(0, 1) < Table.Rows[curTableRow].Field<float>("ChancetoUseSkill") && BattleManager.instance.currentUnit.ActiveSkills.Count > 0){
                skill = BattleManager.instance.currentUnit.ActiveSkills[Random.Range(0, BattleManager.instance.currentUnit.ActiveSkills.Count-1)];
            }
            
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
