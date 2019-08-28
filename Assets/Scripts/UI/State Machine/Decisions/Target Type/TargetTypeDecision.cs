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
            bool hasValidTargets = true;
            TargetType currentSkillType = BattleSkillManager.GetSkillType();

            foreach (TargetType type in desiredTypes)
            {
                hasValidTargets = false;
                if(currentSkillType == type){
                    switch(type){
                        case TargetType.AllAllies:
                        case TargetType.AllEnemies:
                        case TargetType.MultiAlly:
                        case TargetType.MultiEnemy:
                        case TargetType.Self:
                        case TargetType.SingleAlly:
                        case TargetType.SingleEnemy:
                            hasValidTargets = true;
                            break;
                        case TargetType.DeadAllies:
                        case TargetType.DeadAlly:
                            List<BattleUnit> deadAllies = BattleManager.instance.GetTeam(BattleManager.instance.currentUnit, true, true);
                            hasValidTargets = deadAllies.Count > 0;
                            break;
                        case TargetType.DeadEnemies:
                        case TargetType.DeadEnemy:
                            List<BattleUnit> deadEnemies = BattleManager.instance.GetEnemies(BattleManager.instance.currentUnit, true, true);
                            hasValidTargets = deadEnemies.Count > 0;
                            break;
                    }
                }

                desiredType = desiredType || hasValidTargets;
            }
            return desiredType;
        }

    }

}
