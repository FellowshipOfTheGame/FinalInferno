using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.EventSystem{
    public class EventRaiserFI : MonoBehaviour
    {
        public void RaiseEvent(EventFI eventRaised){
            if(eventRaised != null){
                eventRaised.Raise();
            }
        }
    }
}