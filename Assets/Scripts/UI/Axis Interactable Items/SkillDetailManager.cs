using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FinalInferno.UI.SkillsMenu;

namespace FinalInferno.UI.AII {
    [RequireComponent(typeof(ScrollRect))]
    public class SkillDetailManager : AIIManager {
        [SerializeField] private List<SkillDetailPage> detailPages = new List<SkillDetailPage>();
        [SerializeField] private ToggleItem toggle = null;
        private List<float> currentScrollValues = null;
        private PlayerSkill currentSkill = null;
        public PlayerSkill CurrentSkill {
            set {
                currentSkill = value;
                toggle.Toggle(currentSkill.active);
            }
        }

        private int currentIndex = 0;
        [SerializeField] private ScrollRect scrollRect = null;
        [SerializeField] private KeyboardScrollbar scrollbar = null;
        [SerializeField] private GameObject rightArrow;
        [SerializeField] private GameObject leftArrow;
        [SerializeField] private GameObject xIndicator;

        private void Reset() {
            scrollRect = GetComponent<ScrollRect>();
        }

        public new void Awake() {
            currentItem = null;
            rightArrow.SetActive(false);
            leftArrow.SetActive(false);
            scrollRect = this.GetComponentIfNull(scrollRect);
            scrollbar.scrollRect = scrollRect;
            currentScrollValues = new List<float>();
            for (int i = 0; i < detailPages.Count; i++) {
                detailPages[i].Index = i;
                detailPages[i].FocusPage = FocusOn;
                if (!toggle && detailPages[i].GetType() == typeof(ToggleItem))
                    toggle = detailPages[i].AII as ToggleItem;
                currentScrollValues.Add(1f);
            }
            if (toggle)
                toggle.OnToggle += ToggleSkillActive;
        }

        private void ToggleSkillActive() {
            currentSkill.active = !currentSkill.active;
            if (audioSource)
                audioSource.Play();
        }

        public void FocusOn(int index) {
            index = Mathf.Clamp(index, 0, detailPages.Count - 1);
            ShowValidArrowIndicators(index);
            if (index == currentIndex)
                return;
            currentScrollValues[currentIndex] = scrollbar.Value;
            scrollRect.content = detailPages[index].GetComponent<RectTransform>();
            UpdatePagesAnchoredPosition(index);
            currentIndex = index;
            scrollbar.SetValue(currentScrollValues[currentIndex]);
        }

        private void UpdatePagesAnchoredPosition(int index) {
            foreach (SkillDetailPage page in detailPages) {
                if (page.TryGetComponent(out RectTransform rect)) {
                    rect.pivot += new Vector2(currentIndex - index, 0);
                    rect.anchorMin += new Vector2(currentIndex - index, 0);
                    rect.anchorMax += new Vector2(currentIndex - index, 0);
                    rect.anchoredPosition = Vector2.zero;
                }
            }
        }

        private void ShowValidArrowIndicators(int index) {
            if (index < detailPages.Count - 1) {
                rightArrow.SetActive(true);
            } else {
                rightArrow.SetActive(false);
            }
            if (index > 0) {
                leftArrow.SetActive(true);
            } else {
                leftArrow.SetActive(false);
            }
        }

        public void HideToggle() {
            toggle.Hide();
        }

        public void ShowToggle() {
            toggle.Show();
        }

        public override void Activate() {
            xIndicator.SetActive(false);
            ResetIndexAndScrollvalues();
            FocusOn(0);
            base.Activate();
        }

        private void ResetIndexAndScrollvalues() {
            currentIndex = 0;
            for (int i = 0; i < currentScrollValues.Count; i++) {
                currentScrollValues[i] = 1f;
            }
        }

        public override void Deactivate() {
            FocusOn(0);
            ResetIndexAndScrollvalues();
            rightArrow.SetActive(false);
            leftArrow.SetActive(false);
            xIndicator.SetActive(true);
            scrollbar.SetValue(1f);
            base.Deactivate();
        }
    }
}