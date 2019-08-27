﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.SkillsMenu
{
    public class SkillsContent : MonoBehaviour
    {
        [SerializeField] private RectTransform content;

        private float xPosition = 0f;

        void Update()
        {
            content.localPosition = new Vector3(Mathf.Lerp(content.localPosition.x, xPosition, .15f), content.localPosition.y);
        }

        public void SetContentToPosition(int index)
        {
            xPosition = -index * 413.95f;
        }
    }

}