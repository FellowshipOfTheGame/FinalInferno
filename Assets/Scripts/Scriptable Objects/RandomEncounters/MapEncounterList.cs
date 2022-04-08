using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "MapEncounterList", menuName = "ScriptableObject/Map Encounter List")]
    public class MapEncounterList : ScriptableObject, ISerializationCallbackReceiver {
        [SerializeField] private List<EncounterGroup> encounterGroups = null;
        [SerializeField] private List<EncounterGroupItem> encounterGroupItems = new List<EncounterGroupItem>();
        public bool HasEncounterGroup {
            get {
                foreach (EncounterGroupItem item in encounterGroupItems) {
                    if (item?.group != null) {
                        return true;
                    }
                }
                return false;
            }
        }
        [SerializeField] private float difficultyFactor = 0.8f;

        public ReadOnlyDictionary<EncounterGroup, float> GetChancesForLevel(int level) {
            Dictionary<EncounterGroup, float> chanceDict = new Dictionary<EncounterGroup, float>();
            level = Mathf.Clamp(level, 0, 4);
            float sumOfWeights = CalculateEncounterWeightsAndSum(level, chanceDict);
            CalculatePercentages(chanceDict, sumOfWeights);
            return new ReadOnlyDictionary<EncounterGroup, float>(chanceDict);
        }

        private float CalculateEncounterWeightsAndSum(int level, Dictionary<EncounterGroup, float> chanceDict) {
            float sumOfWeights = 0f;
            foreach (EncounterGroupItem encounterGroupItem in encounterGroupItems) {
                EncounterGroup encounterGroup = encounterGroupItem?.group;
                if (encounterGroup == null || !encounterGroup.CanEncounter[level]) {
                    continue;
                }
                float encounterWeight = CalculateEncounterWeight(encounterGroupItem, encounterGroup);
                chanceDict.Add(encounterGroup, encounterWeight);
                sumOfWeights += encounterWeight;
            }
            return sumOfWeights;
        }

        private float CalculateEncounterWeight(EncounterGroupItem encounterGroupItem, EncounterGroup encounterGroup) {
            float encounterWeight = 1f - (difficultyFactor * encounterGroup.DifficultyRating);
            encounterWeight *= encounterGroupItem.chanceMultiplier;
            encounterWeight = Mathf.Max(encounterWeight, Mathf.Epsilon);
            return encounterWeight;
        }

        private static void CalculatePercentages(Dictionary<EncounterGroup, float> chanceDict, float sumOfWeights) {
            foreach (EncounterGroup encounterGroup in new List<EncounterGroup>(chanceDict.Keys)) {
                chanceDict[encounterGroup] = chanceDict[encounterGroup] * 100f / sumOfWeights;
            }
        }

        #region ISerializationCallbackReceiver
        public void OnAfterDeserialize() {
            if (encounterGroups != null && encounterGroups.Count > 0) {
                encounterGroupItems.Clear();
                foreach (EncounterGroup group in encounterGroups) {
                    encounterGroupItems.Add(new EncounterGroupItem(group));
                }
                encounterGroups = null;
            }
            RemoveListDuplicates();
        }

        public void OnBeforeSerialize() {
            RemoveListDuplicates();
        }

        private void RemoveListDuplicates() {
            if(encounterGroupItems == null) {
                return;
            }
            HashSet<EncounterGroup> counter = new HashSet<EncounterGroup>();
            for (int index = 0; index < encounterGroupItems.Count; index++) {
                if (encounterGroupItems[index]?.group == null) {
                    continue;
                }
                if (counter.Contains(encounterGroupItems[index].group)) {
                    RemoveDuplicateEntry(index);
                } else {
                    counter.Add(encounterGroupItems[index].group);
                }
            }
        }

        private void RemoveDuplicateEntry(int index) {
            encounterGroupItems[index].group = null;
            encounterGroupItems[index].chanceMultiplier = 1.0f;
            Debug.LogWarning("Removing duplicate encounter group");
        }
        #endregion
    }
}
