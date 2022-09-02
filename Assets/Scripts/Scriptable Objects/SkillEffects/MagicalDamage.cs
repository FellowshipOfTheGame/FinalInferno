using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "MagicalDamage", menuName = "ScriptableObject/SkillEffect/MagicalDamage")]
    public class MagicalDamage : SkillEffect {
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
        public override string Description => $"Deals {DmgMultiplier}x {DmgTypeString} magical damage";
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.TakeDamage(source.CurDmg, DmgMultiplier, DamageType.Magical, DmgElement, source);
        }
    }
}
