using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Ação que desativa todos os componentes do tipo SpriteRenderer.
    /// (Isso é uma gambiarra)
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Hide Sprites")]
    public class HideSprites : Action
    {
        /// <summary>
        /// Executa uma ação.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller)
        {
            foreach(SpriteRenderer sr in FindObjectsOfType<SpriteRenderer>()){
                // Debug.Log("Desativando sprite do " + sr.GetComponent<BattleUnit>().unit.name);
                sr.enabled = false;
            }
        }

    }

}