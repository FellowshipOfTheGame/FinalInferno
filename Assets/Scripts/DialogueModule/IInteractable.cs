using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fog.Dialogue
{
    public interface IInteractable
    {
        void OnInteractAttempt(Agent agent, FinalInferno.Movable movingAgent);
    }
}
