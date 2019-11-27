using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    //engloba todas as "skills"
    [CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObject/Skill", order = 4)]
    public class Skill : ScriptableObject{
        public new string name; //nome da "skill"
        protected int level; //nivel da "skill"
        public virtual int Level { get{ return level; } set {} }
        public float cost; //tempo que a "skill" custara ao conjurador, em porcentagem da sua velocidade
        public bool active = true; //sinaliza se a "skill" esta ativa ou nao
        public TargetType target; //tipo de alvo da "skill"
        public Element attribute; //elemento da "skill"
        public List<SkillEffectTuple> effects; //lista de efeitos que a "skill" causa e seus valores associados
        [SerializeField] private SkillType type; // Tipo da skill (ativa/passiva e qual tipo de passiva)
        public SkillType Type{ get { return type; } }
        [SerializeField] private GameObject visualEffect; // Prefab contendo uma animação da skill
        public GameObject VisualEffect { get{ return visualEffect; } }

        protected List<BattleUnit> FilterTargets(BattleUnit source, List<BattleUnit> oldList){
            List<BattleUnit> newList = new List<BattleUnit>(oldList);
            List<BattleUnit> allies = BattleManager.instance.GetTeam(source);
            foreach(BattleUnit unit in oldList){
                switch(target){
                    case TargetType.Null:
                    case TargetType.Self:
                        if(unit != source)
                            newList.Remove(unit);
                        break;
                    case TargetType.DeadAlly:
                    case TargetType.DeadAllies:
                        if(!(allies.Contains(unit) && unit.CurHP <= 0))
                            newList.Remove(unit);
                        break;
                    case TargetType.SingleAlly:
                    case TargetType.MultiAlly:
                        if(!(allies.Contains(unit) && unit.CurHP > 0))
                            newList.Remove(unit);
                        break;
                    case TargetType.AllAllies:
                        if(!allies.Contains(unit))
                            newList.Remove(unit);
                        break;
                    case TargetType.DeadEnemy:
                    case TargetType.DeadEnemies:
                        if(allies.Contains(unit) || unit.CurHP > 0)
                            newList.Remove(unit);
                        break;
                    case TargetType.SingleEnemy:
                    case TargetType.MultiEnemy:
                        if(allies.Contains(unit) || unit.CurHP <= 0)
                            newList.Remove(unit);
                        break;
                    case TargetType.AllEnemies:
                        if(allies.Contains(unit))
                            newList.Remove(unit);
                        break;
                }
            }
            return newList;
        }

        // funcao que define como a skill sera usada
        // A versão da função com lista é usada para skills de callback, e invoca o efeito visual
        public virtual void Use(BattleUnit user, List<BattleUnit> targets, bool shouldOverride1 = false, float value1 = 0f, bool shouldOverride2 = false, float value2 = 0f){
            foreach (BattleUnit trgt in targets) {
                
                if(visualEffect){
                    GameObject obj = GameObject.Instantiate(visualEffect, trgt.transform.parent);
                    obj.GetComponent<SpriteRenderer>().sortingOrder = trgt.GetComponent<SpriteRenderer>().sortingOrder + 1;
                }

                foreach (SkillEffectTuple skillEffect in effects) {
                    skillEffect.effect.value1 = (shouldOverride1)? value1 : skillEffect.value1;
                    skillEffect.effect.value2 = (shouldOverride2)? value2 : skillEffect.value2;
                    

                    skillEffect.effect.Apply(user, trgt);
                }
            }
        }
        
        // A versão da função com um único alvo é usado durante o turno normal e é invocada pelo efeito visual
        public virtual void Use(BattleUnit user, BattleUnit target, bool shouldOverride1 = false, float value1 = 0f, bool shouldOverride2 = false,  float value2 = 0f){
            foreach (SkillEffectTuple skillEffect in effects) {
                skillEffect.effect.value1 = (shouldOverride1)? value1 : skillEffect.value1;
                skillEffect.effect.value2 = (shouldOverride2)? value2 : skillEffect.value2;
                
                skillEffect.effect.Apply(user, target);
            }
        }

        public virtual void ResetSkill(){Debug.Log("Reseto skill errado");} 
    }
}
