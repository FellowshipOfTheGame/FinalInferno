using UnityEngine;

namespace FinalInferno {
    public class MoveToTarget : MoveTo {
        [SerializeField] private float maxDistance = 0.5f;
        public Transform target;
        private Rigidbody2D rigid2D;

        private void Start() {
            rigid2D = GetComponent<Rigidbody2D>();
        }

        override public Vector2 Direction() {
            Vector2 target2Dposition = target.position;
            float distance = Vector2.Distance(rigid2D.position, target2Dposition);
            if (distance > maxDistance) {
                return (target2Dposition - rigid2D.position).normalized;
            }
            return Vector2.zero;
        }
    }
}
