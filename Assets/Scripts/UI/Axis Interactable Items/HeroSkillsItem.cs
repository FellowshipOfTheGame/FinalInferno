using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.FSM;
using FinalInferno.UI.SkillsMenu;

namespace FinalInferno.UI.AII
{
    public class HeroSkillsItem : MonoBehaviour
    {
        [SerializeField] private AxisInteractableItem item;
        [SerializeField] private AIIManager skillsManager;

        [SerializeField] private int index;
        [SerializeField] private SkillsContent content;
        [SerializeField] private ComponentProvider provider;

        private void Awake()
        {
            item.OnEnter += EnableFirstSkillDescription;
            item.OnEnter += UpdateSkillsContentPosition;
            item.OnEnter += UpdateAIIReference;
            item.OnExit += DisableSkills;
        }

        private void UpdateAIIReference(){
            provider.UpdateComponent(skillsManager.gameObject);
        }

        private void EnableFirstSkillDescription()
        {
            skillsManager.Active();
        }

        private void DisableSkills()
        {
            skillsManager.Deactive();
        }

        private void UpdateSkillsContentPosition()
        {
            content.SetContentToPosition(index);
        }
    }
}
