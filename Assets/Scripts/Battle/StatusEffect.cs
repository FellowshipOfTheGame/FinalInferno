using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public abstract class StatusEffect {
        [SerializeField] virtual public Sprite Icon{ get{ return null; } }
        protected float rollValue = 1.0f;
        public bool Failed { get; protected set; }
        public abstract StatusType Type { get; }
        private int duration;
        public int Duration { protected set{ duration = value; } get{ return (duration > int.MinValue)? duration : 99;} }
        private int turnsLeft;
        public int TurnsLeft { protected set{ turnsLeft = value; } get{ return (turnsLeft > int.MinValue)? turnsLeft : 99;} }
        public BattleUnit Source { protected set; get; }
        public BattleUnit Target { protected set; get; }
        public abstract float Value { get; } // Valor relevante para replicacao de efeitos

        public virtual void Amplify(float modifier){/*Multiplica os valores relevantes pelo modifier*/}
        public virtual void CopyTo(BattleUnit target, float modifier = 1.0f){/*Copia o status effect para um alvo e aplica um modifier*/}
        public virtual bool Apply(bool force = false){
            if(force || Type != StatusType.Debuff)
                return true;

            float roll = Random.Range(0f, rollValue);
            if(roll < Target.statusResistance)
                return false;
                
            return true;
        }
        public virtual void Remove(){}
        public virtual bool Update() {
            if(TurnsLeft == int.MinValue)
                return false;

            TurnsLeft--;
            if (TurnsLeft == 0){
                Remove();
                return true;
            }
            return false;
        }
    }
}
