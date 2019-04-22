using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectElement : MonoBehaviour
{
    public SkillEffect effect;
    public Image effectImage;
    public Text effectCost;

    public void UpdateEffect(SkillEffect newEffect)
    {
        effect = newEffect;
        effectCost.text = effect.value + "%";
    }

}
