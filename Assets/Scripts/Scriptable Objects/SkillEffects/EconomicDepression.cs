using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "EconomicDepression", menuName = "ScriptableObject/SkillEffect/EconomicDepression")]
    public class EconomicDepression : SkillEffect {
        // value1 = damage multiplier for DoT
        // value2 = element for DoT
        public override string Description {
            get {
                string desc = "Applies Market Crash to the source with all enemies as targets. Each turn after applying, source damage taken and enemies' healing received are reduced, and enemies take" + value1 + "x " + DmgType + " pure damage";
                return desc;
            }
        }
        private string DmgType {
            get {
                string value = "\b";
                Element element = (Element)(Mathf.Clamp((int)value2, 1, (int)Element.Neutral));
                switch (element) {
                    case Element.Fire:
                        value = "Fire";
                        break;
                    case Element.Water:
                        value = "Water";
                        break;
                    case Element.Wind:
                        value = "Wind";
                        break;
                    case Element.Earth:
                        value = "Earth";
                        break;
                    case Element.Neutral:
                        value = "Neutral";
                        break;
                }
                return value;
            }
        }

        public override void Apply(BattleUnit source, BattleUnit target) {
            List<BattleUnit> enemies = BattleManager.instance.GetEnemies(source, true);
            for (int i = 0; i < enemies.Count; i++) {
                source.AddEffect(new MarketCrash(source, enemies[i], value1, DamageType.None, (Element)(Mathf.Clamp((int)value2, 1, (int)Element.Neutral))), true);
            }
        }
    }
}