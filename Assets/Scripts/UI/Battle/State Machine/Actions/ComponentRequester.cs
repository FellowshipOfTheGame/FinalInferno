using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    public abstract class ComponentRequester : Action
    {
        public abstract void RequestComponent(GameObject provider);
    }

}