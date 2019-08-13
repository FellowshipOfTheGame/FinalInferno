using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.FSM;
using FinalInferno.UI.Battle.SkillMenu;

namespace FinalInferno.UI.AII
{
    /// <summary>
	/// Item da lista de skills.
	/// </summary>
    public class SkillListItem : MonoBehaviour
    {
        /// <summary>
        /// Referência à lista de skills.
        /// </summary>
        public SkillList skillList;

        /// <summary>
        /// Referência à skill do item.
        /// </summary>
        protected PlayerSkill skill;

        /// <summary>
        /// Referência ao retângulo do item.
        /// </summary>
        protected RectTransform rect;

        /// <summary>
        /// Referência ao item da lista.
        /// </summary>
        [SerializeField] protected AxisInteractableItem item;

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            
            item.OnEnter += UpdateSkillDescription;
            item.OnEnter += ClampSkillContent;
        }

        /// <summary>
        /// Atualiza a descrição da skill no menu.
        /// </summary>
        private void UpdateSkillDescription()
        {
            if (skill == null)
                skill = GetComponent<SkillElement>().skill;

            skillList.UpdateSkillDescription(skill);
        }

        /// <summary>
        /// Atualiza a posição do content das skills.
        /// </summary>
        private void ClampSkillContent()
        {
            skillList.ClampSkillContent(rect);
        }
    }

}
