using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.Victory;

namespace FinalInferno.UI.AII
{
    public class VictoryHeroItem : MonoBehaviour
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
