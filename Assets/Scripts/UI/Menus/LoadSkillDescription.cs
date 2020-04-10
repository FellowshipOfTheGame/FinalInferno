using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.SkillsMenu
{
    public class LoadSkillDescription : MonoBehaviour
    {
        [SerializeField] private Text skillNameText;
        [SerializeField] private Image skillImage;

        [SerializeField] private Image skillElementImage;
        [SerializeField] private Text skillElementText;

        [SerializeField] private Image skillTargetTypeImage;
        [SerializeField] private Text skillTargetTypeText;

        [SerializeField] private Text skillTypeText;
        [SerializeField] private Text skillDescriptionText;
        [SerializeField] private Text skillLevelInfoText;

        [SerializeField] private AII.SkillDetailManager detailManager;

        [SerializeField] private VerticalLayoutGroup effectList;
        [SerializeField] private SkillEffectItem prefab;

        public void LoadSkillInfo(PlayerSkill skill)
        {
            skillNameText.text = skill.name;
            skillImage.sprite = skill.skillImage;

            skillElementImage.sprite = Icons.instance.elementSprites[(int)skill.attribute - 1];
            skillElementText.text = skill.attribute.ToString();

            skillTargetTypeImage.sprite = Icons.instance.targetTypeSprites[(int)skill.target];
            skillTargetTypeText.text = skill.target.ToString();

            skillTypeText.text = skill.TypeString;
            skillDescriptionText.text = skill.description;
            string levelInfo = "Current Level: " + skill.Level + "\n";
            levelInfo += "Exp to next level: " + (skill.xpNext - skill.xp);
            skillLevelInfoText.text = levelInfo;

            SkillEffectItem[] children = effectList.GetComponentsInChildren<SkillEffectItem>();
            for(int i = 0; i < skill.effects.Count || i < children.Length; i++){
                if(i >= skill.effects.Count){
                    Destroy(children[i].gameObject);
                }else{
                    SkillEffectItem child = null;
                    if(i >= children.Length){
                        child = Instantiate(prefab, effectList.GetComponent<RectTransform>());
                    }else{
                        child = children[i];
                    }
                    skill.effects[i].effect.value1 = skill.effects[i].value1;
                    skill.effects[i].effect.value2 = skill.effects[i].value2;
                    child.ShowEffect(skill.effects[i].effect);
                }
            }

            detailManager.interactable = skill.Type != SkillType.Active;
            if(detailManager.interactable){
                detailManager.ShowToggle();
            }else{
                detailManager.HideToggle();
            }
            detailManager.CurrentSkill = skill;
        }
    }
}