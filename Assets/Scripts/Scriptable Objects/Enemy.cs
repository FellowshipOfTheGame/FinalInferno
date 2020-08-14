using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;
using FinalInferno.UI.Battle;
// using System.Data;


namespace FinalInferno{
    //engloba os inimigos do jogador
    [CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObject/Enemy/Basic", order = 0)]
    public class Enemy : Unit, IDatabaseItem{
        public Color dialogueColor;
        [Space(10)]
        [Header("Enemy Info")]
        [SerializeField] protected Element element = Element.Neutral;
        public Element Element { get{ return element; } }
        [SerializeField] protected DamageType damageFocus = DamageType.None;
        public DamageType DamageFocus { get{ return damageFocus; } }
        public override Color DialogueColor { get { return dialogueColor; } }
        public override string DialogueName { get { return (name == null)? "" : name; } }
        [SerializeField] private Sprite bestiaryPortrait;
        public Sprite BestiaryPortrait { get => bestiaryPortrait; }
        [SerializeField] private AudioClip enemyCry;
        public AudioClip EnemyCry { get => enemyCry; }
        [TextArea]
        [SerializeField] private string bio = "Bio";
        public string Bio { get => bio; }
        [Space(10)]
        [Header("Table")]
        [SerializeField] protected TextAsset enemyTable = null;
        [SerializeField] protected DynamicTable table = null;
        protected DynamicTable Table {
            get {
                if(table == null && enemyTable != null)
                    table = DynamicTable.Create(enemyTable);
                else if (enemyTable == null)
                    table = null;
                return table;
            }
        }
        protected int curTableRow = 0;
        public override long SkillExp { get { return BaseExp; } } // Quanta exp o inimigo dá pra skill quando ela é usada nele
        public long BaseExp { get; protected set; } // Quanta exp o inimigo dá pra party ao final da batalha

        public void LoadTables(){
            table = DynamicTable.Create(enemyTable);
            curTableRow = 0;
            LevelEnemy(0);
        }

        public void Preload(){
            curTableRow = 0;
            LevelEnemy(0);
        }

        // Funcao atualmente desnecessaria
        public int GetSkillLevel(EnemySkill skill){
            int skillIndex = skills.IndexOf(skill);
            if(skillIndex >= 0)
                return Table.Rows[curTableRow].Field<int>("LevelSkill" + skillIndex);
            return 0;
        }

        // Função que identifica o nível do inimigo de acordo com o progresso e ajusta como for necessário
        public int LevelEnemy(){
            // Calcula o level dos inimigos
            // Avalia os parametros das quests
            int questParam = 0;
            if(AssetManager.LoadAsset<Quest>("MainQuest").events["CerberusDead"]) questParam++;
            int enemyLevel = questParam * 10;

            // Avalia o nível atual da party
            if(Mathf.Clamp(Party.Instance.level - (questParam * 10), 0, 10) > 5)
                enemyLevel += 5;

            LevelEnemy(enemyLevel);

            // O valor calculado é retornado para calcular apenas uma vez em situações de loop
            return enemyLevel;
        }

        // Função que atualiza os status do inimigo para um novo level e seta o nível das skills
        public void LevelEnemy(int newLevel){
            if(Table == null || Table.Rows.Count < 1){
                Debug.Log("This enemy(" + name + ") has no table to load");
                return;
            }

            level = Mathf.Clamp(newLevel, Table.Rows[0].Field<int>("Level"), Table.Rows[Table.Rows.Count-1].Field<int>("Level"));

            int row = -1;
            do{
                row++;
            }while(row < Table.Rows.Count-1 && Table.Rows[row+1].Field<int>("Level") <= newLevel);

            if(row != curTableRow){
                curTableRow = row;
                name = Table.Rows[row].Field<string>("Rank");
                level = Table.Rows[row].Field<int>("Level");
                hpMax = Table.Rows[row].Field<int>("HP");
                baseDmg = Table.Rows[row].Field<int>("Damage");
                baseDef = Table.Rows[row].Field<int>("Defense");
                baseMagicDef = Table.Rows[row].Field<int>("Resistance");
                baseSpeed = Table.Rows[row].Field<int>("Speed");
                BaseExp = Table.Rows[row].Field<int>("XP");

                elementalResistances.Clear();
                foreach(Element element in System.Enum.GetValues(typeof(Element))){
                    string colName = System.Enum.GetName(typeof(Element), element) + "Resistance";
                    if(Table.Rows[row].HasField<float>(colName)){
                        float value = Table.Rows[row].Field<float>(colName);
                        if(value != 1.0f){
                            elementalResistances.Add(element, value);
                        }
                    }
                }

                color = Table.Rows[row].Field<Color>("Color");
            }

            for(int i = 0; i < skills.Count; i++){
                skills[i].Level = Table.Rows[curTableRow].Field<int>("LevelSkill" + i);
            }
        }

        //funcao que escolhe o alvo de um ataque baseado na ameaca que herois representam
        public virtual int TargetDecision(List<BattleUnit> team){
            float sumTotal = 0.0f;
            List<float> percentual = new List<float>();

            //soma a ameaca de todos os herois
            foreach (BattleUnit unit in team){
                sumTotal += Mathf.Clamp(unit.aggro, Mathf.Epsilon, float.MaxValue);
            }
        
            //calcula a porcentagem que cada heroi representa da soma total das ameacas
            foreach (BattleUnit unit in team){
                percentual.Add(Mathf.Clamp(unit.aggro, Mathf.Epsilon, float.MaxValue)/sumTotal);
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
            BattleSkillManager.currentTargets = GetTargets(skill.target);
        }

        public virtual void ResetParameters(){ /* Função para resetar parametros de boss por exemplo */ }

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
