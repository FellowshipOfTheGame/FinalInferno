using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    [RequireComponent(typeof(BattleUnit))]
    public class CompositeBattleUnit : MonoBehaviour {
        private BattleUnit thisUnit;
        [SerializeField] private List<BattleUnit> appendages = new List<BattleUnit>();

        private void Awake() {
            thisUnit = GetComponent<BattleUnit>();
        }

        public void AddApendage(BattleUnit newAppendage) {
            if (!appendages.Contains(newAppendage))
                appendages.Add(newAppendage);
        }

        public void RemoveAppendage(BattleUnit appendage) {
            appendages.Remove(appendage);
        }

        // Precisa se certificar que isso vai ser chamado depois do Setup dos UnitItem.cs
        public void Setup() {
            OverrideTurnCallbacks(thisUnit);
            foreach (BattleUnit appendage in appendages) {
                appendage.transform.position = transform.position;
                OverrideTurnCallbacks(appendage);
            }
        }

        private void OverrideTurnCallbacks(BattleUnit battleUnit) {
            battleUnit.OnTurnStart.RemoveAllListeners();
            battleUnit.OnTurnStart.AddListener(StepForward);
            battleUnit.OnTurnEnd.RemoveAllListeners();
            battleUnit.OnTurnEnd.AddListener(StepBack);
        }

        public void StepForward(BattleUnit unit) {
            thisUnit.battleItem.StepForward(thisUnit);
            foreach (BattleUnit appendage in appendages) {
                appendage.battleItem.StepForward(appendage);
            }
        }

        public void StepBack(BattleUnit unit) {
            thisUnit.battleItem.StepBack(thisUnit);
            foreach (BattleUnit appendage in appendages) {
                appendage.battleItem.StepBack(appendage);
            }
        }
    }
}