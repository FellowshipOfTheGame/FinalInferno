using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FinalInferno.UI.AII;
using FinalInferno.UI.FSM;

public class SkillList : MonoBehaviour
{
    public GameObject skillObject;
    public GameObject effectObject;
    public RectTransform content;
    public RectTransform effectsContent;
    [SerializeField] private AIIManager manager;
    [SerializeField] private AIIManager effectsManager;

    public Text skillNameText;
    public Text descriptionText;
    public Text costText;

    public ButtonClickDecision BCD;

    public void UpdateContent(List<Skill> skills)
    {
        foreach (SkillElement SE in content.GetComponentsInChildren<SkillElement>())
        {
            Destroy(SE.gameObject);
        }

        AxisInteractableItem lastItem = null;
        foreach (PlayerSkill skill in skills)
        {
            if (skill.active)
            {
                GameObject newSkill = Instantiate(skillObject);
                newSkill.GetComponent<SkillElement>().skill = skill;
                newSkill.transform.SetParent(content);

                SkillItem newSkillItem = newSkill.GetComponent<SkillItem>();
                newSkillItem.skillList = this;

                ClickableItem newClickableItem = newSkill.GetComponent<ClickableItem>();
                newClickableItem.BCD = BCD;

                AxisInteractableItem newItem = newSkill.GetComponent<AxisInteractableItem>();
                if (lastItem != null)
                {
                    newItem.positiveItem = lastItem;
                    lastItem.negativeItem = newItem;
                }
                else
                {
                    manager.currentItem = newItem;                    
                }
                lastItem = newItem;
            }
        }
    }

    public void UpdateSkillDescription(PlayerSkill skill)
    {
        skillNameText.text = skill.name;
        costText.text = skill.cost.ToString();
        descriptionText.text = skill.description;

        UpdateEffectsContent(skill.effects);
        effectsManager.Active();
    }

    public void ClampSkillContent(RectTransform currentTrans)
    {
        float itemPos = currentTrans.localPosition.y;

        content.localPosition = new Vector3(content.localPosition.x, 
                                    Mathf.Clamp(content.localPosition.y, -itemPos-279, -itemPos-52));
    }

    private void UpdateEffectsContent(List<SkillEffect> effects)
    {
        foreach (EffectElement EE in effectsContent.GetComponentsInChildren<EffectElement>())
        {
            Destroy(EE.gameObject);
        }

        AxisInteractableItem lastItem = null;
        foreach (SkillEffect effect in effects)
        {
            GameObject newEffect = Instantiate(effectObject);
            newEffect.GetComponent<EffectElement>().UpdateEffect(effect);
            newEffect.transform.SetParent(effectsContent);

            EffectItem newEffectItem = newEffect.GetComponent<EffectItem>();
            newEffectItem.skillList = this;

            AxisInteractableItem newItem = newEffect.GetComponent<AxisInteractableItem>();
            if (lastItem != null)
            {
                newItem.negativeItem = lastItem;
                lastItem.positiveItem = newItem;
            }
            else
            {
                effectsManager.currentItem = newItem;
            }
            lastItem = newItem;
        }
    }

    public void UpdateEffectDescription(SkillEffect effect)
    {
    }

}