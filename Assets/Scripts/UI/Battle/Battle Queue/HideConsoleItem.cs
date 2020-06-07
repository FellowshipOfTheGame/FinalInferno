using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.Battle.QueueMenu
{
    /// <summary>
	/// Item que esconde o console quando é selecionado.
	/// </summary>
    public class HideConsoleItem : MonoBehaviour
    {
        [SerializeField] private AII.AxisInteractableItem item;
        [SerializeField] private Animator consoleAnim;
        void Awake(){
            if(item == null){
                item = GetComponent<AII.AxisInteractableItem>();
            }

            if(item != null){
                item.OnEnter += HideConsole;
            }
        }
        void HideConsole()
        {
            // Esconde o console
            if(consoleAnim){
                consoleAnim.SetTrigger("HideConsole");
            }
        }
    }

}