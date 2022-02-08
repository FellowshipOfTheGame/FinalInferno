using UnityEngine;

namespace FinalInferno.UI {
    public class TextUpdater : MonoBehaviour {
        [SerializeField] private StringVariable textValue;
        [SerializeField] private UnityEngine.UI.Text text;

        // Start is called before the first frame update
        private void Awake() {
            UpdateText();
        }

        public void UpdateText() {
            if (textValue != null) {
                text.text = textValue.Value;
            }
        }
    }
}