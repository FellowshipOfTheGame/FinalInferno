using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.SkillsMenu
{
    public class SkillsContent : MonoBehaviour
    {
        [SerializeField] private RectTransform content;

        private float xPosition = 0f;
        private int curIndex = 0;
        private const float skillListWidth = 360f;
        private const float skillListsSpacing = 0f;
        private bool isInPosition = false;

        void Awake(){
            isInPosition = false;
            curIndex = 0;
        }

        void Update()
        {
            if(!isInPosition && Mathf.Abs(content.localPosition.x - xPosition) >=  (skillListWidth * 0.01f)){
                isInPosition = false;
                content.localPosition = new Vector3(Mathf.Lerp(content.localPosition.x, xPosition, .25f), content.localPosition.y);
            }else{
                SetContentToPosition(curIndex, true);
            }
        }

        public void SetContentToPosition(int index, bool skipAnimation = false)
        {
            curIndex = index;
            if(!skipAnimation){
                isInPosition = false;
                xPosition = -index * skillListWidth -(index * skillListsSpacing);
            }else if(!isInPosition){
                isInPosition = true;
                content.localPosition = new Vector3(xPosition, content.localPosition.y);
            }
        }
    }

}