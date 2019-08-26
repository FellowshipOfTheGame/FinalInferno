using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.SkillsMenu;

namespace FinalInferno.UI.AII
{
    public class HeroSkillsItem : MonoBehaviour
    {
        [SerializeField] private AxisInteractableItem item;
        [SerializeField] private AIIManager skillsManager;

        [SerializeField] private int index;
        [SerializeField] private SkillsContent content;

        private void Awake()
        {
            item.OnEnter += EnableFirstSkillDescription;
            item.OnEnter += UpdateSkillsContentPosition;
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

        private void UpdateSkillsContentPosition()
        {
            content.SetContentToPosition(index);
        }
    }
}
