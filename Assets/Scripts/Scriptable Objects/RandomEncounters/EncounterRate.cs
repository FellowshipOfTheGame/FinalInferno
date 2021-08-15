using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "EncounterRate", menuName = "ScriptableObject/Encounter Rate")]
    public class EncounterRate : ScriptableObject {
        [Range(0, 100), SerializeField] private float baseEncounterRate = 5.0f;
        public float BaseEncounterRate { get => baseEncounterRate; }

        [Range(0, 100), SerializeField] private float rateIncreaseValue = 1f;
        public float RateIncreaseValue{ get => rateIncreaseValue; }

        [Range(1, 20), SerializeField] private int freeWalkDistance = 3;
        public float FreeWalkDistance { get => freeWalkDistance; }
        // Esse valor deve ser o valor mínimo do atributo Range acima
        public int MinFreeWalkDistance { get => 1; }
    }
}
