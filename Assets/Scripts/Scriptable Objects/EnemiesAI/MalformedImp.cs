using UnityEngine;

namespace FinalInferno {

    [CreateAssetMenu(fileName = "Malformed Imp", menuName = "ScriptableObject/Enemy/MalformedImp")]
    public class MalformedImp : Enemy {
        private Skill ImpSkill => skills[0];

        public override Skill AttackDecision() {
            float roll = Random.Range(0.0f, 1.0f);
            if (roll < 0.7f)
                return ImpSkill;
            return attackSkill;
        }
    }
}