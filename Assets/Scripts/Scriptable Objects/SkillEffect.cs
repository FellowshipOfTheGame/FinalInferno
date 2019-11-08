using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno{
    //engloba cada efeitos que uma "skill" pode causar]
    public abstract class SkillEffect : ScriptableObject{

        [SerializeField] private Sprite icon;
        public Sprite Icon { get{ return icon; } }
        public abstract string Description { get; }
        public float value1, value2;

        public abstract void Apply(BattleUnit source, BattleUnit target);
    }
}