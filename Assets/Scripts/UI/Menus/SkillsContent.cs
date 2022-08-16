using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.SkillsMenu {
    public class SkillsContent : MonoBehaviour {
        [SerializeField] private RectTransform content;
        [SerializeField] private AnimationCurve speedCurve;
        [SerializeField] private float speedMultiplier = 25f;
        [SerializeField] private Canvas canvas;
        [SerializeField] private List<LayoutGroup> childLayoutGroups;

        private float xPosition = 0f;
        private int curIndex = 0;
        private const float skillListWidth = 360f;
        private const float skillListsSpacing = 0f;
        private bool isInPosition = false;
        private bool disabledLayoutGroups = false;

        private void Awake() {
            isInPosition = false;
            curIndex = 0;
            disabledLayoutGroups = false;
        }

        private void OnEnable() {
            if (!disabledLayoutGroups)
                StartCoroutine(DisableLayoutGroupsCoroutine());
        }

        private IEnumerator DisableLayoutGroupsCoroutine() {
            yield return new WaitForEndOfFrame();
            foreach (LayoutGroup layoutGroup in childLayoutGroups) {
                layoutGroup.enabled = false;
            }
            disabledLayoutGroups = true;
        }

        private void Update() {
            bool isContentMisplaced = Mathf.Abs(content.localPosition.x - xPosition) >= (skillListWidth * 0.02f);
            if (!isContentMisplaced)
                return;

            if (!isInPosition) {
                canvas.pixelPerfect = false;
                canvas.overridePixelPerfect = true;
                isInPosition = false;
                float previousPos = content.localPosition.x;
                UpdateContentPosition();
                SkipIfOvershotTargetPosition(previousPos);
            } else {
                SkipToPosition(curIndex);
            }
        }

        private void UpdateContentPosition() {
            float distance = (xPosition - content.localPosition.x) / skillListWidth;
            float speed = Mathf.Clamp(speedCurve.Evaluate(Mathf.Clamp(Mathf.Abs(distance), 0, 1f)), 5f, 100f);
            speed *= distance < 0 ? -speedMultiplier : speedMultiplier;
            content.localPosition += new Vector3(speed * Time.deltaTime, 0f);
        }

        private void SkipIfOvershotTargetPosition(float previousPos) {
            if ((xPosition > previousPos && content.localPosition.x > xPosition) ||
                (xPosition < previousPos && content.localPosition.x < xPosition)) {
                SkipToPosition(curIndex);
            }
        }

        public void SkipToPosition(int index) {
            canvas.pixelPerfect = true;
            canvas.overridePixelPerfect = false;
            curIndex = index;
            xPosition = -index * skillListWidth - (index * skillListsSpacing);
            if (isInPosition)
                return;
            isInPosition = true;
            content.localPosition = new Vector3(xPosition, content.localPosition.y);
        }

        public void SetContentPosition(int index) {
            curIndex = index;
            xPosition = -index * skillListWidth - (index * skillListsSpacing);
            isInPosition = false;
        }
    }
}