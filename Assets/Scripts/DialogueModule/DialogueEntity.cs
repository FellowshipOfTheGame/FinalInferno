using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fog.Dialogue{
    public abstract class DialogueEntity : ScriptableObject
    {
        public abstract Color DialogueColor { get; }
        public abstract string DialogueName { get; }
    }
}
