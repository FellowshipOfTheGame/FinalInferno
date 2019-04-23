using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;

[CreateAssetMenu(fileName = "Damage", menuName = "ScriptableObject/SkillEffect/Damage")]
public class Damage : SkillEffect {
    public override void Apply(BattleUnit source, BattleUnit target) {
        target.TakeDamage(source.curDmg, value, DamageType.Physical, Element.Neutral);
    }
}
