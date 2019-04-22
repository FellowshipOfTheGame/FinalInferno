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

    public Text skillNameText;
    public Text damageText;
    public Text costText;

    public ButtonClickDecision BCD;

    public void UpdateContent(List<Skill> skills)
    {
        foreach (SkillElement SE in content.GetComponentsInChildren<SkillElement>())
        {
            Destroy(SE.gameObject);
        }

        SkillItem lastSkillItem = null;
        foreach (PlayerSkill skill in skills)
        {
            if (skill.active)
            {
                GameObject newSkill = Instantiate(skillObject);
                newSkill.GetComponent<SkillElement>().skill = skill;
                newSkill.transform.SetParent(content);

                SkillItem newSkillItem = newSkill.GetComponent<SkillItem>();
                newSkillItem.skillList = this;
                newSkillItem.BCD = BCD;
                if (lastSkillItem != null)
                {
                    newSkillItem.positiveItem = lastSkillItem;
                    lastSkillItem.negativeItem = newSkillItem;
                }
                else
                {
                    manager.currentItem = newSkillItem;                    
                }
                lastSkillItem = newSkillItem;
            }
        }
    }

    public void UpdateSkillDescription(PlayerSkill skill)
    {
        skillNameText.text = skill.name;
        costText.text = skill.cost.ToString();

        UpdateEffectsContent(skill.effects);
    }

    private void UpdateEffectsContent(List<SkillEffect> effects)
    {
        foreach (EffectElement EE in effectsContent.GetComponentsInChildren<EffectElement>())
        {
            Destroy(EE.gameObject);
        }

        foreach (SkillEffect effect in effects)
        {
            GameObject newEffect = Instantiate(effectObject);
            newEffect.GetComponent<EffectElement>().UpdateEffect(effect);
            newEffect.transform.SetParent(effectsContent);
        }
    }

    public void ClampSkillContent(RectTransform currentTrans)
    {
        float itemPos = currentTrans.localPosition.y;

        content.localPosition = new Vector3(content.localPosition.x, Mathf.Clamp(content.localPosition.y, -itemPos-279, -itemPos-52));
    }
}
