using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Overseer", menuName = "ScriptableObject/Enemy/Overseer")]
    public class Overseer : Enemy {
        private Skill LaserSkill => skills[0];
        public override Skill AttackDecision() {
            float roll = Random.Range(0.0f, 1.0f);
            if (roll < 0.7f) {
                return LaserSkill;
            }
            return attackSkill;
        }
    }
}
