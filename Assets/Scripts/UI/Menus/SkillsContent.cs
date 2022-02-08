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
            if (!isInPosition && Mathf.Abs(content.localPosition.x - xPosition) >= (skillListWidth * 0.02f)) {
                isInPosition = false;
                float distance = (xPosition - content.localPosition.x) / skillListWidth;
                float speed = Mathf.Clamp(speedCurve.Evaluate(Mathf.Clamp(Mathf.Abs(distance), 0, 1f)), 5f, 100f) * (distance < 0 ? -speedMultiplier : speedMultiplier);
                float previousPos = content.localPosition.x;
                content.localPosition = new Vector3(content.localPosition.x + (speed * Time.deltaTime), content.localPosition.y);
                if ((xPosition > previousPos && content.localPosition.x > xPosition) ||
                    (xPosition < previousPos && content.localPosition.x < xPosition)) {
                    SetContentToPosition(curIndex, true);
                }
            } else {
                SetContentToPosition(curIndex, true);
            }
        }

        public void SkipToPosition(int index) {
            isInPosition = false;
            SetContentToPosition(index, true);
        }

        public void SetContentToPosition(int index, bool skipAnimation = false) {
            curIndex = index;
            xPosition = -index * skillListWidth - (index * skillListsSpacing);
            if (!skipAnimation) {
                isInPosition = false;
            } else if (!isInPosition) {
                isInPosition = true;
                content.localPosition = new Vector3(xPosition, content.localPosition.y);
            }
        }
    }

}