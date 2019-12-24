using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI;
using FinalInferno.UI.Saves;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Ação que marca os alvos da skill selecionada pelo inimigo.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/LoadPreviews")]
    public class LoadPreviews : Action
    {
        /// <summary>
        /// Executa uma ação.
        /// </summary>
        /// <param name="list"> A lista de slots. </param>
        public SlotsList list = null;

        public override void Act(StateController controller)
        {
            if(list != null){
                list.UpdateSlotsContent(SaveLoader.PreviewAllSlots());
            }
        }

    }
}