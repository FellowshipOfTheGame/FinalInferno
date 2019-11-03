using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Decisão baseada na capacidade na unidade atual de agir.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Can Unit Act")]
    public class CanUnitAct : Decision
    {
        /// <summary>
        /// A situação desejada para a unidade.
        /// </summary>
        [SerializeField] private bool canAct;

        /// <summary>
        /// Verifica se a unidade está na situação desejada.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override bool Decide(StateController controller)
        {
            return (canAct == BattleManager.instance.currentUnit.CanAct);
        }

    }

}
