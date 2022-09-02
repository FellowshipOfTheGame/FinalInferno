namespace FinalInferno {
    public interface ICompositeUnit {
        bool IsMainUnit(BattleUnit battleUnit);
        CompositeUnitInfo GetCompositeUnitInfo(BattleUnit mainBattleUnit);
    }
}
