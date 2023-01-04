using System;
using UnityEngine;
using FinalInferno.EventSystem;

namespace FinalInferno {
    [Serializable]
    public class BattleUnitInstantiator {
        [SerializeField] private GameObject unitPrefab;
        [SerializeField] private BattleUnitEventFI unitInstantiatedEvent;
        public BattleUnit InstantiateNewBattleUnit(Unit unit) {
            GameObject newUnit = GameObject.Instantiate(unitPrefab, null);
            BattleUnit battleUnit = newUnit.GetComponent<BattleUnit>();
            battleUnit.Configure(unit);
            unitInstantiatedEvent.Raise(battleUnit);
            battleUnit.OnSizeChanged?.Invoke();
            SetupIfEnemyUnit(unit, battleUnit);
            return battleUnit;
        }

        private static void SetupIfEnemyUnit(Unit unit, BattleUnit newUnit) {
            if (!(unit is Enemy))
                return;
            newUnit.ChangeColor();
            (newUnit.Unit as Enemy).ResetParameters();
        }
    }
}
