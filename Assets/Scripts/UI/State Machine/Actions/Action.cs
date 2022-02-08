using UnityEngine;

namespace FinalInferno.UI.FSM {
    /// <summary>
    /// Componente que representa uma ação da máquina de estados.
    /// </summary>
    public abstract class Action : ScriptableObject {
        /// <summary>
        /// Executa uma ação.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public abstract void Act(StateController controller);
    }

}
