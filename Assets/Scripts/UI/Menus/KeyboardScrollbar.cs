using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Scrollbar)), RequireComponent(typeof(RectTransform))]
public class KeyboardScrollbar : MonoBehaviour {
    [SerializeField] private Scrollbar scrollbar;
    public float Value => scrollbar.value;
    private bool active;
    public bool Active {
        get => active;
        set {
            if (!value) {
                scrollbar.value = 1;
            }
            scrollbar.interactable = value;
            active = value;
        }
    }
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private InputActionReference movementAction;

    private void Reset() {
        scrollbar = GetComponent<Scrollbar>();
    }

    public float SetValue(float newValue) {
        float previousValue = scrollbar.value;

        scrollbar.value = Mathf.Clamp(newValue, 0f, 1f);

        return previousValue;
    }

    // Start is called before the first frame update
    private void Start() {
        Active = false;
    }

    // Update is called once per frame
    private void Update() {
        if (active) {
            // float axis = Input.GetAxisRaw("Vertical");
            float axis = movementAction.action.ReadValue<Vector2>().y;
            scrollbar.value = Mathf.Clamp(scrollbar.value + speed * axis * scrollbar.size * Time.deltaTime, 0f, 1f);
        }
    }
}
