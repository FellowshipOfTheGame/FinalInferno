using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Decisão baseada se a batalha acabou com vitoria, derrota ou não acabou.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/End Battle")]
    public class EndBattleDecision : Decision
    {
        /// <summary>
        /// Tipo desejado.
        /// </summary>
        [SerializeField] private VictoryType desiredType;

        /// <summary>
        /// Verifica se a decisão ativou.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override bool Decide(StateController controller)
        {
            return (BattleManager.instance.CheckEnd() == desiredType);
        }

    }

}
