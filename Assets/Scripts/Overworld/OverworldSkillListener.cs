using UnityEngine;
using UnityEngine.Events;

namespace FinalInferno {
    public class OverworldSkillListener : MonoBehaviour, IOverworldSkillListener {
        [SerializeField] private OverworldSkill skill;
        [SerializeField] private UnityEvent OnActivate = new UnityEvent();
        [SerializeField] private UnityEvent OnDeactivate = new UnityEvent();

        public void OnEnable() {
            skill?.AddActivationListener(this);
        }

        public void OnDisable() {
            skill?.RemoveActivationListener(this);
        }

        public void ActivatedSkill(OverworldSkill eventSkill) {
            if (eventSkill == skill) {
                OnActivate?.Invoke();
            }
        }

        public void DeactivatedSkill(OverworldSkill eventSkill) {
            if (eventSkill == skill) {
                OnDeactivate?.Invoke();
            }
        }
    }
}
