using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Victory {
    public class SkillInfoLoader : MonoBehaviour {
        [SerializeField] private Image skillImage;
        [SerializeField] private Image skillElementImage;
        [SerializeField] private Image skillTargetTypeImage;
        [SerializeField] private Text skillDescription;
        [SerializeField] private Sprite defaultSprite;

        public void LoadSkillInfo(PlayerSkill skill) {
            skillImage.sprite = skill.skillImage != null ? skill.skillImage : defaultSprite;
            Sprite elementSprite = Icons.instance.elementSprites[(int)skill.attribute - 1];
            skillElementImage.sprite = elementSprite != null ? elementSprite : defaultSprite;
            Sprite targetTypeSprite = Icons.instance.targetTypeSprites[(int)skill.target];
            skillTargetTypeImage.sprite = targetTypeSprite != null ? targetTypeSprite : defaultSprite;
            skillDescription.text = skill.ShortDescription;
        }
    }
}