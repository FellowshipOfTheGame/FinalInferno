using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.AII
{
    public class HeroSkillsItem : MonoBehaviour
    {
        [SerializeField] private AxisInteractableItem item;
        [SerializeField] private AIIManager skillsManager;

        private void Awake()
        {
            item.OnEnter += EnableFirstSkillDescription;
            item.OnExit += DisableSkills;
        }

        private void EnableFirstSkillDescription()
        {
            skillsManager.Active();
        }

        private void DisableSkills()
        {
            skillsManager.Desactive();
        }
    }
}
