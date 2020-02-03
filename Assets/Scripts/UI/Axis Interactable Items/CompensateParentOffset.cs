using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.AII{
    [RequireComponent(typeof(RectTransform))]
    public class CompensateParentOffset : MonoBehaviour
    {
        public RectTransform followTarget = null;
        private RectTransform rectTransform;
        private Vector2 currentOffset = Vector2.zero;

        void Awake(){
            rectTransform = GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if(followTarget){
                RectTransform followParent = followTarget.parent as RectTransform;
                RectTransform myParent = transform.parent as RectTransform;

                Vector2 offset = followParent.anchoredPosition - myParent.anchoredPosition;
                Vector2 rotationOffset = Vector2.zero;
                offset *= Mathf.Cos(Mathf.Deg2Rad * followParent.parent.localEulerAngles.z);
                rotationOffset.x = Mathf.Sin(Mathf.Deg2Rad * followParent.parent.localEulerAngles.z) * offset.y;
                rotationOffset.y = Mathf.Sin(Mathf.Deg2Rad * followParent.parent.localEulerAngles.z) * offset.x;
                offset -= rotationOffset;
                rectTransform.anchoredPosition -= currentOffset;
                rectTransform.anchoredPosition += offset;
                currentOffset = offset;
            }
        }
    }
}
