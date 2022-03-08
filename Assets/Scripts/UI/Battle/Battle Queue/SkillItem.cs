using System.Collections.Generic;
using FinalInferno.UI.AII;
using UnityEngine;

namespace FinalInferno.UI.Battle.QueueMenu {
    /// <summary>
	/// Item que é utilizado para ativar uma skill.
	/// </summary>
    public class SkillItem : MonoBehaviour {
        /// <summary>
        /// Referência à skill do item.
        /// </summary>
        public Skill skill;

        /// <summary>
        /// Referência ao item da lista.
        /// </summary>
        [SerializeField] protected AxisInteractableItem item;

        public void Awake() {
            item.OnEnter += StartPreview;
            item.OnExit += StopPreview;
            item.OnAct += UseSkill;
        }

        /// <summary>
        /// Coloca um marcador na posição da lista onde o personagem ficará quando utilizar a referente skill.
        /// </summary>
        private void StartPreview() {
            BattleManager.instance.queue.PreviewPosition(BattleManager.instance.currentUnit.actionPoints
                                                 + Mathf.FloorToInt((1.0f - BattleManager.instance.currentUnit.ActionCostReduction) * skill.cost));
        }

        /// <summary>
        /// Retira o marcador da posição.
        /// </summary>
        private void StopPreview() {
            BattleManager.instance.queue.StopPreview();
        }

        protected void UseSkill() {
            BattleSkillManager.currentSkill = skill;
            BattleSkillManager.currentTargets = skill.FilterTargets(BattleSkillManager.currentUser, BattleManager.instance.battleUnits);
        }

        // Função provavelmente obsoleta
        private List<BattleUnit> GetTargets(TargetType type) {
            List<BattleUnit> targets = new List<BattleUnit>();

            switch (type) {
                case TargetType.Self:
                    targets.Add(BattleManager.instance.currentUnit);
                    break;
                case TargetType.AllLiveAllies:
                    targets = BattleManager.instance.GetTeam(UnitType.Hero);
                    break;
                case TargetType.AllLiveEnemies:
                    targets = BattleManager.instance.GetTeam(UnitType.Enemy);
                    break;
            }

            return targets;
        }

    }

}
