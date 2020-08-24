using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Componente que implementa um estado da máquina de estados.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/State")]
    public class State : ScriptableObject
    {
        /// <summary>
        /// Transições correspondentes ao estado.
        /// </summary>
        public Transition[] transitions;

        /// <summary>
        /// Função chamada todo frame que o estado está ativado.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public void UpdateState(StateController controller)
        {
            CheckTransitions(controller);
        }

        /// <summary>
        /// Verifica as decisões das transições e a executa se necessário.
        /// </summary>
        /// <param name="controller"> O controlador da máquinda de estados. </param>
        private void CheckTransitions(StateController controller)
        {
            foreach (Transition T in transitions)
            {
                bool decisionSucceeded = true;
                foreach (Decision D in T.decisions)
                {
                    decisionSucceeded = decisionSucceeded && D.Decide(controller);
                }

                if (decisionSucceeded)
                {
                    controller.SetNextState(T.nextState, T.actions);
                }
            }
        }

    }

}
