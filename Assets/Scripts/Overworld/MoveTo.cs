using UnityEngine;

namespace FinalInferno {
    [RequireComponent(typeof(Movable))]
    public abstract class MoveTo : MonoBehaviour {
        public abstract Vector2 Direction();
        public virtual void Activate() { }
        public virtual void Deactivate() { }

        public void Reset() {
            GetComponent<Movable>().moveTo = this;
        }
    }
}
