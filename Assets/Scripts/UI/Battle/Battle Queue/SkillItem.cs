using FinalInferno.UI.AII;
using FinalInferno.EventSystem;
using UnityEngine;

namespace FinalInferno.UI.Battle.QueueMenu {
    public class SkillItem : MonoBehaviour {
        public Skill skill;
        [SerializeField] protected AxisInteractableItem item;
        [SerializeField] private EventFI stopQueuePreviewEvent;
        [SerializeField] private IntEventFI startQueuePreviewEvent;
        private BattleUnit CurrentUnit => BattleManager.instance.CurrentUnit;

        public void Awake() {
            item.OnEnter += StartPreview;
            item.OnExit += StopPreview;
            item.OnAct += UseSkill;
        }

        private void StartPreview() {
            int calculatedActionPoints = CurrentUnit.actionPoints + Mathf.FloorToInt((1.0f - CurrentUnit.ActionCostReduction) * skill.cost);
            startQueuePreviewEvent.Raise(calculatedActionPoints);
        }

        private void StopPreview() {
            stopQueuePreviewEvent.Raise();
        }

        protected void UseSkill() {
            BattleSkillManager.SelectSkill(skill);
            BattleSkillManager.SetTargets(skill.FilterTargets(BattleSkillManager.CurrentUser, BattleManager.instance.battleUnits));
        }
    }

}
