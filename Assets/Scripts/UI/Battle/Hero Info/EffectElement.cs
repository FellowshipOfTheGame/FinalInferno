using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Battle.SkillMenu
{
    /// <summary>
    /// Classe que guarda o scriptable object do efeito referente ao item.
    /// </summary>
    public class EffectElement : MonoBehaviour
    {
        /// <summary>
        /// Scriptable object do efeito.
        /// </summary>
        public SkillEffect effect {get; private set;}

        [Header("UI elements")]
        /// <summary>
        /// Campo de imagem referente à imagem do efeito.
        /// </summary>
        [SerializeField] private Image effectImage;

        /// <summary>
        /// Inicializa o item de efeito.
        /// </summary>
        /// <param name="newEffect"> Efeito a ser inicializado. </param>
        public void UpdateEffect(SkillEffectTuple newEffect)
        {
            effect = newEffect.effect;
            effectImage.sprite = effect.Icon;
        }

    }

}