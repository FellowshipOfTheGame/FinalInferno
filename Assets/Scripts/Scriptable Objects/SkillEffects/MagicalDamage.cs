﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "MagicalDamage", menuName = "ScriptableObject/SkillEffect/MagicalDamage")]
    public class MagicalDamage : SkillEffect {
        // value1 = dmg multiplier
        // value2 = element of the damage
        public override string Description { get { return "Deals " + value1 + "x " + DmgType + " magical damage"; } }
        private string DmgType{
            get{
                string value = "\b";
                Element element = (Element)(Mathf.Clamp((int)value2, 1, (int)Element.Neutral));
                switch(element){
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
            target.TakeDamage(source.curDmg, value1, DamageType.Magical, (Element)(Mathf.Clamp((int)value2, 1, (int)Element.Neutral)), source);
        }
    }
}
