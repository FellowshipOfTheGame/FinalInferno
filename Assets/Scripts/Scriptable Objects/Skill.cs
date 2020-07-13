using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    //engloba todas as "skills"
    [CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObject/Skill", order = 4)]
    public class Skill : ScriptableObject{
        [Header("Skill")]
        public new string name; //nome da "skill"
        protected int level; //nivel da "skill"
        public virtual int Level { get{ return level; } set {} }
        public float cost; //tempo que a "skill" custara ao conjurador, em porcentagem da sua velocidade
        public bool active = true; //sinaliza se a "skill" esta ativa ou nao
        [TextArea, SerializeField] protected string shortDescription;
        public virtual string ShortDescription { get => shortDescription; }
        public TargetType target; //tipo de alvo da "skill"
        public Element attribute; //elemento da "skill"
        [SerializeField] private SkillType type; // Tipo da skill (ativa/passiva e qual tipo de passiva)
        public string TypeString{
            get{
                switch(type){
                    case SkillType.Active:
                        return "Active Skill";
                    case SkillType.PassiveOnDeath:
                        return "Passive: Triggered on Death";
                    case SkillType.PassiveOnEnd:
                        return "Passive: Triggered when Battle Ends";
                    case SkillType.PassiveOnReceiveBuff:
                        return "Passive: Triggered when Buffed";
                    case SkillType.PassiveOnReceiveDebuff:
                        return "Passive: Triggered when Debuffed";
                    case SkillType.PassiveOnSkillUsed:
                        return "Passive: Triggered after Skill Usage";
                    case SkillType.PassiveOnSpawn:
                        return "Base Status Changes";
                    case SkillType.PassiveOnStart:
                        return "Passive: Triggered at Start of Battle";
                    case SkillType.PassiveOnTakeDamage:
                        return "Passive: Triggered when Damage Taken";
                    default:
                        return "";
                }
            }
        }
        public SkillType Type{ get { return type; } }
        [SerializeField] private int callbackDelay = 0;
        public List<SkillEffectTuple> effects; //lista de efeitos que a "skill" causa e seus valores associados
        [Space(15)]
        [SerializeField] private GameObject visualEffect; // Prefab contendo uma animação da skill
        public GameObject VisualEffect { get{ return visualEffect; } }

        public List<BattleUnit> FilterTargets(BattleUnit source, List<BattleUnit> oldList){
            List<BattleUnit> newList = new List<BattleUnit>(oldList);
            List<BattleUnit> allies = BattleManager.instance.GetTeam(source, true);
            foreach(BattleUnit unit in oldList){
                switch(target){
                    case TargetType.Null:
                    case TargetType.Self:
                        if(unit != source)
                            newList.Remove(unit);
                        break;
                    case TargetType.DeadAlly:
                    case TargetType.DeadAllies:
                        if(!allies.Contains(unit) || unit.CurHP > 0)
                            newList.Remove(unit);
                        break;
                    case TargetType.SingleAlly:
                    case TargetType.MultiAlly:
                        if(!allies.Contains(unit) || unit.CurHP <= 0)
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
            // Debug.Log("Skill utilizada(callback): " + name);
            if(callbackDelay <= 0){
                UseCallback(user, targets, shouldOverride1, value1, shouldOverride2, value2);
            }else{
                foreach(BattleUnit target in targets){
                    target.AddEffect(new DelayedSkill(UseCallback, user, target, shouldOverride1, value1, shouldOverride2, value2, callbackDelay));
                }
            }
        }

        protected virtual void UseCallback(BattleUnit user, List<BattleUnit> targets, bool shouldOverride1 = false, float value1 = 0f, bool shouldOverride2 = false, float value2 = 0f){
            foreach (BattleUnit trgt in targets) {
                
                if(visualEffect){
                    GameObject obj = GameObject.Instantiate(visualEffect, trgt.transform);
                    obj.GetComponent<SkillVFX>().forceCallback = true;
                    obj.GetComponent<SpriteRenderer>().sortingOrder = trgt.GetComponent<SpriteRenderer>().sortingOrder + 2;
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
            // Debug.Log("Skill utilizada: " + name);
        }

        public virtual void ResetSkill(){Debug.Log("Reseto skill errado");} 
    }
}
