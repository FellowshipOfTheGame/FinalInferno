namespace FinalInferno {
    public interface IDamageIndicator {
        void Setup();
        void ShowDamage(int value, bool isHeal, float multiplier);
        void ShowMiss();
    }
}
