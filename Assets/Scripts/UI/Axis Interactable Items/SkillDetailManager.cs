using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.AII{
    [RequireComponent(typeof(ScrollRect))]
    public class SkillDetailManager  : AIIManager
    {
        [SerializeField] private List<SkillsMenu.SkillDetailPage> detailPages = new List<SkillsMenu.SkillDetailPage>();
        [SerializeField] private ToggleItem toggle = null;
        private List<float> currentValues = null;
        private PlayerSkill currentSkill = null;
        public PlayerSkill CurrentSkill{
            set{
                currentSkill = value;
                toggle.Toggle(currentSkill.active);
            }
        }

        private int currentIndex = 0;
        [SerializeField] private ScrollRect scrollRect = null;
        [SerializeField] private KeyboardScrollbar scrollbar = null;

        void Reset(){
            scrollRect = GetComponent<ScrollRect>();
        }

        public new void Awake(){
            currentItem = null;

            if(!scrollRect)
                scrollRect = GetComponent<ScrollRect>();

            currentValues = new List<float>();
            for(int i = 0; i < detailPages.Count; i++){
                detailPages[i].Index = i;
                detailPages[i].FocusPage = FocusOn;

                if(!toggle && detailPages[i].GetType() == typeof(ToggleItem)){
                    toggle = detailPages[i].AII as ToggleItem;
                }
                currentValues.Add(1f);
            }
            toggle.OnToggle += ToggleSkillActive;
        }

        private void ToggleSkillActive(){
            currentSkill.active = !currentSkill.active;
            if(AS) AS.Play();
        }

        public void FocusOn(int index){
            index = Mathf.Clamp(index, 0, detailPages.Count-1);
            if(index != currentIndex){

                scrollRect.content = detailPages[index].GetComponent<RectTransform>();
                foreach(SkillsMenu.SkillDetailPage page in detailPages){
                    RectTransform rect = page.GetComponent<RectTransform>();
                    if(rect){
                        rect.pivot += new Vector2(currentIndex - index, 0);
                        rect.anchorMin += new Vector2(currentIndex - index, 0);
                        rect.anchorMax += new Vector2(currentIndex - index, 0);
                        rect.anchoredPosition = Vector2.zero;
                    }
                }
                currentValues[currentIndex] = scrollbar.SetValue(currentValues[index]);

                currentIndex = index;
            }
        }

        public void HideToggle(){
            toggle.Hide();
        }

        public void ShowToggle(){
            toggle.Show();
        }

        public override void Active(){
            currentIndex = 0;
            for(int i = 0; i < currentValues.Count; i++){
                currentValues[i] = 1f;
            }

            base.Active();
        }

        public override void Deactive(){
            FocusOn(0);
            currentIndex = 0;
            for(int i = 0; i < currentValues.Count; i++){
                currentValues[i] = 1f;
            }
            scrollbar.SetValue(1f);

            base.Deactive();
        }
    }
}