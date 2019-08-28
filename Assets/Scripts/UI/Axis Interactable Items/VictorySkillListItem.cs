using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.Victory;

namespace FinalInferno.UI.AII
{
    public class VictorySkillListItem : MonoBehaviour
    {
        [SerializeField] private AxisInteractableItem item;
        protected PlayerSkill skill;
        public SkillInfoLoader loader;

        void Awake()
        {
            item.OnEnter += UpdateSkillDescription;
        }

        private void UpdateSkillDescription()
        {
            if (skill == null)
                skill = GetComponent<UpdatedSkill>().thisSkill;

            loader.LoadSkillInfo(skill);
        }
    }
}
