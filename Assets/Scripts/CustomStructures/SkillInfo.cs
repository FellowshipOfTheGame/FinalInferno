using UnityEngine;

namespace FinalInferno {
    [System.Serializable]
    public struct SkillInfo {
        [SerializeField] public int level;
        [SerializeField] public long xp;
        [SerializeField] public long xpToNextLevel;
        [SerializeField] public long xpCumulative;
        [SerializeField] public bool active;
        public SkillInfo(PlayerSkill skill) {
            if (skill == null) {
                level = 1;
                xp = 0;
                xpCumulative = 0;
                active = false;
                xpToNextLevel = 0;
            } else {
                level = skill.Level;
                xp = skill.xp;
                xpCumulative = skill.XpCumulative;
                active = skill.active;
                xpToNextLevel = skill.xpNext;
            }
        }
        public static bool operator ==(SkillInfo left, SkillInfo right) {
            return left.Equals(right);
        }
        public static bool operator !=(SkillInfo left, SkillInfo right) {
            return !(left == right);
        }
        public override bool Equals(object obj) {
            if (obj.GetType() != typeof(SkillInfo)) {
                return false;
            }

            return Equals((SkillInfo)obj);
        }
        public bool Equals(SkillInfo other) {
            if (level != other.level || xp != other.xp || xpToNextLevel != other.xpToNextLevel || xpCumulative != other.xpCumulative || active != other.active) {
                return false;
            }

            return true;
        }
        public override int GetHashCode() {
            return (3 * level.GetHashCode() + 5 * xp.GetHashCode() + 7 * xpToNextLevel.GetHashCode() + 11 * xpCumulative.GetHashCode() + 13 * active.GetHashCode());
        }
    }

}