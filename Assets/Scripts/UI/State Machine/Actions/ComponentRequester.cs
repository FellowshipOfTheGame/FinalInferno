using UnityEngine;

namespace FinalInferno.UI.FSM {
    public abstract class ComponentRequester : Action {
        public abstract void RequestComponent(GameObject provider);
    }
}