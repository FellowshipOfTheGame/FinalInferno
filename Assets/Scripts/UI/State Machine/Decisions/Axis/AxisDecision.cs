using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Decisão baseada em algum eixo de input.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Axis")]
    public class AxisDecision : Decision
    {
        /// <summary>
        /// Eixo a ser ativado.
        /// </summary>
        [SerializeField] private string activatorAxis;
        public string ActivatorAxis => activatorAxis;

        /// <summary>
        /// Verifica se a decisão ativou.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override bool Decide(StateController controller)
        {
            return Input.GetButtonDown(activatorAxis);
        }

    }

}
