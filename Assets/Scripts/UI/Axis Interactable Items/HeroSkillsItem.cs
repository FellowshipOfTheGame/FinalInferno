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

        private bool isCurrent = false;

        private void Awake()
        {
            item.OnEnter += EnableFirstSkillDescription;
            item.OnEnter += UpdateSkillsContentPosition;
            item.OnEnter += () => RegisterAsCurrent(true);
            item.OnExit += () => RegisterAsCurrent(false);
            item.OnExit += DisableSkills;
        }

        public void LoseFocus(){
            if(isCurrent){
                skillsManager.SetFocus(false);
            }
        }

        public void RegainFocus(){
            if(isCurrent){
                skillsManager.SetFocus(true);
            }
        }

        private void RegisterAsCurrent(bool value){
            isCurrent = value;
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
