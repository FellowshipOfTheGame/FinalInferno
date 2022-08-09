using UnityEngine;
using UnityEngine.EventSystems;

namespace FinalInferno.UI.AII {
    public class AxisInteractableItem : MonoBehaviour {
        [SerializeField] private UIBehaviour activeReference = null;
        public UIBehaviour ActiveReference {
            get => activeReference;
            set {
                if (!activeReference)
                    activeReference = value;
            }
        }

        public delegate void ItemAction();

        public event ItemAction OnEnter;

        public event ItemAction OnExit;

        public event ItemAction OnAct;

        public AxisInteractableItem leftItem;

        public AxisInteractableItem rightItem;

        public AxisInteractableItem downItem;

        public AxisInteractableItem upItem;

        public virtual void Awake() {
            OnEnter += EnableReference;
            OnExit += DisableReference;
        }

        public void Enter() {
            OnEnter?.Invoke();
        }

        public void Exit() {
            OnExit?.Invoke();
        }

        public void Act() {
            OnAct?.Invoke();
        }

        public void EnableReference() {
            if (activeReference)
                activeReference.enabled = true;
        }

        public void DisableReference() {
            if (activeReference)
                activeReference.enabled = false;
        }
    }

}
