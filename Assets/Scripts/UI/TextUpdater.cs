using UnityEngine;

namespace FinalInferno.UI {
    public class TextUpdater : MonoBehaviour {
        [SerializeField] private StringVariable textValue;
        [SerializeField] private UnityEngine.UI.Text text;

        private void Awake() {
            UpdateText();
        }

        public void UpdateText() {
            if (textValue != null)
                text.text = textValue.Value;
        }
    }
}