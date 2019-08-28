using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "ShareDebuff", menuName = "ScriptableObject/SkillEffect/ShareDebuff")]
    public class ShareDebuff : SkillEffect {
        public override string Description0 { get { return "Share "; } }
        // value1 = index of status effect being shared
        public override string Description1{ get {return "\bEffect number " + value1;} }
        // value1 = modifier to apply to the effect
        public override string Description2{ get {return "x";} }
        public override void Apply(BattleUnit source, BattleUnit target) {
            List<BattleUnit> allies = BattleManager.instance.GetTeam(target);
            foreach(BattleUnit unit in allies){
                if(unit != target)
                    target.effects[Mathf.Clamp(((int)value1), 0, target.effects.Count - 1)].CopyTo(unit, value2);
            }
        }
    }
}
