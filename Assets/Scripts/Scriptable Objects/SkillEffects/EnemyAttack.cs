using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "EnemyAttack", menuName = "ScriptableObject/SkillEffect/EnemyAttack")]
    public class EnemyAttack : SkillEffect {
        // value1 = dmg multiplier
        public override string Description => "";

        public override void Apply(BattleUnit source, BattleUnit target) {
            DamageType dmgType = (source.Unit is Enemy) ? ((Enemy)source.Unit).DamageFocus : DamageType.None;
            Element element = (source.Unit is Enemy) ? ((Enemy)source.Unit).Element : Element.Neutral;
            target.TakeDamage(source.curDmg, value1, dmgType, element, source);
        }
    }
}
