using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fog.Dialogue
{
    [RequireComponent(typeof(AudioSource))]
    public class OptionHandler : MonoBehaviour
    {
        [SerializeField] private DialogueScrollPanel scrollPanel = null;
        [SerializeField] private RectTransform container = null;
        [SerializeField] private GameObject optionPrefab = null;
        [SerializeField] private float inputCooldown = 0.5f;
        private float timer;
        private AudioSource source;
        [SerializeField] private AudioClip changeOption;
        [SerializeField] private AudioClip selectOption;

        private int currentOption = -1;
        private List<DialogueOption> options = new List<DialogueOption>();

        public bool IsActive { get; private set; }

        void Awake(){
            source = GetComponent<AudioSource>();
            IsActive = false;
            inputCooldown = Mathf.Max(0f, inputCooldown);
            if(!optionPrefab){
                Debug.Log("No prefab detected");
                Destroy(this);
            }else{
                if(!optionPrefab.GetComponent<DialogueOption>()){
                    Debug.Log("Prefab must have a DialogueOption component");
                    Destroy(this);
                }
            }
        }

        public void CreateOptions(DialogueOptionInfo[] infos){
            if(infos.Length > 0){
                container.gameObject.SetActive(true);
                foreach(DialogueOptionInfo info in infos){
                    GameObject go = Instantiate(optionPrefab, container);
                    DialogueOption newOption = go.GetComponentInChildren<DialogueOption>();
                    newOption.Configure(info);
                    newOption.OnSelect += SelectOption;
                    newOption.OnFocus += FocusOption;
                }
                currentOption = 0;
                options[0].OnFocus?.Invoke();
                timer = 0f;
                IsActive = true;
            }
        }

        private void FocusOption(){
            float normalizedTop = scrollPanel.NormalizedTopPosition(options[currentOption].GetComponent<RectTransform>());
            float normalizedBottom = scrollPanel.NormalizedBottomPosition(options[currentOption].GetComponent<RectTransform>());

            if(scrollPanel.verticalNormalizedPosition > normalizedTop 
            || scrollPanel.viewport.rect.height <= options[currentOption].GetComponent<RectTransform>().rect.height){
                scrollPanel.ScrollToPosition(normalizedTop);
            }else if (scrollPanel.verticalNormalizedPosition < normalizedBottom){
                scrollPanel.ScrollToPosition(normalizedBottom);
            }
        }

        private void SelectOption(){
            IsActive = false;
            timer = 0f;
            source.PlayOneShot(selectOption);
            Dialogue selectedDialogue = options[currentOption].NextDialogue;
            foreach(RectTransform transform in container){
                Destroy(transform.gameObject);
            }
            container.gameObject.SetActive(false);
            DialogueHandler.instance.StartDialogue(selectedDialogue);
        }

        // Update is called once per frame
        void Update()
        {
            if(IsActive){
                if(timer > inputCooldown){
                    if(Input.GetButton("Submit")){
                        options[currentOption].OnSelect?.Invoke();
                    }else{
                        float input = Input.GetAxisRaw("Vertical");
                        if(input != 0){
                            int newOption = Mathf.Clamp(currentOption + ((input > 0)? 1 : -1), 0, options.Count-1);
                            if(newOption != currentOption){
                                options[currentOption].OnExit?.Invoke();
                                currentOption = newOption;
                                options[currentOption].OnFocus?.Invoke();
                            }
                            // If you press up on the first option it tries to scroll further up
                            // For cases when the question and answers are using the same scrollpanel
                            if(input > 0 && currentOption == 0){
                                scrollPanel.ScrollToStart();
                            }
                        }
                    }
                    timer = 0f;
                }
                timer += Time.deltaTime;
            }
        }
    }
}