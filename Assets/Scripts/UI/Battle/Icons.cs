using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI {
    public class Icons : MonoBehaviour {
        public static Icons instance;

        public List<Sprite> damageSprites;
        public List<Sprite> elementSprites;
        public List<Sprite> targetTypeSprites;

        private void Awake() {
            if (instance == null) {
                instance = this;
            } else if (instance != this) {
                Destroy(this);
            }
        }
    }
}