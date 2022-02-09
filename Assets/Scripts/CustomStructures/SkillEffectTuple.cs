namespace FinalInferno {
    [System.Serializable]
    public struct SkillEffectTuple {
        public SkillEffect effect;
        public float value1;
        public float value2;
        public void UpdateValues() {
            effect.value1 = value1;
            effect.value2 = value2;
        }
    }

}