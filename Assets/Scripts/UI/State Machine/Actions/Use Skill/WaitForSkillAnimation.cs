using UnityEngine;

namespace FinalInferno.UI.FSM {
    /// <summary>
    /// Ação que muda o estado de um botão.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Wait For Skill To End")]
    public class WaitForSkillAnimation : Action {
        /// <summary>
        /// Executa uma ação.
        /// Indica que deve esperar a animação de skill acabar.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller) {
            AnimationEnded.StartAnimation();
        }

    }

}