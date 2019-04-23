using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.FSM;

namespace FinalInferno.UI.AII
{
    /// <summary>
	/// A type of item that can be clicked.
	/// </summary>
    public class SkillItem : MonoBehaviour
    {
        /// <summary>
        /// Reference to the button click decision SO.
        /// </summary>
        public SkillList skillList;
        private PlayerSkill skill;
        private RectTransform rect;

        [SerializeField] private AxisInteractableItem item;

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            
            item.OnEnter += UpdateSkillDescription;
            item.OnEnter += ClampSkillContent;
        }

        /// <summary>
        /// Calls the button click decision trigger.
        /// </summary>
        private void UpdateSkillDescription()
        {
            if (skill == null)
                skill = GetComponent<SkillElement>().skill;

            skillList.UpdateSkillDescription(skill);
        }

        private void ClampSkillContent()
        {
            skillList.ClampSkillContent(rect);
        }
    }

}
