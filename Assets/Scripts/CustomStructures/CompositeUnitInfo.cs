using System.Collections.Generic;

namespace FinalInferno {
    public struct CompositeUnitInfo {
        public BattleUnit mainUnit;
        public List<BattleUnit> appendages;
        public List<BattleUnit> units;

        public void AddAppendage(BattleUnit newAppendage) {
            appendages.Add(newAppendage);
            units.Add(newAppendage);
        }

        public CompositeUnitInfo(BattleUnit mainUnit, List<BattleUnit> appendages) {
            this.mainUnit = mainUnit;
            this.appendages = new List<BattleUnit>(appendages);
            units = new List<BattleUnit>(appendages) { mainUnit };
        }

        public CompositeUnitInfo(BattleUnit mainUnit) {
            this.mainUnit = mainUnit;
            appendages = new List<BattleUnit>();
            units = new List<BattleUnit>() { mainUnit };
        }
    }
}
