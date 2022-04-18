using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Eunuch Satyr", menuName = "ScriptableObject/Enemy/EunuchSatyr")]
    public class EunuchSatyr : Enemy {
        private Skill SpeedDrainSkill => skills[0];
        private static bool IsDrainingSpeed(BattleUnit thisUnit) {
            if (thisUnit == null || !(thisUnit.Unit is EunuchSatyr))
                return false;
            foreach (StatusEffect effect in thisUnit.effects) {
                if (effect is DrainingSpeed && effect.Source == thisUnit)
                    return true;
            }
            return false;
        }

        public override Skill AttackDecision() {
            BattleUnit thisUnit = BattleManager.instance.currentUnit;
            return (IsDrainingSpeed(thisUnit)) ? attackSkill : SpeedDrainSkill;
        }
    }
}
