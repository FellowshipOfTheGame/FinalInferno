using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Battle.SkillMenu {
    public class EffectElement : MonoBehaviour {
        public SkillEffectTuple EffectTuple { get; private set; }
        [Header("UI elements")]
        [SerializeField] private Image effectImage;

        public void SetEffect(SkillEffectTuple newEffect) {
            EffectTuple = newEffect;
            effectImage.sprite = EffectTuple.effect.Icon;
        }

    }

}