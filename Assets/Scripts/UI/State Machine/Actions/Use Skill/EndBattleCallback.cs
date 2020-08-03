using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Ação que muda o estado de um botão.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/End Battle Callbacks")]
    public class EndBattleCallback : Action
    {
        /// <summary>
        /// Executa uma ação.
        /// Indica que deve esperar a animação de skill acabar.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller)
        {
            // Revive todos os heroes para garantir que as funções de callback serão chamadas propriamente
            // Reseta o maxhp das unidades, desfazendo aumentos e reduções causados por skills
            foreach(BattleUnit battleUnit in BattleManager.instance.battleUnits){
                if(battleUnit.unit.IsHero && battleUnit.CurHP <= 0){
                    battleUnit.Revive();
                }
                battleUnit.ResetMaxHP();
            }

            // Chama callback de fim de batalha para todas as unidades passando os heroes como alvos
            foreach(BattleUnit battleUnit in BattleManager.instance.battleUnits){
                // Debug.Log("Chamando o callback da unidade " + battleUnit.name);
                if(battleUnit.OnEndBattle != null)
                    battleUnit.OnEndBattle(battleUnit, BattleManager.instance.GetTeam(UnitType.Hero, true));
            }

            // Calcula a exp ganhada pela party, da a recompensa e registra os inimigos mortos no bestiario
            long xpReward = 0;
            int cerberusCount = 0;
            foreach(BattleUnit battleUnit in BattleManager.instance.battleUnits){
                if(battleUnit.unit is Enemy){
                    Enemy enemy = (Enemy)battleUnit.unit;
                    if(enemy is CerberusHead) cerberusCount++;

                    xpReward += enemy.BaseExp;
                    if(cerberusCount == 0 || (cerberusCount % 3 == 1)){
                        Party.Instance.RegisterKill(enemy);
                    }
                }
            }
            Party.Instance.GiveExp(xpReward);
        }

    }

}