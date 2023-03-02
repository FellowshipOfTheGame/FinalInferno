using UnityEngine;

namespace FinalInferno {

    [CreateAssetMenu(fileName = "EnemySingleActiveSkill", menuName = "ScriptableObject/Enemy/SingleActiveSkill")]
    public class EnemySingleActiveSkillUser : Enemy {
        private EnemySkill ActiveSkill => (EnemySkill)skills[0];

        public override Skill AttackDecision() {
            Debug.LogWarning($"Probability: {ActiveSkill.Probability}");
            float roll = Random.Range(0.0f, 1.0f);
            return roll < ActiveSkill.Probability ? ActiveSkill : attackSkill;
        }
    }
}