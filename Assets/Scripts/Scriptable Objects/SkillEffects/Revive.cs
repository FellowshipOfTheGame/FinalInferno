﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Revive", menuName = "ScriptableObject/SkillEffect/Revive")]
    public class Revive : SkillEffect {
        public override string Description0 { get { return ""; } }
        // value1 = 1 for callback skills, any other value assumes active skill
        public override string Description1{ get {return "\bRevive unit with 1 HP";} }
        public override string Description2{ get {return null;} }
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.Revive(value1 == 1f);
        }
    }
}
