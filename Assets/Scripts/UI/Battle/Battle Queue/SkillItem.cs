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
            item.OnAct += UseSkill;
        }

        /// <summary>
        /// Coloca um marcador na posição da lista onde o personagem ficará quando utilizar a referente skill.
        /// </summary>
        private void StartPreview()
        {
            BattleManager.instance.queueUI.StartPreview(BattleManager.instance.queue.PreviewPosition(BattleManager.instance.currentUnit.actionPoints
                                                 + Mathf.FloorToInt((1.0f - BattleManager.instance.currentUnit.ActionCostReduction) * skill.cost)));
        }

        /// <summary>
        /// Retira o marcador da posição.
        /// </summary>
        private void StopPreview()
        {
            BattleManager.instance.queueUI.StopPreview();
        }

        private void UseSkill()
        {
            // Debug.Log("Defesa Antes = " + BattleManager.instance.currentUnit.curDef);
            BattleSkillManager.currentSkill = skill;
            BattleSkillManager.currentUser = BattleManager.instance.currentUnit;
            if (skill.target == TargetType.Self || skill.target == TargetType.MultiAlly || skill.target == TargetType.MultiEnemy)
            {
                // Debug.Log(BattleSkillManager.currentSkill);
                BattleSkillManager.currentTargets = GetTargets(skill.target);
                // BattleSkillManager.UseSkill();
                // Debug.Log(BattleSkillManager.currentTargets);
            }
            // Debug.Log("Defesa Depois = " + BattleManager.instance.currentUnit.curDef);
        }

        private List<BattleUnit> GetTargets(TargetType type)
        {
            List<BattleUnit> targets = new List<BattleUnit>();

            switch (type)
            {
                case TargetType.Self : 
                    targets.Add(BattleManager.instance.currentUnit);
                    break;
                case TargetType.MultiAlly : 
                    targets = BattleManager.instance.GetTeam(UnitType.Hero);
                    break;
                case TargetType.MultiEnemy :
                    targets = BattleManager.instance.GetTeam(UnitType.Enemy);
                    break;
            }

            return targets;
        }

    }

}
