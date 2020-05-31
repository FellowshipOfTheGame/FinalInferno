using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.SkillsMenu
{
    public class LoadSkillDescription : MonoBehaviour
    {
        private Hero currentHero;
        [SerializeField] private Text skillNameText;
        [SerializeField] private Image skillImage;

        [SerializeField] private Image skillElementImage;
        [SerializeField] private Text skillElementText;

        [SerializeField] private Image skillTargetTypeImage;
        [SerializeField] private Text skillTargetTypeText;

        [SerializeField] private Text skillSpeedText;
        [SerializeField] private Gradient gradient;

        [SerializeField] private Text skillTypeText;
        [SerializeField] private Text skillDescriptionText;
        [SerializeField] private Text skillLevelInfoText;
        [SerializeField] private Text listDescriptionText;

        [SerializeField] private AII.SkillDetailManager detailManager;

        [SerializeField] private VerticalLayoutGroup effectList;
        [SerializeField] private VerticalLayoutGroup requirementList;
        [SerializeField] private SkillEffectItem effectPrefab;
        [SerializeField] private SkillRequirementItem requirementPrefab;

        void Awake(){
            currentHero = Party.Instance.characters[0].archetype;
        }

        public void LoadSkillInfo(PlayerSkill skill, int heroIndex)
        {
            skillNameText.text = skill.name;
            skillImage.sprite = skill.skillImage;

            skillElementImage.sprite = Icons.instance.elementSprites[(int)skill.attribute - 1];
            skillElementText.text = skill.attribute.ToString();

            skillTargetTypeImage.sprite = Icons.instance.targetTypeSprites[(int)skill.target];
            skillTargetTypeText.text = skill.target.ToString();

            heroIndex = Mathf.Clamp(heroIndex, 0, Party.Instance.characters.Count-1);
            currentHero = Party.Instance.characters[heroIndex].archetype;
            float attackCost = currentHero? currentHero.attackSkill.cost : 10f;
            skillSpeedText.text = (skill.cost / attackCost).ToString();
            float timeKey = ((Mathf.Clamp((skill.cost/attackCost), 0.8f, 2f)) - 0.8f) / 1.2f;
            skillSpeedText.color = gradient.Evaluate(timeKey);

            skillTypeText.text = skill.TypeString;
            skillDescriptionText.text = skill.description;
            if(skill.Level > 0){
                string levelInfo = "Current Level: " + skill.Level + "\n";
                levelInfo += "Exp to next level: " + (skill.xpNext - skill.xp);
                skillLevelInfoText.text = levelInfo;
                listDescriptionText.text = "\nEffects:";
                effectList.gameObject.SetActive(true);
                requirementList.gameObject.SetActive(false);

                SkillEffectItem[] children = effectList.GetComponentsInChildren<SkillEffectItem>();
                for(int i = 0; i < skill.effects.Count || i < children.Length; i++){
                    if(i >= skill.effects.Count){
                        Destroy(children[i].gameObject);
                    }else{
                        SkillEffectItem child = null;
                        if(i >= children.Length){
                            child = Instantiate(effectPrefab, effectList.GetComponent<RectTransform>());
                        }else{
                            child = children[i];
                        }
                        skill.effects[i].effect.value1 = skill.effects[i].value1;
                        skill.effects[i].effect.value2 = skill.effects[i].value2;
                        child.ShowEffect(skill.effects[i].effect);
                    }
                }
            }else{
                string unlockInfo = "Skill unlocks at party level ";
                unlockInfo += (Party.Instance.level >= skill.prerequisiteHeroLevel)? "<color=#006400>" : "<color=#840000>";
                unlockInfo += skill.prerequisiteHeroLevel + "</color>";
                skillLevelInfoText.text = unlockInfo;
                listDescriptionText.text = "\nSkill Requirements:" + ((skill.prerequisiteSkills.Count > 0)? "" : " None");
                effectList.gameObject.SetActive(false);
                requirementList.gameObject.SetActive(true);

                SkillRequirementItem[] children = requirementList.GetComponentsInChildren<SkillRequirementItem>();
                for(int i = 0; i < skill.prerequisiteSkills.Count || i < children.Length; i++){
                    if(i >= skill.prerequisiteSkills.Count){
                        Destroy(children[i].gameObject);
                    }else{
                        SkillRequirementItem child = null;
                        if(i >= children.Length){
                            child = Instantiate(requirementPrefab, requirementList.GetComponent<RectTransform>());
                        }else{
                            child = children[i];
                        }
                        child.ShowRequirement(skill.prerequisiteSkills[i], skill.prerequisiteSkillsLevel[i]);
                    }
                }
            }

            detailManager.interactable = (skill.Level > 0 && skill.Type != SkillType.Active);
            if(detailManager.interactable){
                detailManager.ShowToggle();
            }else{
                detailManager.HideToggle();
            }
            detailManager.CurrentSkill = skill;
        }
    }
}