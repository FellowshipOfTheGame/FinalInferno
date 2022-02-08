using UnityEngine;

namespace FinalInferno.EventSystem {
    public class EventRaiserFI : MonoBehaviour {
        public void RaiseEvent(EventFI eventRaised) {
            if (eventRaised != null) {
                eventRaised.Raise();
            }
        }
    }
}