using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace FinalInferno.UI.AII {
    public class AIIManager : MonoBehaviour {
        [Header("References")]
        public AxisInteractableItem currentItem;
        public AxisInteractableItem firstItem;
        public AxisInteractableItem lastItem;
        [Header("Input Actions")]
        [SerializeField] private InputActionReference movementAction;
        [SerializeField] private InputActionReference activationAction;
        protected bool active;
        public bool IsActive => active;
        private bool enableInput = true;
        [Header("Config")]
        [SerializeField] private bool interactable;
        public bool Interactable => interactable;
        [SerializeField] protected AudioSource audioSource;


        public void Awake() {
            currentItem = null;
        }

        public void Start() {
            active = false;
        }

        public void Update() {
            if (!active)
                return;
            Vector2 direction = movementAction.action.ReadValue<Vector2>();
            ProcessDirectionInput(direction);
            if (interactable && activationAction && activationAction.action.triggered)
                currentItem.Act();
        }

        private void ProcessDirectionInput(Vector2 direction) {
            if (direction == Vector2.zero) {
                enableInput = true;
                return;
            }
            if (!enableInput || currentItem == null)
                return;
            AxisInteractableItem newItem = GetItemInDirection(direction);
            if (newItem == null)
                return;
            ChangeItem(newItem);
            enableInput = false;
        }

        private AxisInteractableItem GetItemInDirection(Vector2 direction) {
            if (direction == Vector2.up)
                return currentItem.upItem;
            else if (direction == Vector2.down)
                return currentItem.downItem;
            else if (direction == Vector2.right)
                return currentItem.rightItem;
            else if (direction == Vector2.left)
                return currentItem.leftItem;
            return null;
        }

        public virtual void Activate() {
            active = true;
            currentItem = firstItem;
            if (currentItem != null)
                currentItem.Enter();
        }

        public virtual void Deactivate() {
            if (currentItem != null)
                currentItem.Exit();
            active = false;
        }

        public void SetFocus(bool isActive) {
            active = isActive;
        }

        public void SetInteractable(bool value) {
            interactable = value;
        }

        public void ClearItems() {
            if (currentItem != null)
                currentItem.Exit();
            currentItem = null;
            firstItem = null;
            lastItem = null;
        }

        protected void ChangeItem(AxisInteractableItem nextItem) {
            if (currentItem == null || nextItem == null)
                return;
            currentItem.Exit();
            currentItem = nextItem;
            currentItem.Enter();
            if (audioSource)
                audioSource.Play();
        }
    }

}
