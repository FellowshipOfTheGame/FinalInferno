using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.Battle.QueueMenu
{
    /// <summary>
	/// Item que é utilizado para ativar uma skill.
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

        /// <summary>
        /// Coloca um marcador na posição da lista onde o personagem ficará quando utilizar a referente skill.
        /// </summary>
        private void StartPreview()
        {
            BattleManager.instance.queueUI.StartPreview(BattleManager.instance.queue.PreviewPosition(BattleManager.instance.currentUnit.actionPoints
                                                 + skill.cost));
        }

        /// <summary>
        /// Retira o marcador da posição.
        /// </summary>
        private void StopPreview()
        {
            BattleManager.instance.queueUI.StopPreview();
        }

    }

}
