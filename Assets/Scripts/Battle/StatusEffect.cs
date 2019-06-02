using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public abstract class StatusEffect {
        public abstract StatusType Type { get; }
        public int Duration { protected set; get; }
        public int TurnsLeft { protected set; get; }
        public BattleUnit Source { protected set; get; }
        public BattleUnit Target { protected set; get; }
        public abstract float Value { get; } // Valor relevante para replicacao de efeitos

        public abstract void Apply();
        public abstract void Remove();
        public void Update() {
            TurnsLeft--;
            if (TurnsLeft == 0)
                Remove();
        }
    }
}
