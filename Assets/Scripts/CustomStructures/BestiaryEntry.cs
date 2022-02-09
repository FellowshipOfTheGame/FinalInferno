using UnityEngine;

namespace FinalInferno {
    [System.Serializable]
    public struct BestiaryEntry {
        [SerializeField] public string monsterName;
        [SerializeField] public int numberKills;
        public BestiaryEntry(Enemy enemy, int n) {
            if (enemy != null) {
                monsterName = enemy.AssetName;
            } else {
                monsterName = "";
            }
            numberKills = Mathf.Max(1, n);
        }
        public static bool operator ==(BestiaryEntry left, BestiaryEntry right) {
            return left.Equals(right);
        }
        public static bool operator !=(BestiaryEntry left, BestiaryEntry right) {
            return !(left == right);
        }
        public override bool Equals(object obj) {
            if (obj.GetType() != typeof(BestiaryEntry)) {
                return false;
            }

            return Equals((BestiaryEntry)obj);
        }
        public bool Equals(BestiaryEntry other) {
            if (monsterName != other.monsterName || numberKills != other.numberKills) {
                return false;
            }

            return true;
        }
        public override int GetHashCode() {
            return (3 * monsterName.GetHashCode() + 5 * numberKills.GetHashCode());
        }
    }

}