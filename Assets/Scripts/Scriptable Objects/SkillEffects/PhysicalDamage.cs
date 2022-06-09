using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "PhysicalDamage", menuName = "ScriptableObject/SkillEffect/PhysicalDamage")]
    public class PhysicalDamage : SkillEffect {
        private float DmgMultiplier => value1;
        private Element DmgElement => (Element)Mathf.Clamp((int)value2, 1, (int)Element.Neutral);
        private string DmgTypeString => DmgElement switch {
            Element.Fire => "Fire",
            Element.Water => "Water",
            Element.Wind => "Wind",
            Element.Earth => "Earth",
            Element.Neutral => "Neutral",
            _ => "\b",
        };
        public override string Description => $"Deals {DmgMultiplier}x {DmgTypeString} physical damage";
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.TakeDamage(source.CurDmg, DmgMultiplier, DamageType.Physical, DmgElement, source);
        }
    }
}
