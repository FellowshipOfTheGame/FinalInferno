using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.FSM;

namespace FinalInferno.UI.AII
{
    /// <summary>
	/// A type of item that can be clicked.
	/// </summary>
    public class SkillItem : ClickableItem
    {
        /// <summary>
        /// Reference to the button click decision SO.
        /// </summary>
        public SkillList skillList;
        private PlayerSkill skill;
        private RectTransform rect;

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            
            OnEnter += EnableGO;
            OnExit += DisableGO;
            OnEnter += UpdateSkillDescription;
            OnEnter += ClampSkillContent;
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
