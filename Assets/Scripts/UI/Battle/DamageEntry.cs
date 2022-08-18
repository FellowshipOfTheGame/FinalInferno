namespace FinalInferno.UI.Battle {
    public struct DamageEntry {
        public int value;
        public bool isHeal;
        public DamageStrength strength;
        public DamageEntry(int val, bool heal, DamageStrength str) {
            value = val;
            isHeal = heal;
            strength = str;
        }
    }
}
