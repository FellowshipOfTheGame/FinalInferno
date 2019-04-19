using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//engloba cada efeitos que uma "skill" pode causar
[CreateAssetMenu(fileName = "SkillEffect", menuName = "ScriptableObject/SkillEffect", order = 6)]
public class SkillEffect : ScriptableObject{
    //public Effect effect; //efeito que a "skill" causara
    public int value; //valor do efeito aplicado

}
