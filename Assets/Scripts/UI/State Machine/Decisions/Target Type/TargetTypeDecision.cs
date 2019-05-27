using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.Battle;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Decisão baseada no tipo de unidade que está no turno.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Target Type")]
    public class TargetTypeDecision : Decision
    {
        /// <summary>
        /// Tipo desejado.
        /// </summary>
        [SerializeField] private List<TargetType> desiredTypes;

        /// <summary>
        /// Verifica se a decisão ativou.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override bool Decide(StateController controller)
        {
            bool desiredType = false;
            foreach (TargetType type in desiredTypes)
            {
                desiredType = desiredType || (BattleSkillManager.GetSkillType() == type);
            }
            return desiredType;
        }

    }

}
