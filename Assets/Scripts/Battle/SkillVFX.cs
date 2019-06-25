using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(SpriteRenderer))]
public class SkillVFX : MonoBehaviour
{
    public static int nTargets;
    private static int counter = 0;
    void UseSkill(){
        // TO DO: usa a skill selecionada com o currentUnit como source e transform.parent.getComponent<BattleUnit>(como alvo)
        counter++;
        if(counter >= nTargets){
            counter = 0;
            // TO DO: Avisa que acabou as animacoes da skill
        }
    }
    void DestroySkillObject()
    {
        Destroy(this);
    }
}
