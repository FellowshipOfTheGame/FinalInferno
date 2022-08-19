using UnityEngine;
using UnityEngine.UI;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.Battle.SkillMenu {
    public class EffectElement : AxisInteractableItem {
        public SkillEffectTuple EffectTuple { get; private set; }
        [Header("UI elements")]
        [SerializeField] private Image effectImage;
        [SerializeField] private EffectListItem effectListItem;

        public void Configure(SkillEffectTuple newEffect, SkillList skillList) {
            EffectTuple = newEffect;
            effectImage.sprite = EffectTuple.effect.Icon;
            effectListItem.skillList = skillList;
        }
    }
}