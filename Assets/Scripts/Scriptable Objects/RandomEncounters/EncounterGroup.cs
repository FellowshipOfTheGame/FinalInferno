using System.Collections;
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
        [SerializeField, Range(0,1f)] private float difficultyRating = 0;
        public float DifficultyRating => difficultyRating;
    }
}
