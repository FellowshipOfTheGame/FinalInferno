using UnityEngine;

namespace FinalInferno.UI.SkillsMenu {
    public class HeroSkillsContent : MonoBehaviour {
        [SerializeField] private RectTransform content;
        [SerializeField] private RectTransform viewport;
        private bool descriptionSelected = false;

        public void ClampContent(RectTransform rect) {
            float itemPos = rect.localPosition.y;
            float curPos = content.localPosition.y;
            float itemHeight = rect.rect.height;
            float viewportHeight = viewport.rect.height;

            content.localPosition = new Vector3(content.localPosition.x,
                                        Mathf.Clamp(content.localPosition.y, -viewportHeight - itemPos + itemHeight / 2, -itemPos - itemHeight / 2));
        }
    }

}