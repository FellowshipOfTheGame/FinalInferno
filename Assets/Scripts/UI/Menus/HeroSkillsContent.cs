using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.SkillsMenu
{
    public class HeroSkillsContent : MonoBehaviour
    {
        [SerializeField] private RectTransform content;

        public void ClampContent(RectTransform rect)
        {
            float itemPos = rect.localPosition.y;

            content.localPosition = new Vector3(content.localPosition.x,
                                        Mathf.Clamp(content.localPosition.y, -itemPos - 139, -itemPos + 189));
        }
    }

}