using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    public abstract class Action : ScriptableObject 
    {
        public abstract void Act (StateController controller);
    }

}
