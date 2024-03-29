﻿using System.Collections;
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
        public int MinLevel{
            get{
                return Mathf.Max(XpTable.Rows[0].Field<int>("Level"), Table.Rows[0].Field<int>("Level"));
            }
        }
        public int MaxLevel{
            get{
                return Mathf.Min(XpTable.Rows[XpTable.Rows.Count-1].Field<int>("Level"), Table.Rows[Table.Rows.Count-1].Field<int>("Level"));
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
        [SerializeField] private DynamicTable table;
        private DynamicTable Table {
            get {
                if(table == null)
                    table = DynamicTable.Create(skillTable);
                return table;
            }
        }
        [Header("Exp Table")]
        [SerializeField] private TextAsset expTable;
        [SerializeField] private DynamicTable xpTable;
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

        public override void LoadTables(){
            table = DynamicTable.Create(skillTable);
            xpTable = DynamicTable.Create(expTable);
        }

        public override void Preload(){
            level = 0;
            xp = 0;
            xpNext = 0;
        }

        //atualiza o value dos efeitos, se for necessario.
        public void LevelUp(){

            for(int i = 0; i < effects.Count; i++){
                SkillEffectTuple modifyEffect = effects[i];

                modifyEffect.value1 = Table.Rows[level-1].Field<float>("SkillEffect" + i + "Value0");
                
                modifyEffect.value2 = Table.Rows[level-1].Field<float>("SkillEffect" + i + "Value1");

                effects[i] = modifyEffect;
            }

            foreach(PlayerSkill child in skillsToUpdate){
                child.CheckUnlock(Party.Instance.Level);
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

            if(Type == SkillType.PassiveOnStart || Type == SkillType.PassiveOnEnd){
                if(target != TargetType.AllAllies && target != TargetType.AllEnemies && target != TargetType.DeadAllies &&
                   target != TargetType.DeadEnemies && target != TargetType.MultiAlly && target != TargetType.MultiEnemy){
                    // Habilidades que não sejam em área e que só são executadas uma vez durante a batalha precisam disso
                    // para que seu ganho de experiencia fique igual ao de outras habilidades com a mesma tabela de exp
                    GiveExp(BattleManager.instance.GetEnemies(user, true));
                }else{
                    GiveExp(targets);
                }
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
