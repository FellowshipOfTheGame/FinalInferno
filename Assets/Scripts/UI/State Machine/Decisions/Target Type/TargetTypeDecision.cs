using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Target Type")]
    public class TargetTypeDecision : Decision {
        [SerializeField] private List<TargetType> desiredTypes;

        public override bool Decide(StateController controller) {
            bool hasOneOrMoreDesiredType = false;
            TargetType currentSkillType = BattleSkillManager.GetSkillType();
            foreach (TargetType type in desiredTypes) {
                bool hasThisType = currentSkillType == type;
                hasOneOrMoreDesiredType |= hasThisType;
            }
            return hasOneOrMoreDesiredType;
        }
    }
}
