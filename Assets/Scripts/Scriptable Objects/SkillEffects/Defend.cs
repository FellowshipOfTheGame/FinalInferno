using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;

[CreateAssetMenu(fileName = "Defend", menuName = "ScriptableObject/SkillEffect/Defend")]
public class Defend : SkillEffect {
    public override void Apply(BattleUnit source, BattleUnit target) {
        target.effects.Add(new Defending(target, value));
    }
}
