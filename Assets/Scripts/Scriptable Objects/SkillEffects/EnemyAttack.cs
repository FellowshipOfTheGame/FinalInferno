using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "EnemyAttack", menuName = "ScriptableObject/SkillEffect/EnemyAttack")]
    public class EnemyAttack : SkillEffect {
        // value1 = dmg multiplier
        public override string Description1{ get{ return "x Damage"; } }
        public override string Description2{ get{ return null; } }
        public override void Apply(BattleUnit source, BattleUnit target) {
            DamageType dmgType = (source.unit.GetType() == typeof(Enemy))? ((Enemy)source.unit).DamageFocus : DamageType.None;
            Element element = (source.unit.GetType() == typeof(Enemy))? ((Enemy)source.unit).Element : Element.Neutral;
            target.TakeDamage(source.curDmg, value1, dmgType, element, source);
        }
    }
}
