using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Decision based in whether a key is pressed or not.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Axis")]
    public class AxisDecision : Decision
    {
        /// <summary>
        /// Key to be pressed to activate the trigger.
        /// </summary>
        [SerializeField] private string activatorAxis;

        /// <summary>
        /// Verify if the decision triggers.
        /// </summary>
        /// <param name="controller"> The Finite State Machine controller. </param>
        public override bool Decide(StateController controller)
        {
            return (Input.GetAxisRaw(activatorAxis) != 0);
        }

    }

}
