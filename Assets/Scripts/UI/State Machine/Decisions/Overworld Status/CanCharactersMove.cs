using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Decisão baseada na capacidade dos personagens de se moverem no overworld.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Can Party Move")]
    public class CanCharactersMove : Decision
    {
        /// <summary>
        /// A situação desejada para a unidade.
        /// </summary>
        [SerializeField] private bool canMove;

        /// <summary>
        /// Verifica se a unidade está na situação desejada.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override bool Decide(StateController controller)
        {
            return (canMove == CharacterOW.PartyCanMove);
        }

    }

}
