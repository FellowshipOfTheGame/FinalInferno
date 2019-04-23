using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.FSM;

namespace FinalInferno.UI.AII
{
    /// <summary>
	/// A type of item that can be clicked.
	/// </summary>
    public class EffectItem : MonoBehaviour
    {
        /// <summary>
        /// Reference to the button click decision SO.
        /// </summary>
        public SkillList skillList;
        private SkillEffect effect;
        private RectTransform rect;

        [SerializeField] private AxisInteractableItem item;

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            
            item.OnEnter += UpdateEffectDescription;
        }

        /// <summary>
        /// Calls the button click decision trigger.
        /// </summary>
        private void UpdateEffectDescription()
        {
            if (effect == null)
                effect = GetComponent<EffectElement>().effect;

            skillList.UpdateEffectDescription(effect);
        }

    }

}
