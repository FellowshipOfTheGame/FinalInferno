using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "MapEncounterList", menuName = "ScriptableObject/Map Encounter List")]
    public class MapEncounterList : ScriptableObject {
        [SerializeField] private List<EncounterGroup> encounterGroups;
    }
}
