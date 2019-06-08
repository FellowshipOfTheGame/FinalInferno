using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    //engloba todas as "skills"
    [CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObject/Skill", order = 4)]
    public class Skill : ScriptableObject{
        public new string name; //nome da "skill"
        public int level; //nivel da "skill"
        public int cost; //tempo que a "skill" custara ao conjurador
        public TargetType target; //tipo de alvo da "skill"
        public Element attribute; //elemento da "skill"
        public List<SkillEffectTuple> effects; //lista de efeitos que a "skill" causa e seus valores associados
        [SerializeField] private SkillType type;
        public SkillType Type{ get { return type; } }

        //funcao que define como a skill sera usada
        public virtual void Use(BattleUnit user, List<BattleUnit> targets, bool shouldOverride = false, float value1 = 0f, float value2 = 0f){
            foreach (BattleUnit trgt in targets) {
                foreach (SkillEffectTuple skillEffect in effects) {
                    skillEffect.effect.value1 = (shouldOverride)? value1 : skillEffect.value1;
                    skillEffect.effect.value2 = (shouldOverride)? value2 : skillEffect.value2;
                    
                    skillEffect.effect.Apply(user, trgt);
                }
            }
        }
    }
}
