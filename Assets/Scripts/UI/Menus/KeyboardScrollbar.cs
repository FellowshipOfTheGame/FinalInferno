using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Scrollbar)), RequireComponent(typeof(RectTransform))]
public class KeyboardScrollbar : MonoBehaviour {
    public ScrollRect scrollRect;
    [SerializeField] private Scrollbar scrollbar;
    private bool processingCodeValueChange = false;
    public float Value {
        get => Mathf.Clamp(scrollbar.value, 0f, 1f);
        private set => scrollbar.value = Mathf.Clamp(value, 0f, 1f);
    }
    private float newValue;
    private bool active;
    public bool Active {
        get => active;
        set {
            if (!value)
                Value = 1;
            scrollbar.interactable = value;
            active = value;
        }
    }
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private InputActionReference movementAction;
    private float inputValue;

    private void Reset() {
        scrollbar = GetComponent<Scrollbar>();
    }

    public void SetValue(float desiredValue) {
        if (!active) {
            Value = desiredValue;
            return;
        }
        newValue = desiredValue;
        processingCodeValueChange = true;
    }

    private void Start() {
        Active = false;
        newValue = scrollbar.value;
    }

    private void Update() {
        if (active)
            inputValue = movementAction.action.ReadValue<Vector2>().y;
    }

    private void LateUpdate() {
        if (scrollbar.size <= 0f)
            RecalculateSize();
        if (processingCodeValueChange) {
            processingCodeValueChange = false;
            Value = newValue;
        } else if (active) {
            Value += speed * inputValue * scrollbar.size * Time.deltaTime;
        }
    }

    private void RecalculateSize() {
        float contentHeight = Mathf.Max(float.Epsilon, scrollRect.content.rect.height);
        float viewportHeight = Mathf.Max(float.Epsilon, scrollRect.viewport.rect.height);
        scrollbar.size = Mathf.Clamp(viewportHeight / contentHeight, 0.1f, 1f);
    }
}
