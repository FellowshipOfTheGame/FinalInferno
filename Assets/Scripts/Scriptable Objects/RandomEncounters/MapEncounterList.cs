using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "MapEncounterList", menuName = "ScriptableObject/Map Encounter List")]
    public class MapEncounterList : ScriptableObject, ISerializationCallbackReceiver{
        [SerializeField] private List<EncounterGroup> encounterGroups;
        // O editor tava com uns comportamentos estranhos com isso aqui
        // [SerializeField, Range(0.1f, 0.9f)] private float difficultyFactor = 0.8f;
        private const float difficultyFactor = 0.8f;
        public ReadOnlyDictionary<EncounterGroup, float> GetChancesForLevel(int level){
            Dictionary<EncounterGroup, float> dict = new Dictionary<EncounterGroup, float>();
            level = Mathf.Clamp(level, 0, 4);

            float accumulatedWeights = 0f;
            foreach(EncounterGroup encounterGroup in encounterGroups){
                if(encounterGroup == null || !encounterGroup.CanEncounter[level]) continue;

                float encounterWeight = 1f - (difficultyFactor * encounterGroup.DifficultyRating); 
                accumulatedWeights += encounterWeight;
                dict.Add(encounterGroup, encounterWeight);
            }

            foreach(EncounterGroup encounterGroup in new List<EncounterGroup>(dict.Keys)){
                dict[encounterGroup] = dict[encounterGroup] * 100f / accumulatedWeights;
            }

            return new ReadOnlyDictionary<EncounterGroup, float>(dict);
        }

		public void OnAfterDeserialize() {
            RemoveListDuplicates();
		}

		public void OnBeforeSerialize() {
            RemoveListDuplicates();
		}

        private void RemoveListDuplicates(){
            List<EncounterGroup> counter = new List<EncounterGroup>();
            for(int i = 0; i < encounterGroups.Count; i++){
                if(encounterGroups[i] == null) continue;
                if(counter.Contains(encounterGroups[i])){
                    encounterGroups[i] = null;
                    Debug.LogWarning("Removing duplicate encounter group");
                }else{
                    counter.Add(encounterGroups[i]);
                }
            }
        }
	}
}
