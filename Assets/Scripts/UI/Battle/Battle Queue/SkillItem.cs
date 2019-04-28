using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.Battle.QueueMenu
{
    /// <summary>
	/// Item da lista de skills.
	/// </summary>
    public class SkillItem : MonoBehaviour
    {
        /// <summary>
        /// Referência à skill do item.
        /// </summary>
        public Skill skill;

        /// <summary>
        /// Referência ao item da lista.
        /// </summary>
        [SerializeField] private AxisInteractableItem item;

        void Awake()
        {
            item.OnEnter += StartPreview;
            item.OnExit += StopPreview;
        }

        private void StartPreview()
        {
            BattleManager.instance.queueUI.StartPreview(BattleManager.instance.queue.PreviewPosition(BattleManager.instance.currentUnit.actionPoints
                                                 + skill.cost));
        }

        private void StopPreview()
        {
            BattleManager.instance.queueUI.StopPreview();
        }

    }

}
