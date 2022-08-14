using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.SkillsMenu {
    public class LoadSkillDescription : MonoBehaviour {
        private const string greenColorString = "<color=#006400>";
        private const string redColorString = "<color=#840000>";
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

        private void Awake() {
            currentHero = Party.Instance.characters[0].archetype;
        }

        public void LoadSkillInfo(PlayerSkill skill, int heroIndex) {
            ShowBasicSkillInfo(skill, heroIndex);
            if (skill.Level > 0) {
                ShowSkillEffectInfo(skill);
            } else {
                ShowSkillUnlockInfo(skill);
            }
            UpdateDetailManager(skill);
        }

        private void ShowBasicSkillInfo(PlayerSkill skill, int heroIndex) {
            skillNameText.text = skill.name;
            skillImage.sprite = skill.skillImage;
            skillElementImage.sprite = Icons.instance.elementSprites[(int)skill.attribute - 1];
            skillElementText.text = skill.attribute.ToString();
            skillTargetTypeImage.sprite = Icons.instance.targetTypeSprites[(int)skill.target];
            skillTargetTypeText.text = skill.target.ToString();
            ShowSkillSpeedInfo(skill, heroIndex);
            skillTypeText.text = skill.TypeString;
            skillDescriptionText.text = skill.description;
        }

        private void ShowSkillSpeedInfo(PlayerSkill skill, int heroIndex) {
            heroIndex = Mathf.Clamp(heroIndex, 0, Party.Instance.characters.Count - 1);
            currentHero = Party.Instance.characters[heroIndex].archetype;
            float attackCost = currentHero ? currentHero.attackSkill.cost : 10f;
            skillSpeedText.text = (skill.cost / attackCost).ToString();
            float timeKey = (Mathf.Clamp(skill.cost / attackCost, 0.8f, 2f) - 0.8f) / 1.2f;
            skillSpeedText.color = gradient.Evaluate(timeKey);
        }

        private void ShowSkillEffectInfo(PlayerSkill skill) {
            skillLevelInfoText.text = GetLevelInfoString(skill);
            listDescriptionText.text = "\nEffects:";
            effectList.gameObject.SetActive(true);
            requirementList.gameObject.SetActive(false);
            SkillEffectItem[] children = effectList.GetComponentsInChildren<SkillEffectItem>();
            for (int effectIndex = 0; effectIndex < skill.effects.Count || effectIndex < children.Length; effectIndex++) {
                ConfigureEffectItemAndUpdateChildrenList(skill, children, effectIndex);
            }
        }

        private static string GetLevelInfoString(PlayerSkill skill) {
            StringBuilder levelInfo = new StringBuilder($"Current Level: {skill.Level}\n");
            if (skill.Level < skill.MaxLevel)
                levelInfo.Append($"Exp to next level: {skill.xpNext - skill.xp}");
            else
                levelInfo.Append("Max level reached!");
            return levelInfo.ToString();
        }

        private void ConfigureEffectItemAndUpdateChildrenList(PlayerSkill skill, SkillEffectItem[] children, int effectIndex) {
            if (effectIndex >= skill.effects.Count) {
                Destroy(children[effectIndex].gameObject);
                return;
            }
            SkillEffectItem child;
            if (effectIndex >= children.Length) {
                child = Instantiate(effectPrefab, effectList.GetComponent<RectTransform>());
            } else {
                child = children[effectIndex];
            }
            skill.effects[effectIndex].effect.value1 = skill.effects[effectIndex].value1;
            skill.effects[effectIndex].effect.value2 = skill.effects[effectIndex].value2;
            child.ShowEffect(skill.effects[effectIndex].effect);
        }

        private void ShowSkillUnlockInfo(PlayerSkill skill) {
            skillLevelInfoText.text = GetUnlockInfoString(skill);
            listDescriptionText.text = $"\nSkill Requirements:{((skill.prerequisiteSkills.Count > 0) ? "" : " None")}";
            effectList.gameObject.SetActive(false);
            requirementList.gameObject.SetActive(true);

            SkillRequirementItem[] children = requirementList.GetComponentsInChildren<SkillRequirementItem>();
            for (int prerequisiteIndex = 0; prerequisiteIndex < skill.prerequisiteSkills.Count || prerequisiteIndex < children.Length; prerequisiteIndex++) {
                ConfigurePrerequisiteItemAndUpdateChildrenList(skill, children, prerequisiteIndex);
            }
        }

        private static string GetUnlockInfoString(PlayerSkill skill) {
            StringBuilder unlockInfo = new StringBuilder("Skill unlocks at party level ");
            unlockInfo.Append((Party.Instance.Level >= skill.prerequisiteHeroLevel) ? greenColorString : redColorString);
            unlockInfo.Append($"{skill.prerequisiteHeroLevel}</color>");
            return unlockInfo.ToString();
        }

        private void ConfigurePrerequisiteItemAndUpdateChildrenList(PlayerSkill skill, SkillRequirementItem[] children, int prerequisiteIndex) {
            if (prerequisiteIndex >= skill.prerequisiteSkills.Count) {
                Destroy(children[prerequisiteIndex].gameObject);
                return;
            }
            SkillRequirementItem child;
            if (prerequisiteIndex >= children.Length) {
                child = Instantiate(requirementPrefab, requirementList.GetComponent<RectTransform>());
            } else {
                child = children[prerequisiteIndex];
            }
            child.ShowRequirement(skill.prerequisiteSkills[prerequisiteIndex], skill.prerequisiteSkillsLevel[prerequisiteIndex]);
        }

        private void UpdateDetailManager(PlayerSkill skill) {
            detailManager.SetInteractable(skill.Level > 0 && skill.Type != SkillType.Active);
            if (detailManager.Interactable) {
                detailManager.ShowToggle();
            } else {
                detailManager.HideToggle();
            }
            detailManager.CurrentSkill = skill;
        }
    }
}