using UnityEngine;

namespace FinalInferno {
    [System.Serializable]
    public struct SkillInfoArray {
        [SerializeField] public SkillInfo[] skills;
        public static bool operator ==(SkillInfoArray left, SkillInfoArray right) {
            return left.Equals(right);
        }
        public static bool operator !=(SkillInfoArray left, SkillInfoArray right) {
            return !(left == right);
        }
        public override int GetHashCode() {
            return skills.GetHashCode();
        }
        public override bool Equals(object obj) {
            if (obj.GetType() != typeof(SkillInfoArray)) {
                return false;
            }

            return Equals((SkillInfoArray)obj);
        }
        public bool Equals(SkillInfoArray other) {
            if (skills != null && other.skills != null) {
                if (skills.Length != other.skills.Length) {
                    return false;
                }

                for (int i = 0; i < skills.Length; i++) {
                    if (skills[i] != other.skills[i]) {
                        return false;
                    }
                }
            } else if (skills != null || other.skills != null) {
                return false;
            }

            return true;
        }
    }

}