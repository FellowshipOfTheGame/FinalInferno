using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{

    [System.Serializable]
    public class Transition
    {
        public Decision[] decisions;
        public State nextState;
        public Action[] actions;
    }

}
