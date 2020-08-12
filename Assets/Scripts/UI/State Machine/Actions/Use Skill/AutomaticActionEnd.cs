using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Ação que muda o estado de um botão.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Automatic Action End")]
    public class AutomaticActionEnd : Action
    {
        [SerializeField] private float delay = 0f;
        /// <summary>
        /// Executa uma ação.
        /// Indica que deve esperar a animação de skill acabar.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller)
        {
            AnimationEnded.StartAnimation();
            BattleManager.instance.StartCoroutine(EndAnimationAfterSeconds(delay));
        }

        IEnumerator EndAnimationAfterSeconds(float time){
            yield return new WaitForSeconds(time);
            AnimationEnded.EndAnimation();
        }

    }

}