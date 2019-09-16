using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Battle.LifeMenu
{

    /// <summary>
    /// Classe responsável por mostrar a vida máxima e atual de um heroi.
    /// </summary>
    public class UnitLifeImage : UnitLife
    {
        public Image unitImage;
        /// <summary>
        /// Atualiza o campo de texto com as informações de vida do heroi e também com sua cor.
        /// </summary>
        public override void UpdateUnitLife()
        {
            unitImage.sprite = thisUnit.unit.Portrait;
            lifeText.text = " - " + thisUnit.CurHP + "/" + thisUnit.MaxHP;
            lifeText.color = thisUnit.unit.color;
        }
    }

}