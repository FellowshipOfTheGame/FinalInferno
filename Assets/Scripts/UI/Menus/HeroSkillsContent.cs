using System.Collections;
using UnityEngine;

namespace FinalInferno.UI.SkillsMenu {
    public class HeroSkillsContent : MonoBehaviour {
        [SerializeField] private RectTransform content;
        [SerializeField] private RectTransform viewport;
        private RectTransform rectTransform;
        private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        private bool runningCoroutine = false;
        private bool updatePending = false;
        private IEnumerator clampCoroutine = null;

        private void OnEnable() {
            if (!updatePending)
                return;
            updatePending = false;
            StartClampCoroutine();
        }

        private void StartClampCoroutine() {
            clampCoroutine = ClampContentCoroutine();
            StartCoroutine(clampCoroutine);
        }

        private void OnDisable() {
            StopClampCoroutine();
        }

        private void StopClampCoroutine() {
            if (clampCoroutine == null)
                return;
            StopCoroutine(clampCoroutine);
            clampCoroutine = null;
        }

        public void ClampContent(RectTransform rect) {
            rectTransform = rect;
            if (isActiveAndEnabled && !runningCoroutine)
                StartClampCoroutine();
            else
                updatePending = true;
        }

        private IEnumerator ClampContentCoroutine() {
            runningCoroutine = true;
            yield return waitForEndOfFrame;
            float itemPos = rectTransform.localPosition.y;
            float curPos = content.localPosition.y;
            float itemHeight = rectTransform.rect.height;
            float viewportHeight = viewport.rect.height;
            float minYPosition = -viewportHeight - itemPos + itemHeight / 2;
            float maxYPosition = -itemPos - itemHeight / 2;
            float newYPosition = Mathf.Clamp(curPos, minYPosition, maxYPosition);
            content.localPosition = new Vector3(content.localPosition.x, newYPosition, content.localPosition.z);
            runningCoroutine = false;
        }
    }

}