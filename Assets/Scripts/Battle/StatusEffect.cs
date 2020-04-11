using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public abstract class StatusEffect {
        [SerializeField] virtual public Sprite Icon{ get{ return null; } }
        private List<StatusEffectVFX> persistentSourceVFX = new List<StatusEffectVFX>();
        private List<StatusEffectVFX> persistentTargetVFX = new List<StatusEffectVFX>();
        public List<StatusEffectVFX> updateSourceVFX = new List<StatusEffectVFX>();
        public List<StatusEffectVFX> updateTargetVFX = new List<StatusEffectVFX>();
        public List<StatusEffectVFX> removeSourceVFX = new List<StatusEffectVFX>();
        public List<StatusEffectVFX> removeTargetVFX = new List<StatusEffectVFX>();
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

            // Destroy all vfx created
            garbageCollector.AddRange(persistentSourceVFX);
            garbageCollector.AddRange(persistentTargetVFX);
            foreach(StatusEffectVFX vfx in garbageCollector){
                vfx.DestroyVFX();
            }

            // Creates removal effects; These MUST have a destroy event at the end of animation
            foreach(StatusEffectVFX vfx in removeSourceVFX){
                StatusEffectVFX newvfx = GameObject.Instantiate(vfx, Source.transform.parent);
                newvfx.GetComponent<SpriteRenderer>().sortingOrder = Source.GetComponent<SpriteRenderer>().sortingOrder + 1;
            }
            foreach(StatusEffectVFX vfx in removeTargetVFX){
                StatusEffectVFX newvfx = GameObject.Instantiate(vfx, Target.transform.parent);
                newvfx.GetComponent<SpriteRenderer>().sortingOrder = Target.GetComponent<SpriteRenderer>().sortingOrder + 1;
            }
        }
        public virtual void ForceRemove(){
            Remove();
        }

        public void AddPersistentVFX(List<StatusEffectVFX> source, List<StatusEffectVFX> target){
            if(source != null)
                persistentSourceVFX.AddRange(source);
            if(target != null)
                persistentTargetVFX.AddRange(target);
        }
        
        public virtual bool Update() { // Retorna true quando o status effect foi removido
            if(TurnsLeft == int.MinValue){
                return false;
            }

            TurnsLeft--;

            // Updates or creates visual effects as needed
            foreach(StatusEffectVFX vfx in persistentSourceVFX){
                vfx.TurnsLeft = TurnsLeft;
            }
            foreach(StatusEffectVFX vfx in persistentTargetVFX){
                vfx.TurnsLeft = TurnsLeft;
            }
            foreach(StatusEffectVFX vfx in updateTargetVFX){
                StatusEffectVFX newvfx = GameObject.Instantiate(vfx, Target.transform.parent);
                newvfx.GetComponent<SpriteRenderer>().sortingOrder = Target.GetComponent<SpriteRenderer>().sortingOrder + 1;
                garbageCollector.Add(newvfx);
            }
            foreach(StatusEffectVFX vfx in updateSourceVFX){
                StatusEffectVFX newvfx = GameObject.Instantiate(vfx, Source.transform.parent);
                newvfx.GetComponent<SpriteRenderer>().sortingOrder = Source.GetComponent<SpriteRenderer>().sortingOrder + 1;
                garbageCollector.Add(newvfx);
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
