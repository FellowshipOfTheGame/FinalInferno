using UnityEngine;

namespace FinalInferno.UI.FSM {
    /// <summary>
    /// Componente base para as decisões.
    /// </summary>
    public abstract class Decision : ScriptableObject {
        /// <summary>
        /// Verifica se a decisão ativou.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public abstract bool Decide(StateController controller);
    }

}
