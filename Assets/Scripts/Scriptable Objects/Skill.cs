using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    //engloba todas as "skills"
    [CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObject/Skill")]
    public class Skill : ScriptableObject, IDatabaseItem {
        public const int maxCost = 20;
        public const int baseCost = 8;
        [Header("Skill")]
        public new string name;
        [SerializeField] protected int level;
        public virtual int Level { get => level; set { } }
        public float cost;
        public bool active = true;
        [TextArea, SerializeField] protected string shortDescription;
        public virtual string ShortDescription => shortDescription;
        public TargetType target;
        public Element attribute;
        [SerializeField] private SkillType type;
        public string TypeString {
            get => type switch {
                    SkillType.Active => "Active Skill",
                    SkillType.PassiveOnDeath => "Passive: Triggered on Death",
                    SkillType.PassiveOnEnd => "Passive: Triggered when Battle Ends",
                    SkillType.PassiveOnReceiveBuff => "Passive: Triggered when Buffed",
                    SkillType.PassiveOnReceiveDebuff => "Passive: Triggered when Debuffed",
                    SkillType.PassiveOnSkillUsed => "Passive: Triggered after Skill Usage",
                    SkillType.PassiveOnSpawn => "Base Status Changes",
                    SkillType.PassiveOnStart => "Passive: Triggered at Start of Battle",
                    SkillType.PassiveOnTakeDamage => "Passive: Triggered when Damage Taken",
                    _ => ""
                };
        }
        public SkillType Type => type;
        [SerializeField] private int callbackDelay = 0;
        public List<SkillEffectTuple> effects;
        [Space(15)]
        [SerializeField] private GameObject visualEffect;
        public GameObject VisualEffect => visualEffect;

        #region IDatabaseItem
        public virtual void LoadTables() { }

        public virtual void Preload() { }
        #endregion

        // TO DO: Would work better as a function of TargetType when they become SOs
        public List<BattleUnit> FilterTargets(BattleUnit source, List<BattleUnit> oldList) {
            List<BattleUnit> newList = new List<BattleUnit>(oldList);
            List<BattleUnit> allies = BattleManager.instance.GetTeam(source, true);
            foreach (BattleUnit unit in oldList) {
                switch (target) {
                    case TargetType.Null:
                    case TargetType.Self:
                        if (unit != source) {
                            newList.Remove(unit);
                        }

                        break;
                    case TargetType.SingleDeadAlly:
                    case TargetType.AllDeadAllies:
                        if (!allies.Contains(unit) || unit.CurHP > 0) {
                            newList.Remove(unit);
                        }

                        break;
                    case TargetType.SingleLiveAlly:
                    case TargetType.AllLiveAllies:
                        if (!allies.Contains(unit) || unit.CurHP <= 0) {
                            newList.Remove(unit);
                        }

                        break;
                    case TargetType.AllAlliesLiveOrDead:
                        if (!allies.Contains(unit)) {
                            newList.Remove(unit);
                        }

                        break;
                    case TargetType.SingleDeadEnemy:
                    case TargetType.AllDeadEnemies:
                        if (allies.Contains(unit) || unit.CurHP > 0) {
                            newList.Remove(unit);
                        }

                        break;
                    case TargetType.SingleLiveEnemy:
                    case TargetType.AllLiveEnemies:
                        if (allies.Contains(unit) || unit.CurHP <= 0) {
                            newList.Remove(unit);
                        }

                        break;
                    case TargetType.AllEnemiesLiveOrDead:
                        if (allies.Contains(unit)) {
                            newList.Remove(unit);
                        }

                        break;
                }
            }
            return newList;
        }

        public virtual void UseCallbackOrDelayed(BattleUnit user, List<BattleUnit> targets, bool shouldOverride1 = false, float value1 = 0f, bool shouldOverride2 = false, float value2 = 0f) {
            if (callbackDelay <= 0) {
                UseCallback(user, targets, shouldOverride1, value1, shouldOverride2, value2);
            } else {
                UseDelayed(user, targets, shouldOverride1, value1, shouldOverride2, value2);
            }
        }

        protected virtual void UseCallback(BattleUnit user, List<BattleUnit> targets, bool shouldOverride1 = false, float value1 = 0f, bool shouldOverride2 = false, float value2 = 0f) {
            foreach (BattleUnit trgt in targets) {
                ShowVisualEffects(trgt);
                ApplyAllSkillEffects(user, trgt, shouldOverride1, value1, shouldOverride2, value2);
            }
        }

        protected virtual void UseDelayed(BattleUnit user, List<BattleUnit> targets, bool shouldOverride1, float value1, bool shouldOverride2, float value2) {
            foreach (BattleUnit target in targets) {
                target.AddEffect(new DelayedSkill(UseCallback, user, target, shouldOverride1, value1, shouldOverride2, value2, callbackDelay));
            }
        }

        protected virtual void ShowVisualEffects(BattleUnit target) {
            if (!visualEffect) {
                return;
            }
            GameObject obj = GameObject.Instantiate(visualEffect, target.transform);
            obj.GetComponent<SkillVFX>().SetTarget(target, true);
        }

        protected virtual void ApplyAllSkillEffects(BattleUnit user, BattleUnit target, bool shouldOverride1, float value1, bool shouldOverride2, float value2) {
            foreach (SkillEffectTuple skillEffect in effects) {
                skillEffect.effect.value1 = (shouldOverride1) ? value1 : skillEffect.value1;
                skillEffect.effect.value2 = (shouldOverride2) ? value2 : skillEffect.value2;
                skillEffect.effect.Apply(user, target);
            }
        }

        public virtual void Use(BattleUnit user, BattleUnit target, bool shouldOverride1 = false, float value1 = 0f, bool shouldOverride2 = false, float value2 = 0f) {
            ApplyAllSkillEffects(user, target, shouldOverride1, value1, shouldOverride2, value2);
        }

        public virtual void ResetSkill() { Debug.LogError("Wrong use of skill reset", this); }
    }
}
