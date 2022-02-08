using FinalInferno.UI.AII;
using FinalInferno.UI.Battle;
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
            foreach (BattleUnit battleUnit in BattleSkillManager.currentTargets) {
                battleUnit.battleItem.GetComponent<AxisInteractableItem>().DisableReference();
            }

            BattleUnit currentUnit = BattleManager.instance.currentUnit;
            // Quando a unidade morre por usar uma skill ou status effect currentUnit==null
            if (currentUnit == null) {
                currentUnit = BattleSkillManager.currentUser;
                // Quando a unidade morre por status effect currentUnit==null aqui e nada deve ser feito
                if (currentUnit != null) {
                    Debug.Log("Unidade morreu por conta de counter ou algo do tipo");
                    // Eu acho que seria impossível chegar aqui e currentSkill ser null, mas fica a precaução
                    if (BattleSkillManager.currentSkill == null) {
                        BattleSkillManager.currentSkill = currentUnit.Unit.attackSkill;
                    }
                    currentUnit.actionPoints += Mathf.FloorToInt(BattleSkillManager.currentSkill.cost * (1.0f - currentUnit.ActionCostReduction));
                    BattleManager.instance.UpdateQueue(0, true);
                }
            } else {
                Skill skillSelected = BattleSkillManager.currentSkill;
                if (forceAttack || skillSelected == null) {
                    BattleManager.instance.UpdateQueue(Mathf.FloorToInt(currentUnit.Unit.attackSkill.cost * (1.0f - currentUnit.ActionCostReduction)));
                } else {
                    BattleManager.instance.UpdateQueue(Mathf.FloorToInt(BattleSkillManager.currentSkill.cost * (1.0f - currentUnit.ActionCostReduction)));
                }
            }
        }

    }

}
