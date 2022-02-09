using UnityEngine;

namespace FinalInferno {
    [System.Serializable]
    public struct QuestInfo {
        [SerializeField] public string name;
        [SerializeField] public string[] flagsNames;
        [SerializeField] public ulong flagsTrue;
        public static bool operator ==(QuestInfo left, QuestInfo right) {
            return left.Equals(right);
        }
        public static bool operator !=(QuestInfo left, QuestInfo right) {
            return !(left == right);
        }
        public override bool Equals(object obj) {
            if (obj.GetType() != typeof(QuestInfo)) {
                return false;
            }

            return Equals((QuestInfo)obj);
        }
        public bool Equals(QuestInfo other) {
            if (name != other.name) {
                return false;
            }

            if (flagsNames != null && other.flagsNames != null) {
                if (flagsNames.Length != other.flagsNames.Length) {
                    return false;
                }

                for (int i = 0; i < flagsNames.Length; i++) {
                    if (flagsNames[i] != other.flagsNames[i]) {
                        return false;
                    }
                }
            } else if (flagsNames != null || other.flagsNames != null) {
                return false;
            }

            if (flagsTrue != other.flagsTrue) {
                return false;
            }

            return true;
        }
        public override int GetHashCode() {
            return (3 * name.GetHashCode() + 5 * flagsNames.GetHashCode() + 7 * flagsTrue.GetHashCode());
        }
    }

}