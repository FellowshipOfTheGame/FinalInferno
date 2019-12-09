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
        private SkillEffectTuple effect;

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
            effect = GetComponent<EffectElement>().effect;

            effect.effect.UpdateValues(effect.value1, effect.value2);
            skillList.UpdateEffectDescription(effect.effect);
        }

    }

}
