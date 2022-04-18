using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Evil Spirit", menuName = "ScriptableObject/Enemy/EvilSpirit")]
    public class EvilSpirit : Enemy {
        private Skill OminousWindSkill => skills[0];
        public override Skill AttackDecision() {
            float roll = Random.Range(0.0f, 1.0f);
            if (roll < 0.7f)
                return OminousWindSkill;
            return attackSkill;
        }
    }
}
