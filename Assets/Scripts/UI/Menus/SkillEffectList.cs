using UnityEngine;

namespace FinalInferno.UI.AII {
    public class SkillEffectList : AII.AIIManager {
        [Space]
        [SerializeField] private GameObject scrollLeftIndicator = null;
        [SerializeField] private GameObject scrollRightIndicator = null;

        private new void Awake() {
            base.Awake();
        }

        public override void Activate() {
            base.Activate();
            if (scrollLeftIndicator != null) {
                scrollLeftIndicator.SetActive(false);
            }
            if (scrollRightIndicator != null) {
                scrollRightIndicator.SetActive(false);
            }
        }

        public override void Deactivate() {
            base.Deactivate();
            if (scrollLeftIndicator != null) {
                scrollLeftIndicator.SetActive(false);
            }
            if (scrollRightIndicator != null) {
                scrollRightIndicator.SetActive(false);
            }
        }

        // Update is called once per frame
        private void LateUpdate() {
            scrollRightIndicator.SetActive(active && (currentItem != lastItem));
            scrollLeftIndicator.SetActive(active && (currentItem != firstItem));
        }
    }
}