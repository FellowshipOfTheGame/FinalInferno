using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "UnitIndex", menuName = "ScriptableObject/UnitIndex")]
    public class UnitIndex : ScriptableObject
    {
        [SerializeField] private List<Unit> unitList = new List<Unit>();
        public ReadOnlyCollection<Unit> UnitList { get; private set; }

        void OnEnable(){
            UnitList = unitList.AsReadOnly();
        }
    }
}