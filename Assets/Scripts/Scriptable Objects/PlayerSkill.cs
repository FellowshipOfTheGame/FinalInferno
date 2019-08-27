using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

namespace FinalInferno{
    //engloba todas as "skills" dos personagens do jogador, que ganham nivel
    [CreateAssetMenu(fileName = "PlayerSkill", menuName = "ScriptableObject/PlayerSkill", order = 5)]
    public class PlayerSkill : Skill{
        public long xp; //experiencia da "skill"
        public long xpNext; //experiencia necessaria para a "skill" subir de nivel
        // TO DO: Revisão de tabelas (nao sabemos o nome definitivo da coluna)
        public long XpCumulative { get { return ( (table == null)? 0 : xp/*(xp +  ((level <= 1)? 0 : (table.Rows[level-2].Field<long>("XPAcumulada"))) ) */ ); } }
        public string description; //descricao da "skill" que aparecera para o jogador durante a batalha
        [SerializeField] private string ShortDescription; //descricao mais curta da skill para casos onde a descricao completa é muito longa
        public string shortDescription { get { return (ShortDescription != "" && ShortDescription != null) ? ShortDescription : description; } }
        public bool active; //sinaliza se a "skill" esta ativa ou nao
        public List<PlayerSkill> skillsToUpdate; //lista de skills que podem ser destravadas com o level dessa skill
        public List<PlayerSkill> prerequisiteSkills; //lista de skills que sao pre requisitos para essa skill destravar
        public List<int> prerequisiteSkillsLevel; //level que a skill de pre requisito precisa estar para essa skill destravar
        public int prerequisiteHeroLevel; //level que o heroi precisa estar para essa skill destravar
        [SerializeField] private TextAsset skillTable;
        [SerializeField] private DynamicTable table = null;
        private DynamicTable Table {
            get {
                if(table == null)
                    table = DynamicTable.Create(skillTable);
                return table;
            }
        }
        public Sprite skillImage;

        void Awake(){
            table = null;
            table = DynamicTable.Create(skillTable);
            level = 0;
            xp = 0;
            xpNext = 0;
        }

        //atualiza o value dos efeitos, se for necessario.
        public void LevelUp(){
            // int i = 0;
            // effects[i].effect.value1 = Table.Rows[level-1].Field<float>("Effect0Value0");
            // effects[i].effect.value2 = Table.Rows[level-1].Field<float>("Effect0Value1");

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
        }

        //Adiciona os pontos de experiência ao utilizar a skill
        public bool GiveExp(long exp){
            bool up = false;
            
            xp += exp;

            //testa se a skill subiu de nivel
            while(xp >= xpNext && level < Table.Rows.Count-1){
                level++;
                xpNext = level * 100;
                
                up = true;
            }

            if(up) LevelUp();

            return up;
        }

        public bool GiveExp(List<BattleUnit> targets){
            long expValue = 0;

            foreach(BattleUnit target in targets){
                expValue += target.unit.SkillExp;
            }
            expValue /= Mathf.Max(targets.Count, 1);

            return GiveExp(expValue);
        }

        // checa se todos os pre requisitos foram cumpridos para essa skill ser destravada,
        // em caso positivo destrava a skill e retorna TRUE, caso contrario retorna FALSE
        public bool CheckUnlock(int heroLevel){
            bool check = true;

            if(heroLevel >= prerequisiteHeroLevel){
                //checa se todos os pre requisitos foram atendidos
                for(int i = 0; i < prerequisiteSkills.Count; i++){
                    check &= prerequisiteSkills[i].CheckLevel(prerequisiteSkillsLevel[i]);
                }

                //se todos os pre requisitos foram atendidos, destrava a skill
                if(check) GiveExp(0);
            }
            else check = false;

            return check;
        }

        //checa se o level dessa skill cumpre um pre requisito, i.e., eh maior ou igual a um certo valor
        public bool CheckLevel(int prerequisite){
            return (level >= prerequisite);
        }

        // Versao de callback das skills precisa ser responsavel por calcular o ganho de exp
        public override void Use(BattleUnit user, List<BattleUnit> targets, bool shouldOverride1 = false, float value1 = 0f, bool shouldOverride2 = false, float value2 = 0f){
            targets = FilterTargets(user, targets); // Filtragem para garantir a consistencia dos callbacks de AoE
            GiveExp(targets);
            base.Use(user, targets, shouldOverride1, value1, shouldOverride2, value2);
        }

        public override void ResetSkill(){
            level = 0;
            xp = 0;
            xpNext = 0;
            Debug.Log("Skill resetada");
        } 
    }
}