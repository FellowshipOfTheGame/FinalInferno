using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FinalInferno.EventSystem{
    public class EventsListenerFI : MonoBehaviour, IEventListenerFI
    {
        [SerializeField] private List<EventFI> _events = new List<EventFI>();
        [SerializeField] private UnityEvent _response;
        // Start is called before the first frame update

        void OnEnable(){
            foreach(EventFI _event in _events){
                if(_event != null){
                    _event.AddListener(this);
                }
            }
        }

        void OnDisable(){
            foreach(EventFI _event in _events){
                if(_event != null){
                    _event.RemoveListener(this);
                }
            }
        }

        public void OnEventRaised(){
            if(_response != null){
                _response.Invoke();
            }
        }
    }
}