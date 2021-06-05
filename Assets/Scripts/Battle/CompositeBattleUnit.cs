using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [RequireComponent(typeof(BattleUnit))]
    public class CompositeBattleUnit : MonoBehaviour
    {
        private BattleUnit thisUnit;
        private bool alreadySetup = false;
        [SerializeField] private List<BattleUnit> appendages = new List<BattleUnit>();

        void Awake()
        {
            thisUnit = GetComponent<BattleUnit>();
        }

        public void AddApendage(BattleUnit newAppendage){
            // Garante que não seja possivel adicionar o mesmo elemento 2 vezes
            if(!appendages.Contains(newAppendage)){
                appendages.Add(newAppendage);
            }
        }

        public void RemoveAppendage(BattleUnit appendage){
            appendages.Remove(appendage);
        }

        // Precisa se certificar que isso vai ser chamado depois do Setup dos UnitItem.cs
        public void Setup(){
            // Remove os listeners de movimento padrão e faz com que todas as unidades se movam juntas
            thisUnit.OnTurnStart.RemoveAllListeners();
            thisUnit.OnTurnStart.AddListener(StepForward);
            thisUnit.OnTurnEnd.RemoveAllListeners();
            thisUnit.OnTurnEnd.AddListener(StepBack);
            foreach(BattleUnit appendage in appendages){
                appendage.transform.position = transform.position;
                appendage.OnTurnStart.RemoveAllListeners();
                appendage.OnTurnStart.AddListener(StepForward);
                appendage.OnTurnEnd.RemoveAllListeners();
                appendage.OnTurnEnd.AddListener(StepBack);
            }
        }

        public void StepForward(BattleUnit unit){
            // Debug.Log($"composite unit {unit} stepped forward");
            thisUnit.battleItem.StepForward(thisUnit);
            foreach(BattleUnit appendage in appendages){
                appendage.battleItem.StepForward(appendage);
            }
        }

        public void StepBack(BattleUnit unit){
            // Debug.Log($"composite unit {unit} stepped back");
            thisUnit.battleItem.StepBack(thisUnit);
            foreach(BattleUnit appendage in appendages){
                appendage.battleItem.StepBack(appendage);
            }
        }
    }
}