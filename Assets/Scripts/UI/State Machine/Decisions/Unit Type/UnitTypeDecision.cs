using UnityEngine;

namespace FinalInferno.UI.FSM {
    /// <summary>
    /// Decisão baseada no tipo de unidade que está no turno.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Unit Type")]
    public class UnitTypeDecision : Decision {
        /// <summary>
        /// Tipo desejado.
        /// </summary>
        [SerializeField] private UnitType desiredType;

        /// <summary>
        /// Verifica se a decisão ativou.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override bool Decide(StateController controller) {
            return (BattleManager.instance.Turn() == desiredType);
        }

    }

}
