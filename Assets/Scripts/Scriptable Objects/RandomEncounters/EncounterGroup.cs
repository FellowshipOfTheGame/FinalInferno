using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "EncounterGroup", menuName = "ScriptableObject/Encounter Group")]
    public class EncounterGroup : ScriptableObject {
        [SerializeField] private Enemy enemyA;
        public Enemy EnemyA => enemyA;
        [SerializeField] private Enemy enemyB;
        public Enemy EnemyB => enemyB;
        [SerializeField] private Enemy enemyC;
        public Enemy EnemyC => enemyC;
        [SerializeField] private Enemy enemyD;
        public Enemy EnemyD => enemyD;
        public Enemy this[int index]{
            get => index switch{
                0 => enemyA,
                1 => enemyB,
                2 => enemyC,
                3 => enemyD,
                _ => null
            };
        }
        [SerializeField, Range(0,1f)] private float difficultyRating = 0;
        public float DifficultyRating => difficultyRating;
        [SerializeField] private List<bool> canEncounter = new List<bool>{false, false, false, false, false};
        private ReadOnlyCollection<bool> canEncounterReadOnly = null;
        public ReadOnlyCollection<bool> CanEncounter{
            get{
                canEncounterReadOnly ??= canEncounter.AsReadOnly();
                return canEncounterReadOnly;
            }
        }
    }
}
