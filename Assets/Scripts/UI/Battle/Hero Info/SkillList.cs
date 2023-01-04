using System.Collections.Generic;
using System.Linq;
using FinalInferno.UI.AII;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Battle.SkillMenu {
    public class SkillList : MonoBehaviour {
        [Header("Prefabs")]
        [SerializeField] private GameObject skillObject;
        [SerializeField] private GameObject effectObject;
        [Header("Contents references")]
        [SerializeField] private RectTransform skillsContent;
        [SerializeField] private RectTransform viewportRect;
        [SerializeField] private RectTransform effectsContent;
        [Header("Managers")]
        [SerializeField] private AIIManager manager;
        [SerializeField] private AIIManager effectsManager;
        [Header("UI elements")]
        [SerializeField] private Text skillNameText;
        [SerializeField] private Text descriptionText;
        [SerializeField] private Text costText;
        [SerializeField] private Text effectDescriptionText;
        [SerializeField] private Image skillIconImage;
        [SerializeField] private Image elementImage;
        [SerializeField] private Image targetTypeImage;

        public void UpdateSkillsContent(List<Skill> skills) {
            ClearExistingSkillElements();
            AxisInteractableItem lastItem = null;
            foreach (PlayerSkill skill in skills.Cast<PlayerSkill>()) {
                if (!skill.active)
                    continue;
                GameObject newSkill = Instantiate(skillObject, skillsContent);
                SkillElement newSkillElement = newSkill.GetComponent<SkillElement>();
                newSkillElement.Configure(this, skill);
                lastItem = AddNewSkillElementAsLastItem(lastItem, newSkillElement);
            }
        }

        private void ClearExistingSkillElements() {
            foreach (SkillElement element in skillsContent.GetComponentsInChildren<SkillElement>()) {
                Destroy(element.gameObject);
            }
        }

        private AxisInteractableItem AddNewSkillElementAsLastItem(AxisInteractableItem lastItem, AxisInteractableItem newItem) {
            if (lastItem != null) {
                newItem.upItem = lastItem;
                lastItem.downItem = newItem;
            } else {
                manager.firstItem = newItem;
            }
            lastItem = newItem;
            return lastItem;
        }

        public void ActivateManager() {
            manager.Activate();
        }

        public void UpdateSkillDescription(Skill skill) {
            ShowSkillInfo(skill);
            ShowIconIfPlayerSkill(skill);
            UpdateEffectsContent(skill.effects);
            if (skill is PlayerSkill)
                effectsManager.Activate();
        }

        private void ShowSkillInfo(Skill skill) {
            skillNameText.text = skill.name;
            costText.text = $"{skill.cost}";
            descriptionText.text = skill.ShortDescription;
            elementImage.sprite = Icons.instance.elementSprites[(int)skill.attribute - 1];
            targetTypeImage.sprite = Icons.instance.targetTypeSprites[(int)skill.target];
        }

        private void ShowIconIfPlayerSkill(Skill skill) {
            if (skill is PlayerSkill) {
                skillIconImage.enabled = true;
                skillIconImage.sprite = (skill as PlayerSkill).skillImage;
            } else {
                skillIconImage.enabled = false;
            }
        }

        private void UpdateEffectsContent(List<SkillEffectTuple> effects) {
            ClearExistingEffectElements();
            AxisInteractableItem lastItem = null;
            foreach (SkillEffectTuple effect in effects) {
                GameObject newGameObject = Instantiate(effectObject, effectsContent);
                EffectElement newEffect = newGameObject.GetComponent<EffectElement>();
                newEffect.Configure(effect, this);
                lastItem = AddNewEffectElementAsLastItem(lastItem, newEffect);
            }
            UpdateAllEffectDescriptions(effects);
        }

        private AxisInteractableItem AddNewEffectElementAsLastItem(AxisInteractableItem lastItem, EffectElement newEffect) {
            AxisInteractableItem newItem = newEffect;
            if (lastItem != null) {
                newItem.leftItem = lastItem;
                lastItem.rightItem = newItem;
            } else {
                effectsManager.firstItem = newItem;
            }
            lastItem = newItem;
            effectsManager.lastItem = newItem;
            return lastItem;
        }

        private void ClearExistingEffectElements() {
            foreach (EffectElement element in effectsContent.GetComponentsInChildren<EffectElement>()) {
                Destroy(element.gameObject);
            }
        }

        private void UpdateAllEffectDescriptions(List<SkillEffectTuple> effects) {
            if (effects.Count <= 0)
                return;
            effects[0].UpdateValues();
            UpdateEffectDescription(effects[0].effect);
        }

        public void UpdateEffectDescription(SkillEffect effect) {
            effectDescriptionText.text = (effect.DisplayName != null && effect.DisplayName != "") ? effect.DisplayName : effect.name;
        }

        public Skill GetFirstSkill() {
            if (manager == null || manager.firstItem == null)
                return null;
            return (manager.firstItem as SkillElement).Skill;
        }

        public void ClampSkillContent(RectTransform itemTransform) {
            Vector3 contentPosition = skillsContent.localPosition;
            float itemPos = itemTransform.localPosition.y;
            float itemHeight = itemTransform.rect.height;
            float viewportHeight = viewportRect.rect.height;
            float minValueY = -viewportHeight - itemPos + itemHeight / 2;
            float maxValueY = -itemPos - itemHeight / 2;
            skillsContent.localPosition = new Vector3(contentPosition.x, Mathf.Clamp(contentPosition.y, minValueY, maxValueY));
        }
    }
}