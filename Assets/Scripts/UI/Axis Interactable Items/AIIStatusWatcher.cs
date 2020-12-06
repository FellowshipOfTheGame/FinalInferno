using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.FSM;

namespace FinalInferno.UI.AII
{
    public class AIIStatusWatcher : MonoBehaviour
    {
        [SerializeField] private BoolDecision hasActiveAII;
        [SerializeField] private int counter = 0;

        private List<AIIManager> managers = new List<AIIManager>();
        private int Counter{
            get => counter;
            set{
                counter = value;
                if(hasActiveAII != null){
                    hasActiveAII.UpdateValue(counter > 0);
                }
            }
        }

        public void ActivatedAII(AIIManager manager){
            if(!managers.Contains(manager)){
                managers.Add(manager);
                Counter++;
            }
        }

        public void DeactivatedAII(AIIManager manager){
            if(managers.Contains(manager)){
                managers.Remove(manager);
                Counter--;
            }
        }
    }
}