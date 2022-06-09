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
            BattleManager.instance.queue.PreviewPosition(BattleManager.instance.CurrentUnit.actionPoints
                                                 + Mathf.FloorToInt((1.0f - BattleManager.instance.CurrentUnit.ActionCostReduction) * skill.cost));
        }

        /// <summary>
        /// Retira o marcador da posição.
        /// </summary>
        private void StopPreview() {
            BattleManager.instance.queue.StopPreview();
        }

        protected void UseSkill() {
            BattleSkillManager.SelectSkill(skill);
            BattleSkillManager.SetTargets(skill.FilterTargets(BattleSkillManager.CurrentUser, BattleManager.instance.battleUnits));
        }
    }

}
