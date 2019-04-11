using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.FSM
{
    public class SetAII : MonoBehaviour
    {
        [SerializeField] private ActiveAIIAction AA;
        [SerializeField] private DesactiveAIIAction DA;
        void Start()
        {
            AIIManager manager = GetComponent<AIIManager>();
            AA.SetAII(manager);
            DA.SetAII(manager);
        }

    }

}