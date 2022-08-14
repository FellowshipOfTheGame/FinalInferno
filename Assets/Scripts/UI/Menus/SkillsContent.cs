using UnityEngine;

namespace FinalInferno.UI.SkillsMenu {
    public class SkillsContent : MonoBehaviour {
        [SerializeField] private RectTransform content;
        [SerializeField] private AnimationCurve speedCurve;
        [SerializeField] private float speedMultiplier = 25f;

        private float xPosition = 0f;
        private int curIndex = 0;
        private const float skillListWidth = 360f;
        private const float skillListsSpacing = 0f;
        private bool isInPosition = false;

        private void Awake() {
            isInPosition = false;
            curIndex = 0;
        }

        private void Update() {
            bool isContentMisplaced = Mathf.Abs(content.localPosition.x - xPosition) >= (skillListWidth * 0.02f);
            if (!isContentMisplaced)
                return;

            if (!isInPosition) {
                isInPosition = false;
                float distance = (xPosition - content.localPosition.x) / skillListWidth;
                float speed = Mathf.Clamp(speedCurve.Evaluate(Mathf.Clamp(Mathf.Abs(distance), 0, 1f)), 5f, 100f);
                speed *= distance < 0 ? -speedMultiplier : speedMultiplier;
                float previousPos = content.localPosition.x;
                content.localPosition += new Vector3(speed * Time.deltaTime, 0f);
                SkipIfOvershotTargetPosition(previousPos);
            } else {
                SkipToPosition(curIndex);
            }
        }

        private void SkipIfOvershotTargetPosition(float previousPos) {
            if ((xPosition > previousPos && content.localPosition.x > xPosition) ||
                (xPosition < previousPos && content.localPosition.x < xPosition)) {
                SkipToPosition(curIndex);
            }
        }

        public void SkipToPosition(int index) {
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