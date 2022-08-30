using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "EconomicDepression", menuName = "ScriptableObject/SkillEffect/EconomicDepression")]
    public class EconomicDepression : SkillEffect {
        private float DoTDamageMultiplier;
        private Element DoTElement => (Element)Mathf.Clamp((int)value2, 1, (int)Element.Neutral);
        public override string Description => $"Applies Market Crash to the source with all enemies as targets. Each turn after applying, source damage taken and enemies' healing received are reduced, and enemies take{value1}x {DmgTypeString} pure damage";
        private string DmgTypeString => DoTElement switch {
            Element.Fire => "Fire",
            Element.Water => "Water",
            Element.Wind => "Wind",
            Element.Earth => "Earth",
            Element.Neutral => "Neutral",
            _ => "\b",
        };

        public override void Apply(BattleUnit source, BattleUnit target) {
            List<BattleUnit> enemies = BattleManager.instance.GetEnemies(source, true);
            for (int i = 0; i < enemies.Count; i++) {
                source.AddEffect(new MarketCrash(source, enemies[i], value1, DamageType.None, DoTElement), true);
            }
        }
    }
}