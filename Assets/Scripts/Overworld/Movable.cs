using UnityEngine;
using UnityEngine.Serialization;

namespace FinalInferno {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movable : MonoBehaviour {
        [SerializeField] private float moveSpeed = 5f;
        public float MoveSpeed {
            get => moveSpeed;
            set => moveSpeed = Mathf.Clamp(value, 0, float.MaxValue);
        }
        [FormerlySerializedAs("nextPosition")]
        public MoveTo moveTo;
        private bool canMove;
        public bool CanMove {
            get => canMove;
            set {
                if (value != canMove) {
                    rigid2D.velocity = Vector2.zero;
                    SetMoveToActive(value);
                }
                canMove = value;
            }
        }

        private void SetMoveToActive(bool value) {
            if (moveTo == null) {
                return;
            }
            if (value) {
                moveTo.Activate();
            } else {
                moveTo.Deactivate();
            }
        }

        private Rigidbody2D rigid2D;
        private Animator anim;

        public void Reset() {
            moveSpeed = 5f;
            ResetRigidbody();
        }

        private void ResetRigidbody() {
            Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
            rb2D.bodyType = RigidbodyType2D.Kinematic;
            rb2D.sharedMaterial = null;
            rb2D.simulated = true;
            rb2D.useFullKinematicContacts = true;
            rb2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb2D.sleepMode = RigidbodySleepMode2D.StartAwake;
            rb2D.interpolation = RigidbodyInterpolation2D.None;
            rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb2D.gravityScale = 0f;
        }

        private void Awake() {
            rigid2D = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        private void FixedUpdate() {
            if (moveTo == null || !canMove) {
                return;
            }
            Vector2 direction = moveTo.Direction();
            rigid2D.velocity = moveSpeed * direction;
        }

        private void Update() {
            if (anim == null) {
                return;
            }
            Vector2 normalizedVelocity = rigid2D.velocity.normalized;
            SetAnimationMoveSpeed(normalizedVelocity);
            SetAnimationDirection(normalizedVelocity);
        }

        private void SetAnimationMoveSpeed(Vector2 normalizedVelocity) {
            anim.SetBool("moving", rigid2D.velocity != Vector2.zero);
            anim.SetFloat("moveX", normalizedVelocity.x);
            anim.SetFloat("moveY", normalizedVelocity.y);
        }

        private void SetAnimationDirection(Vector2 normalizedVelocity) {
            if (HorizontalSpeedGreaterThanVertical()) {
                anim.SetFloat("directionY", 0f);
                anim.SetFloat("directionX", normalizedVelocity.x);
            } else if (VerticalSpeedGreaterThanHorizontal()) {
                anim.SetFloat("directionX", 0f);
                anim.SetFloat("directionY", normalizedVelocity.y);
            }
        }

        private bool HorizontalSpeedGreaterThanVertical() {
            return Mathf.Abs(rigid2D.velocity.x) - Mathf.Abs(rigid2D.velocity.y) > Mathf.Epsilon;
        }

        private bool VerticalSpeedGreaterThanHorizontal() {
            return Mathf.Abs(rigid2D.velocity.y) - Mathf.Abs(rigid2D.velocity.x) > Mathf.Epsilon;
        }
    }
}
