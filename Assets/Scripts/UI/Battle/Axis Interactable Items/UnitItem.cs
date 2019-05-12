﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.Battle;

namespace FinalInferno.UI.AII
{
    /// <summary>
	/// Item da lista de efeitos.
	/// </summary>
    public class UnitItem : MonoBehaviour
    {
        /// <summary>
        /// Referência ao efeito do item.
        /// </summary>
        public BattleUnit unit;

        /// <summary>
        /// Referência ao item da lista.
        /// </summary>
        [SerializeField] private AxisInteractableItem item;

        void Awake()
        {
            item.OnEnter += UpdateEnemyContent;
            item.OnAct += SetTarget;
            item.OnExit += ResetEnemyContent;
        }

        private void SetTarget()
        {
            // Debug.Log("Setting target: " + unit.unit.name);
            BattleSkillManager.currentTargets.Clear();
            BattleSkillManager.currentTargets.Add(unit);
        }

        private void UpdateEnemyContent()
        {
            BattleManager.instance.enemyContent.ShowEnemyInfo(unit);
        }

        private void ResetEnemyContent()
        {
            BattleManager.instance.enemyContent.ShowAllLives();
        }

    }

}
