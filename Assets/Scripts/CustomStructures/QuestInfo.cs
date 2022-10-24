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

        public void SaveQuestInfo(Quest quest) {
            name = quest.SerializedID;
            flagsNames = quest.GetSerializableFlagNames();
            System.Array.Sort(flagsNames);
            CopyQuestFlags(quest);
        }

        private void CopyQuestFlags(Quest quest) {
            flagsTrue = 0;
            ulong bitValue = 1;
            for (int j = 0; j < quest.EventCount; j++) {
                flagsTrue |= bitValue * (quest.GetFlag(flagsNames[j]) ? 1 : (ulong)0);
                bitValue <<= 1;
            }
        }

        public void LoadQuestInfo(Quest quest) {
            if (quest.SerializedID != name) {
                Debug.LogError("Tried to apply quest info to wrong ScriptableObject", quest);
                return;
            }
            ulong bitValue = 1;
            for (int i = 0; i < quest.EventCount; i++) {
                quest.SetFlag(flagsNames[i], (flagsTrue & bitValue) != 0);
                bitValue = bitValue << 1;
            }
        }
    }
}