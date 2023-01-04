using FinalInferno.UI.Battle.SkillMenu;
using UnityEngine;

namespace FinalInferno.UI.AII {
    public class EffectListItem : MonoBehaviour {
        public SkillList skillList;
        private EffectElement effectElement;
        [SerializeField] private AxisInteractableItem item;

        private void Awake() {
            effectElement = GetComponent<EffectElement>();
            item.OnEnter += UpdateEffectDescription;
        }

        private void UpdateEffectDescription() {
            effectElement.EffectTuple.UpdateValues();
            skillList.UpdateEffectDescription(effectElement.EffectTuple.effect);
        }

    }

}
