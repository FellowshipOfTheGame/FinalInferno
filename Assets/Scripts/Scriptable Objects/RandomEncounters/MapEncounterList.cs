using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "MapEncounterList", menuName = "ScriptableObject/Map Encounter List")]
    public class MapEncounterList : ScriptableObject, ISerializationCallbackReceiver{
        [SerializeField] private List<EncounterGroup> encounterGroups = null;
        [SerializeField] private List<EncounterGroupItem> encounterGroupItems = new List<EncounterGroupItem>();
        public bool HasEncounterGroup {
            get {
                foreach (EncounterGroupItem item in encounterGroupItems) {
                    if(item?.group != null) return true;
                }
                return false;
            }
        }
        [SerializeField] private float difficultyFactor = 0.8f;
        public ReadOnlyDictionary<EncounterGroup, float> GetChancesForLevel(int level){
            Dictionary<EncounterGroup, float> dict = new Dictionary<EncounterGroup, float>();
            level = Mathf.Clamp(level, 0, 4);

            float accumulatedWeights = 0f;
            foreach(EncounterGroupItem encounterGroupItem in encounterGroupItems){
                EncounterGroup encounterGroup = encounterGroupItem?.group;
                if(encounterGroup == null || !encounterGroup.CanEncounter[level]) continue;

                float encounterWeight = 1f - (difficultyFactor * encounterGroup.DifficultyRating); 
                encounterWeight *= encounterGroupItem.chanceMultiplier;
                encounterWeight = Mathf.Max(encounterWeight, Mathf.Epsilon);
                accumulatedWeights += encounterWeight;
                dict.Add(encounterGroup, encounterWeight);
            }

            foreach(EncounterGroup encounterGroup in new List<EncounterGroup>(dict.Keys)){
                dict[encounterGroup] = dict[encounterGroup] * 100f / accumulatedWeights;
            }

            return new ReadOnlyDictionary<EncounterGroup, float>(dict);
        }

		public void OnAfterDeserialize() {
            if(encounterGroups != null && encounterGroups.Count > 0){
                encounterGroupItems.Clear();
                foreach(EncounterGroup group in encounterGroups){
                    encounterGroupItems.Add(new EncounterGroupItem(group));
                }
                encounterGroups = null;
            }
            RemoveListDuplicates();
		}

		public void OnBeforeSerialize() {
            RemoveListDuplicates();
		}

        private void RemoveListDuplicates(){
            HashSet<EncounterGroup> counter = new HashSet<EncounterGroup>();
            for(int i = 0; encounterGroupItems != null && i < encounterGroupItems.Count; i++){
                if(encounterGroupItems[i]?.group == null) continue;

                if(counter.Contains(encounterGroupItems[i].group)){
                    encounterGroupItems[i].group = null;
                    encounterGroupItems[i].chanceMultiplier = 1.0f;
                    Debug.LogWarning("Removing duplicate encounter group");
                }else{
                    counter.Add(encounterGroupItems[i].group);
                }
            }
        }
	}
}
