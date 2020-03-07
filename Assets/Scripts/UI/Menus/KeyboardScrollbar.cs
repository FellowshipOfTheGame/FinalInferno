using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Scrollbar)), RequireComponent(typeof(RectTransform))]
public class KeyboardScrollbar : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private RectTransform rect;
    private bool active;
    public bool Active{
        get{
            return active;
        }
        set{
            if(value){
                ratio = scrollbar.GetComponent<RectTransform>().rect.height / rect.rect.height;
            }else{
                scrollbar.value = 1;
            }
            scrollbar.interactable = value;
            active = value;
        }
    }
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private RectTransform variableContent;
    private float ratio;

    void Reset(){
        scrollbar = GetComponent<Scrollbar>();
        rect = GetComponent<RectTransform>();
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
            scrollbar.value = Mathf.Clamp(scrollbar.value + speed * axis * ratio * Time.deltaTime, 0, 1);
        }
    }
}
