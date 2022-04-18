using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "SummonVengeful", menuName = "ScriptableObject/SkillEffect/Summon Vengeful")]
    public class SummonVengeful : SkillEffect {
        private float GhostDmgModifier => value1;
        private int StatusDuration => (int)value2;
        public override string Description {
            get {
                string description = $"Summons target as a vengeful ghost with {GhostDmgModifier}x damage";
                description += (StatusDuration < 0) ? " until the end of the battle or" : $" for {StatusDuration} turns or until";
                description += " unit is revived";
                return description;
            }
        }

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new VengefulGhost(source, target, GhostDmgModifier, StatusDuration));
        }
    }
}