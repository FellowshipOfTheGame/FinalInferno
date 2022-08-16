using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Give Action Points")]
    public class GiveActionPoints : Action {
        [SerializeField] private bool forceAttack = false;
        private BattleUnit CurrentUnit => BattleManager.instance.CurrentUnit ? BattleManager.instance.CurrentUnit : BattleSkillManager.CurrentUser;
        private bool CurrentUnitIsDead => !BattleManager.instance.CurrentUnit;
        private bool CurrentUnitIsDeadFromEffect => !BattleManager.instance.CurrentUnit && !BattleSkillManager.CurrentUser;
        public override void Act(StateController controller) {
            foreach (BattleUnit battleUnit in BattleSkillManager.CurrentTargets) {
                battleUnit.StopShowingThisAsATarget();
            }

            if (CurrentUnitIsDeadFromEffect)
                return;

            if (CurrentUnitIsDead) {
                if (!BattleSkillManager.CurrentSkill)
                    BattleSkillManager.SelectSkill(CurrentUnit.Unit.attackSkill);
                CurrentUnit.actionPoints += CalculateActionCost(BattleSkillManager.CurrentSkill);
                BattleManager.instance.EndTurn();
            } else {
                Skill skillSelected = forceAttack || !BattleSkillManager.CurrentSkill ? CurrentUnit.Unit.attackSkill : BattleSkillManager.CurrentSkill;
                BattleManager.instance.UpdateQueue(CalculateActionCost(skillSelected));
            }
        }

        private int CalculateActionCost(Skill skillUsed) {
            return Mathf.FloorToInt(skillUsed.cost * (1.0f - CurrentUnit.ActionCostReduction));
        }
    }
}
