using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "EncounterRate", menuName = "ScriptableObject/Encounter Rate")]
    public class EncounterRate : ScriptableObject {
        [Range(0, 100), SerializeField] private float baseEncounterRate = 5.0f;
        public float BaseEncounterRate => baseEncounterRate;

        [Range(0, 100), SerializeField] private float rateIncreaseValue = 1f;
        public float RateIncreaseValue => rateIncreaseValue;

        [Range(1, 20), SerializeField] private int freeWalkDistance = 3;
        public float FreeWalkDistance => freeWalkDistance;
        // O valor abaixo deve ser o valor mínimo do atributo Range em freeWalkDistance
        public int MinFreeWalkDistance => 1;
    }
}
