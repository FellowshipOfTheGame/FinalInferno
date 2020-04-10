using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Scrollbar)), RequireComponent(typeof(RectTransform))]
public class KeyboardScrollbar : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollbar;
    public float Value { get => scrollbar.value; }
    private bool active;
    public bool Active{
        get{
            return active;
        }
        set{
            if(!value){
                scrollbar.value = 1;
            }
            scrollbar.interactable = value;
            active = value;
        }
    }
    [SerializeField] private float speed = 0.1f;

    void Reset(){
        scrollbar = GetComponent<Scrollbar>();
    }

    public float SetValue(float newValue){
        float previousValue = scrollbar.value;

        scrollbar.value = Mathf.Clamp(newValue, 0f, 1f);

        return previousValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        Active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(active){
            float axis = Input.GetAxisRaw("Vertical");
            scrollbar.value = Mathf.Clamp(scrollbar.value + speed * axis * scrollbar.size * Time.deltaTime, 0f, 1f);
        }
    }
}
