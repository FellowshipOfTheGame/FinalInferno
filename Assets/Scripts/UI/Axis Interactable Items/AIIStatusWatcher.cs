using System.Collections.Generic;
using FinalInferno.UI.FSM;
using UnityEngine;

namespace FinalInferno.UI.AII {
    public class AIIStatusWatcher : MonoBehaviour {
        [SerializeField] private BoolDecision hasActiveAII;
        [SerializeField] private int counter = 0;

        private List<AIIManager> managers = new List<AIIManager>();
        private int Counter {
            get => counter;
            set {
                counter = value;
                if (hasActiveAII != null) {
                    hasActiveAII.UpdateValue(Counter > 0);
                }
            }
        }

        void Awake() {
            counter = 0;
        }

        public void ActivatedAII(AIIManager manager) {
            if (!managers.Contains(manager)) {
                managers.Add(manager);
                Counter++;
            }
        }

        public void DeactivatedAII(AIIManager manager) {
            if (managers.Contains(manager)) {
                managers.Remove(manager);
                Counter--;
            }
        }
    }
}