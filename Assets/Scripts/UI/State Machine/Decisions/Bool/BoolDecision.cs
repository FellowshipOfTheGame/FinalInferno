using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Bool")]
    public class BoolDecision : Decision {
        [SerializeField] private bool isTrue;

        /// <summary>
        /// Verifica se a decisão ativou.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override bool Decide(StateController controller) {
            return isTrue;
        }

        /// <summary>
        /// Função chamada para ativar a decisão.
        /// </summary>
        public void UpdateValue(bool newValue) {
            isTrue = newValue;
        }
    }

}
