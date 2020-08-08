using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

namespace FinalInferno{
    //engloba todas as "skills" dos personagens do jogador, que ganham nivel
    [CreateAssetMenu(fileName = "PlayerSkill", menuName = "ScriptableObject/PlayerSkill", order = 5)]
    public class PlayerSkill : Skill{
        [Header("Player Skill")]
        public long xp; //experiencia da "skill"
        public long xpNext; //experiencia necessaria para a "skill" subir de nivel
        public long XpCumulative { get { return ( (table == null)? 0 : (xp +  ((level <= 1)? 0 : (XpTable.Rows[level-2].Field<long>("XPAccumulated"))) ) ); } }
        public int MaxLevel{
            get{
                return Mathf.Min(XpTable.Rows.Count, Table.Rows.Count);
            }
        }
        [TextArea]
        public string description; //descricao da "skill" que aparecera para o jogador no menu de pause
        public override string ShortDescription { get { return (shortDescription != null && shortDescription != "") ? shortDescription : description; } }
        [Header("Unlock Info")]
        public List<PlayerSkill> skillsToUpdate; //lista de skills que podem ser destravadas com o level dessa skill
        public List<PlayerSkill> prerequisiteSkills; //lista de skills que sao pre requisitos para essa skill destravar
        public List<int> prerequisiteSkillsLevel; //level que a skill de pre requisito precisa estar para essa skill destravar
        public int prerequisiteHeroLevel; //level que o heroi precisa estar para essa skill destravar
        [Header("Stats Table")]
        [SerializeField] private TextAsset skillTable;
        [SerializeField] private DynamicTable table = null;
        private DynamicTable Table {
            get {
                if(table == null)
                    table = DynamicTable.Create(skillTable);
                return table;
            }
        }
        [Header("Exp Table")]
        [SerializeField] private TextAsset expTable;
        [SerializeField] private DynamicTable xpTable = null;
        private DynamicTable XpTable {
            get {
                if(xpTable == null)
                    xpTable = DynamicTable.Create(expTable);
                return xpTable;
            }
        }
        private bool ShouldCalculateMean{
            get{
                return true;
                // Code below will probably be removed soon (tm)
                switch(Type){
                    case SkillType.PassiveOnEnd:
                    case SkillType.PassiveOnSpawn:
                    case SkillType.PassiveOnStart:
                        return false;
                    default:
                        return true;
                }
            }
        }
        public Sprite skillImage;

        void Awake(){
            table = null;
            table = DynamicTable.Create(skillTable);
            xpTable = null;
            xpTable = DynamicTable.Create(expTable);
            level = 0;
            xp = 0;
            xpNext = 0;
        }

        //atualiza o value dos efeitos, se for necessario.
        public void LevelUp(){
            // int i = 0;
            // effects[i].effect.value1 = Table.Rows[level-1].Field<float>("Effect0Value0");
            // effects[i].effect.value2 = Table.Rows[level-1].Field<float>("Effect0Value1");

            //Debug.LogError("Skill = " + name + "; level = " + level);
            for(int i = 0; i < effects.Count; i++){
                SkillEffectTuple modifyEffect = effects[i];
                //Debug.Log("levelapo a " + name);

                modifyEffect.value1 = Table.Rows[level-1].Field<float>("SkillEffect" + i + "Value0");
                //Debug.Log("Mvalue1: " + modifyEffect.value1);
                
                modifyEffect.value2 = Table.Rows[level-1].Field<float>("SkillEffect" + i + "Value1");
                //Debug.Log("Mvalue2: " + modifyEffect.value2);

                effects[i] = modifyEffect;
                //Debug.Log("value1: " + effects[i].value1);
                //Debug.Log("value2: " + effects[i].value2);
            }

            foreach(PlayerSkill child in skillsToUpdate){
                child.CheckUnlock(Party.Instance.level);
            }
        }

        //Adiciona os pontos de experiência ao utilizar a skill
        public bool GiveExp(long exp){
            bool up = false;
            
            xp += exp;

            // testa se a skill subiu de nivel
            // max level = XpTable.Rows.Count
            while(xp >= xpNext && level < XpTable.Rows.Count && level < Table.Rows.Count){
                xp -= xpNext;
                level++;

                xpNext = XpTable.Rows[level-1].Field<long>("XPNextLevel");
                
                up = true;
            }

            if(up) LevelUp();

            return up;
        }

        public bool GiveExp(List<BattleUnit> targets){
            long expValue = 0;

            foreach(BattleUnit target in targets){
                expValue += target.Unit.SkillExp;
            }
            if(ShouldCalculateMean)
                expValue /= Mathf.Max(targets.Count, 1);

            return GiveExp(expValue);
        }

        public bool GiveExp(List<Unit> units){
            long expValue = 0;

            foreach(Unit unit in units){
                expValue += unit.SkillExp;
            }
            if(ShouldCalculateMean)
                expValue /= Mathf.Max(units.Count, 1);

            return GiveExp(expValue);
        }

        // checa se todos os pre requisitos foram cumpridos para essa skill ser destravada,
        // em caso positivo destrava a skill e retorna TRUE, caso contrario retorna FALSE
        public bool CheckUnlock(int heroLevel){
            if(level > 0) return true;

            bool check = (heroLevel >= prerequisiteHeroLevel);

            if(check){
                //checa se todos os pre requisitos foram atendidos
                for(int i = 0; i < prerequisiteSkills.Count; i++){
                    check &= (prerequisiteSkills[i].Level >= prerequisiteSkillsLevel[i]);
                }

                //se todos os pre requisitos foram atendidos, destrava a skill
                if(check){
                    GiveExp(0);
                    active = true;
                }
            }

            return check;
        }

        // Versao de callback das skills precisa ser responsavel por calcular o ganho de exp
        public override void Use(BattleUnit user, List<BattleUnit> targets, bool shouldOverride1 = false, float value1 = 0f, bool shouldOverride2 = false, float value2 = 0f){
            targets = FilterTargets(user, targets); // Filtragem para garantir a consistencia dos callbacks de AoE

            if(Type == SkillType.PassiveOnStart){ // Passivas do tipo OnStart estavam recebendo muito pouca experiencia
                GiveExp(BattleManager.instance.GetEnemies(user));
            }else
                GiveExp(targets);

            base.Use(user, targets, shouldOverride1, value1, shouldOverride2, value2);
        }
        
        public override void ResetSkill(){
            level = 0;
            xp = 0;
            xpNext = 0;
            active = false;
            Debug.Log("Skill resetada");
        } 
    }
}
