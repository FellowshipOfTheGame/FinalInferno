using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "EnemyAttack", menuName = "ScriptableObject/SkillEffect/EnemyAttack")]
    public class EnemyAttack : SkillEffect {
        private float DmgMultiplier => value1;
        public override string Description => "";

        public override void Apply(BattleUnit source, BattleUnit target) {
            Enemy sourceAsEnemy = source.Unit as Enemy;
            DamageType dmgType = sourceAsEnemy ? sourceAsEnemy.DamageFocus : DamageType.None;
            Element element = sourceAsEnemy ? sourceAsEnemy.Element : Element.Neutral;
            target.TakeDamage(source.curDmg, DmgMultiplier, dmgType, element, source);
        }
    }
}
