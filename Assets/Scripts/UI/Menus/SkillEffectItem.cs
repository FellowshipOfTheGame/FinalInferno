using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.SkillsMenu{
    public class SkillEffectItem : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Text description;

        public void ShowEffect(SkillEffect effect){
            image.sprite = effect.Icon;
            description.text = effect.Description;
        }
    }
}