using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.SkillsMenu;

namespace FinalInferno.UI.AII
{
    public class SkillsMenuSkillItem : MonoBehaviour
    {
        [SerializeField] private AxisInteractableItem item;
        private PlayerSkill skill;
        public LoadSkillDescription loader;

        void Awake()
        {
            item.OnEnter += UpdateSkillDescription;
        }

        private void UpdateSkillDescription()
        {
            if (skill == null)
                // skill = GetComponent<UpdatedSkill>().thisSkill;

            loader.LoadSkillInfo(skill);
        }
    }
}
