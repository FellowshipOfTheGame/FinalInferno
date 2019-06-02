using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.FSM;
using FinalInferno.UI.Battle.SkillMenu;

namespace FinalInferno.UI.AII
{
    /// <summary>
	/// Item da lista de efeitos.
	/// </summary>
    public class EffectListItem : MonoBehaviour
    {
        /// <summary>
        /// Referência à lista de skills.
        /// </summary>
        public SkillList skillList;

        /// <summary>
        /// Referência ao efeito do item.
        /// </summary>
        private SkillEffect effect;

        /// <summary>
        /// Referência ao item da lista.
        /// </summary>
        [SerializeField] private AxisInteractableItem item;

        void Awake()
        {
            item.OnEnter += UpdateEffectDescription;
        }

        /// <summary>
        /// Atualiza a descrição do efeito no menu.
        /// </summary>
        private void UpdateEffectDescription()
        {
            if (effect == null)
                effect = GetComponent<EffectElement>().effect;

            skillList.UpdateEffectDescription(effect);
        }

    }

}
