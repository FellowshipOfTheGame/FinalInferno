using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//engloba cada efeitos que uma "skill" pode causar]
public abstract class SkillEffect : ScriptableObject{
    public float value; //valor do efeito aplicado

    public abstract void Apply(BattleUnit source, BattleUnit target);

    /*public void Update(float valueUpdated){
        value = valueUpdated;
    }*/
}
