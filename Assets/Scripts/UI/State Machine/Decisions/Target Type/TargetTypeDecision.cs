using System.Collections.Generic;
using FinalInferno.UI.Battle;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    /// <summary>
    /// Decisão baseada no tipo de unidade que está no turno.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Target Type")]
    public class TargetTypeDecision : Decision {
        /// <summary>
        /// Tipo desejado.
        /// </summary>
        [SerializeField] private List<TargetType> desiredTypes;

        /// <summary>
        /// Verifica se a decisão ativou.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override bool Decide(StateController controller) {
            bool desiredType = false;
            TargetType currentSkillType = BattleSkillManager.GetSkillType();

            foreach (TargetType type in desiredTypes) {
                bool hasThisType = (currentSkillType == type);

                desiredType = desiredType || hasThisType;
            }

            return desiredType;
        }

    }

}
