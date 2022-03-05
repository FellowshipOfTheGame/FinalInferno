using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    [RequireComponent(typeof(Collider2D)), RequireComponent(typeof(Animator))]
    public class Changeable : MonoBehaviour {
        [SerializeField, HideInInspector] private Animator anim = null;
        [SerializeField] private List<ChangeRule> changeRules = new List<ChangeRule>();
        private List<Collider2D> colliders = new List<Collider2D>();
        private List<Collider2D> triggers = new List<Collider2D>();

        public void Awake() {
            SaveAnimatorReference();
            SaveColliderReferences();
        }

        private void SaveAnimatorReference() {
            if (anim == null) {
                anim = GetComponent<Animator>();
            }
        }

        private void SaveColliderReferences() {
            foreach (Collider2D col in GetComponents<Collider2D>()) {
                if (col.isTrigger) {
                    triggers.Add(col);
                } else {
                    colliders.Add(col);
                }
            }
        }

        public void Reset() {
            SaveAnimatorReference();
        }

        public void Update() {
            foreach (ChangeRule rule in changeRules) {
                ApplyChangeIfNecessary(rule);
            }
        }

        private void ApplyChangeIfNecessary(ChangeRule rule) {
            if (rule.IsConditionSatisfied) {
                anim.SetBool(rule.animationFlag, rule.newValue);
            }
        }

        public void DisableColliders() {
            foreach (Collider2D col in colliders) {
                col.enabled = false;
            }
        }

        public void ReenableColliders() {
            foreach (Collider2D col in colliders) {
                col.enabled = true;
            }
        }

        public void DisableInteractions() {
            foreach (Collider2D col in triggers) {
                col.enabled = false;
            }
        }

        public void ReenableInteractions() {
            foreach (Collider2D col in triggers) {
                col.enabled = true;
            }
        }
    }
}