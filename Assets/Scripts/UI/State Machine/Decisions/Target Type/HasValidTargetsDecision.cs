using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Has Valid Targets")]
    public class HasValidTargetsDecision : Decision {
        [SerializeField] private bool value;

        public override bool Decide(StateController controller) {
            TargetType currentSkillType = BattleSkillManager.GetSkillTargetType();

            switch (currentSkillType) {
                case TargetType.AllAlliesLiveOrDead:
                case TargetType.AllEnemiesLiveOrDead:
                case TargetType.AllLiveAllies:
                case TargetType.AllLiveEnemies:
                case TargetType.Self:
                case TargetType.SingleLiveAlly:
                case TargetType.SingleLiveEnemy:
                    return value;
                case TargetType.AllDeadAllies:
                case TargetType.SingleDeadAlly:
                    List<BattleUnit> deadAllies = BattleManager.instance.GetTeam(BattleManager.instance.CurrentUnit, true, true);
                    return value == deadAllies.Count > 0;
                case TargetType.AllDeadEnemies:
                case TargetType.SingleDeadEnemy:
                    List<BattleUnit> deadEnemies = BattleManager.instance.GetEnemies(BattleManager.instance.CurrentUnit, true, true);
                    return value == deadEnemies.Count > 0;
                default:
                    return !value;
            }
        }
    }
}