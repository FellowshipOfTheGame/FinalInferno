using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public abstract class StatusEffect {
        [SerializeField] virtual public Sprite Icon{ get{ return null; } }
        public List<StatusEffectVFX> sourceVFX = new List<StatusEffectVFX>();
        public List<StatusEffectVFX> targetVFX = new List<StatusEffectVFX>();
        private List<StatusEffectVFX> garbageCollector = new List<StatusEffectVFX>();
        protected float rollValue = 1.0f;
        public bool Failed { get; protected set; }
        public abstract StatusType Type { get; }
        private int duration;
        public virtual int Duration { protected set{ duration = value; } get{ return (duration > int.MinValue)? duration : 99;} }
        private int turnsLeft;
        public virtual int TurnsLeft { protected set{ turnsLeft = value; } get{ return (turnsLeft > int.MinValue)? turnsLeft : 99;} }
        public BattleUnit Source { protected set; get; }
        public BattleUnit Target { protected set; get; }
        public abstract float Value { get; } // Valor relevante para replicacao de efeitos
        public virtual float AggroOnApply { get{ return 0f; } } // Para ser usado na geração de aggro ao aplicar buffs e debuffs
        public virtual float AggroOnUpdate { get{ return 0f; } } // Está aqui por precaução mas não acho que usaremos

        public virtual void Amplify(float modifier){/*Multiplica os valores relevantes pelo modifier*/}
        public virtual void CopyTo(BattleUnit target, float modifier = 1.0f){/*Copia o status effect para um alvo e aplica um modifier*/}
        public virtual bool Apply(bool force = false){ // Retorna true quando o status effect for aplicado de maneira bem sucedida
            if(force || Type != StatusType.Undesirable)
                return true;

            float roll = Random.Range(0f, rollValue);
            if(roll < Target.statusResistance)
                return false;
                
            return true;
        }
        public virtual void Remove(){
            Target.effects.Remove(this);

            // Os efeitos que não tem uma animação de remoção se autodestroem
            // As animações que são chamadas com o trigger de remove dever usar o evento de destruição
            foreach(StatusEffectVFX vfx in sourceVFX){
                vfx?.RemoveTrigger();
            }
            foreach(StatusEffectVFX vfx in targetVFX){
                vfx?.RemoveTrigger();
            }
        }
        public virtual void ForceRemove(){
            Remove();
        }

        public void AddPersistentVFX(List<StatusEffectVFX> source, List<StatusEffectVFX> target){
            if(source != null){
                sourceVFX.AddRange(source);
            }
            foreach(StatusEffectVFX vfx in sourceVFX){
                // TO DO: instanciar a prefab
                vfx.GetComponent<SpriteRenderer>().sortingOrder = Source.GetComponent<SpriteRenderer>().sortingOrder + 1;
            }

            if(target != null){
                targetVFX.AddRange(target);
            }
            foreach(StatusEffectVFX vfx in targetVFX){
                vfx.GetComponent<SpriteRenderer>().sortingOrder = Target.GetComponent<SpriteRenderer>().sortingOrder + 1;
            }
        }
        
        public virtual bool Update() { // Retorna true quando o status effect foi removido
            if(TurnsLeft == int.MinValue){
                return false;
            }

            TurnsLeft--;

            // Updates or creates visual effects as needed
            foreach(StatusEffectVFX vfx in sourceVFX){
                vfx.TurnsLeft = TurnsLeft;
                vfx.UpdateTrigger();
            }
            foreach(StatusEffectVFX vfx in targetVFX){
                vfx.TurnsLeft = TurnsLeft;
                vfx.UpdateTrigger();
            }

            Source.aggro += AggroOnUpdate;
            if (TurnsLeft < 0){
                Remove();
                return true;
            }
            return false;
        }
    }
}
