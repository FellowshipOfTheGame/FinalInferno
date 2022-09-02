using System.Collections;
using System.Collections.Generic;

namespace FinalInferno {
    public struct CompositeUnitInfo : IEnumerable<BattleUnit> {
        public BattleUnit mainUnit;
        public List<BattleUnit> appendages;
        private List<BattleUnit> units;

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

        public IEnumerator<BattleUnit> GetEnumerator() {
            return ((IEnumerable<BattleUnit>)units).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)units).GetEnumerator();
        }
    }
}
