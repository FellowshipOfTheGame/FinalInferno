using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Skeleton", menuName = "ScriptableObject/Enemy/Skeleton")]
    public class Skeleton : Enemy {
        private Skill ImpaleSkill => skills[0];
        public override Skill AttackDecision() {
            float roll = Random.Range(0.0f, 1.0f);
            if (roll < 0.7f)
                return ImpaleSkill;
            return attackSkill;
        }
    }
}
