using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    //engloba cada efeitos que uma "skill" pode causar]
    public abstract class SkillEffect : ScriptableObject{

        public float value1, value2;
        public abstract string Description1 { get; }
        public abstract string Description2 { get; }

        public abstract void Apply(BattleUnit source, BattleUnit target);

        /*public void Update(float valueUpdated){
            value = valueUpdated;
        }*/
    }
}