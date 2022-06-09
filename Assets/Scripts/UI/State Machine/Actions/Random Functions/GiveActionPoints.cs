using UnityEngine;

namespace FinalInferno.UI.FSM {
    /// <summary>
    /// Ação que recoloca a unidade atual na fila como se ela tivesse atacado.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Give Action Points")]
    public class GiveActionPoints : Action {
        /// <summary>
        /// Executa uma ação.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>

        [SerializeField] private bool forceAttack = false;
        public override void Act(StateController controller) {
            foreach (BattleUnit battleUnit in BattleSkillManager.CurrentTargets) {
                battleUnit.StopShowingThisAsATarget();
            }

            BattleUnit currentUnit = BattleManager.instance.CurrentUnit;
            // Quando a unidade morre por usar uma skill ou status effect currentUnit==null
            if (currentUnit == null) {
                currentUnit = BattleSkillManager.CurrentUser;
                // Quando a unidade morre por status effect currentUnit==null aqui e nada deve ser feito
                if (currentUnit != null) {
                    Debug.Log("Unidade morreu por conta de counter ou algo do tipo");
                    // Eu acho que seria impossível chegar aqui e currentSkill ser null, mas fica a precaução
                    if (BattleSkillManager.CurrentSkill == null) {
                        BattleSkillManager.SelectSkill(currentUnit.Unit.attackSkill);
                    }
                    currentUnit.actionPoints += Mathf.FloorToInt(BattleSkillManager.CurrentSkill.cost * (1.0f - currentUnit.ActionCostReduction));
                    BattleManager.instance.EndTurn();
                }
            } else {
                Skill skillSelected = BattleSkillManager.CurrentSkill;
                if (forceAttack || skillSelected == null) {
                    BattleManager.instance.UpdateQueue(Mathf.FloorToInt(currentUnit.Unit.attackSkill.cost * (1.0f - currentUnit.ActionCostReduction)));
                } else {
                    BattleManager.instance.UpdateQueue(Mathf.FloorToInt(BattleSkillManager.CurrentSkill.cost * (1.0f - currentUnit.ActionCostReduction)));
                }
            }
        }

    }

}
