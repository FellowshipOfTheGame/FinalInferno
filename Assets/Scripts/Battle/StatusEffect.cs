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

        public virtual void Amplify(float modifier){/*Multiplica os valores relevantes pelo modifier*/}
        public virtual void Apply(){}
        public virtual void Remove(){}
        public virtual void Update() {
            TurnsLeft--;
            if (TurnsLeft == 0)
                Remove();
        }
    }
}
