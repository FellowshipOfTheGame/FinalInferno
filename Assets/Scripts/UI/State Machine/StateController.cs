using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
	/// Componente que controla a máquina de estados.
	/// </summary>
    public class StateController : MonoBehaviour
    {
        /// <summary>
        /// Estado atual da máquina.
        /// </summary>
        [SerializeField] private State currentState;

        /// <summary>
        /// Tempo que passou desde o início do estado.
        /// </summary>
        private float stateTimeElapsed;

        /// <summary>
        /// Acões que serão executadas no início da execução.
        /// </summary>
        [SerializeField] private Action[] startActions;

        void Start()
        {
            // Executa as ações iniciais.
            foreach (Action A in startActions)
            {
                A.Act(this);
            }
        }

        void Update()
        {
            // Atualiza o tempo passado e verifica por transições
            stateTimeElapsed += Time.deltaTime;
            currentState.UpdateState(this);
        }

        /// <summary>
        /// Muda o estado atual para o novo e executa as ações da transição.
        /// </summary>
        /// <param name="nextState"> O próximo estado da máquina. </param>
        /// <param name="transitionActions"> Ações que serão executadas na transição. </param>
        public void TransitionToState(State nextState, Action[] transitionActions)
        {
            OnExitState(transitionActions);
            currentState = nextState;
            Debug.Log("New State: " + nextState.name);
        }

        /// <summary>
        /// Verifica se o estado já durou mais que o valor dado.
        /// </summary>
        /// <param name="duration"> A duração a ser verificada. </param>
        public bool CheckIfCountDownElapsed(float duration)
        {
            return (stateTimeElapsed >= duration);
        }

        /// <summary>
        /// Executa as ações de transição.
        /// </summary>
        /// <param name="transitionActions"> Ações executadas na transição. </param>
        private void OnExitState(Action[] transitionActions)
        {
            stateTimeElapsed = 0;
            foreach (Action A in transitionActions)
            {
                A.Act(this);
            }
        }
    }

}
