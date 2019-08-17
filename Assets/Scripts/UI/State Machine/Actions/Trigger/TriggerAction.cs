using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Ação que invoca um trigger de um animator.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Trigger")]
    public class TriggerAction : ComponentRequester
    {
        /// <summary>
        /// Referência ao animator.
        /// </summary>
        private Animator animator;

        /// <summary>
        /// Nome do parâmetro a ser chamado.
        /// </summary>
        [SerializeField] private string trigger;

        /// <summary>
        /// Executa uma ação.
        /// Chama o trigger.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller)
        {
            animator.SetTrigger(trigger);
            AudioSource src = animator.GetComponent<AudioSource>();
            if(trigger == "Enable" && src != null){
                foreach(AudioSource source in FindObjectsOfType<AudioSource>()){
                    if(source.isPlaying)
                        source.Stop();
                }
                src.Play();
            }
        }

        /// <summary>
        /// Função chamada para pedir um componente ao provedor.
        /// Requisita um animator.
        /// </summary>
        /// <param name="provider"> Game object que provê o componente desejado. </param>
        public override void RequestComponent(GameObject provider)
        {
            animator = provider.GetComponent<Animator>();
        }

    }

}
