using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "IncreaseStatusResistance", menuName = "ScriptableObject/SkillEffect/IncreaseStatusResistance")]
    public class IncreaseStatusResistance : SkillEffect {
        private float StatusResistanceIncrease => value1;
        private int BuffDuration => (int)value2;
        public override string Description {
            get {
                string desc = $"Increase resistance to abnormal effects by {StatusResistanceIncrease * 100}% for ";
                desc += (BuffDuration > 0) ? $"{BuffDuration} turns" : "rest of battle";
                return desc;
            }
        }

        public override void Apply(BattleUnit source, BattleUnit target) {
            if (BuffDuration < 0) {
                target.statusResistance += StatusResistanceIncrease;
            } else {
                target.AddEffect(new StatusResistUp(source, target, StatusResistanceIncrease, BuffDuration));
            }
        }
    }
}