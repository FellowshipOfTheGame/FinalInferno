using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    //engloba todas as "skills" dos personagens do jogador, que ganham nivel
    [CreateAssetMenu(fileName = "EnemySkill", menuName = "ScriptableObject/EnemySkill")]
    public class EnemySkill : Skill{
        // TO DO: Revisão de tabelas (nao sabemos o nome definitivo da coluna)
        public string description; //descricao da "skill" que aparecera para o jogador durante a batalha
        [SerializeField] private TextAsset skillTable;
        [SerializeField] private DynamicTable table = null;
        private DynamicTable Table {
            get {
                if(table == null && skillTable != null)
                    table = DynamicTable.Create(skillTable);
                else
                    table = null;
                return table;
            }
        }
        public override int Level{
            get{ return level; }
            set{
                if(value != level && Table != null){
                    level = Mathf.Clamp(value, Table.Rows[0].Field<int>("Level"), Table.Rows[Table.Rows.Count-1].Field<int>("Level"));
                    LevelUp();
                }
            }
        }

        void Awake(){
            if(Table != null){
                table = DynamicTable.Create(skillTable);
                Level = -1;
            }
            active = true;
        }

        //atualiza o value dos efeitos, se for necessario.
        public void LevelUp(){
            if(Table == null){
                Debug.Log("This skill(" + name + ") has no table to load");
                return;
            }
            // int i = 0;
            // effects[i].effect.value1 = Table.Rows[level-1].Field<float>("Effect0Value0");
            // effects[i].effect.value2 = Table.Rows[level-1].Field<float>("Effect0Value1");

            for(int i = 0; i < effects.Count; i++){
                SkillEffectTuple modifyEffect = effects[i];
                //Debug.Log("levelapo a " + name);

                modifyEffect.value1 = Table.Rows[Level-1].Field<float>("SkillEffect" + i + "Value0");
                //Debug.Log("Mvalue1: " + modifyEffect.value1);
                
                modifyEffect.value2 = Table.Rows[Level-1].Field<float>("SkillEffect" + i + "Value1");
                //Debug.Log("Mvalue2: " + modifyEffect.value2);

                effects[i] = modifyEffect;
                //Debug.Log("value1: " + effects[i].value1);
                //Debug.Log("value2: " + effects[i].value2);
            }
        }

        public override void ResetSkill(){
            Level = 0;
            active = true;
            Debug.Log("Skill resetada");
        }
        
        // public override void Use(BattleUnit user, BattleUnit target, bool shouldOverride = false, float value1 = 0, float value2 = 0){
        //     /*
        //     if(user.unit.GetType() == typeof(Enemy)){
        //         Level = ((Enemy)user.unit).GetSkillLevel(this);
        //     } */
        //     base.Use(user, target, shouldOverride, value1, value2);
        // }

        public override void Use(BattleUnit user, List<BattleUnit> targets, bool shouldOverride1 = false, float value1 = 0f, bool shouldOverride2 = false, float value2 = 0f){
            /*
            if(user.unit.GetType() == typeof(Enemy)){ // Isso aqui pode ser bem complicado de garantir nos callbacks, ja que o callback sendo chamado em uma unidade nao necessariamente foi colocado ali por ela mesma
                Level = ((Enemy)user.unit).GetSkillLevel(this);
            }*/
            targets = FilterTargets(user, targets); // Filtragem para garantir a consistencia dos callbacks de AoE
            base.Use(user, targets, shouldOverride1, value1, shouldOverride2, value2);
        }
    }
}