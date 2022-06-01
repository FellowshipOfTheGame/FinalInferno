using UnityEngine;

namespace FinalInferno {
    /*
    Para adicionar um novo status effect que tenha efeito visual:
        - Adicionar o Status effect no enum StatusEffectVisuals
        - Dar override na propriedade VFXID para o novo valor criado
        - Criar um objeto vazio como filho do objeto com componente StatusVFXHandler no Prefab da unidade
            - Esse objeto filho deve ter o mesmo nome que a entrada criada no enum
        - Adicionar os objetos do tipo StatusEffectVFX como filhos desse novo objeto
            - A posição relativa desses objetos deve evitar overlaps com outros efeitos
            - Todos os objetos aqui receberão os parametros de animação de maneira igual
            - Comportamentos diferentes devem ser definidos pela máquina de estados
    */
    public abstract class StatusEffect {
        [SerializeField] virtual public Sprite Icon => null;
        public abstract StatusEffectVisuals VFXID { get; }
        protected float rollValue = 1.0f;
        public bool Failed { get; protected set; }
        public bool Removed { get; protected set; } = false;
        public abstract StatusType Type { get; }
        private int duration;
        public virtual int Duration { protected set => duration = value; get => (duration > int.MinValue) ? duration : 99; }
        private int turnsLeft;
        public virtual int TurnsLeft { protected set => turnsLeft = value; get => (turnsLeft > int.MinValue) ? turnsLeft : 99; }
        public BattleUnit Source { protected set; get; }
        public BattleUnit Target { protected set; get; }
        public abstract float Value { get; }
        public virtual float AggroOnApply => 0f;
        public virtual float AggroOnUpdate => 0f;

        public virtual void Amplify(float modifier) { }
        public virtual void CopyTo(BattleUnit target, float modifier = 1.0f) { }
        public virtual bool Apply(bool force = false) {
            if (force || Type != StatusType.Undesirable)
                return true;

            float roll = Random.Range(0f, rollValue);
            return roll >= Target.statusResistance;
        }
        public virtual void Remove() {
            Removed = true;
            Target.RemoveEffect(this);
        }

        public virtual void ForceRemove() {
            Remove();
        }

        public virtual bool Update() { // Retorna true quando o status effect foi removido
            if (TurnsLeft == int.MinValue)
                return false;

            TurnsLeft--;
            Source.aggro += AggroOnUpdate;
            if (TurnsLeft >= 0)
                return false;

            Remove();
            return true;
        }
    }
}
