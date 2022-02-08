using System;

namespace FinalInferno {
    [Serializable]
    public class EncounterGroupItem {
        public EncounterGroup group = null;
        public float chanceMultiplier = 1.0f;

        public EncounterGroupItem(EncounterGroup encounterGroup, float multiplier = 1.0f) {
            group = encounterGroup;
            chanceMultiplier = multiplier;
        }
    }
}
