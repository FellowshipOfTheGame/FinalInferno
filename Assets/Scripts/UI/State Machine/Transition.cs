namespace FinalInferno.UI.FSM {
    /// <summary>
    /// Componente que guarda especificações da transição.
    /// </summary>
    [System.Serializable]
    public class Transition {
        /// <summary>
        /// Decisões para ativar a transição.
        /// Todas as decisões devem estar ativas para ativar a transição.
        /// </summary>
        public Decision[] decisions;

        /// <summary>
        /// O próximo estado ao ativar a transição.
        /// </summary>
        public State nextState;

        /// <summary>
        /// Ações que serão executadas durante a transição.
        /// </summary>
        public Action[] actions;
    }

}