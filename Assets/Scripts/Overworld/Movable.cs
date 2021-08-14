using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
	[RequireComponent(typeof(Rigidbody2D))]
	public class Movable : MonoBehaviour {
		[SerializeField] private float moveSpeed = 5f;
		public float MoveSpeed{
			get => moveSpeed;
			set => moveSpeed = Mathf.Clamp(value, 0, float.MaxValue);
		}
		public MoveTo nextPosition;
		private bool canMove;
		public bool CanMove{
			get { return canMove; }
			set {
				if(value != canMove){
					rigid2D.velocity = Vector2.zero;
					if(nextPosition != null){
						if(value){
							nextPosition.Activate();
						}else{
							nextPosition.Deactivate();
						}
					}
				}
				canMove = value;
			}
		}
		private Rigidbody2D rigid2D;
		private Animator anim;

		public void Reset(){
			moveSpeed = 5f;
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

		// Salva a referencia para o rigdigbody
		void Awake(){
			//canMove = true;
			rigid2D = GetComponent<Rigidbody2D>();
			anim = GetComponent<Animator>();
		}

		// Atualiza a velocidade atual de acordo com a direcao definida pelo script de MoveTo
		void FixedUpdate(){
			if(nextPosition != null && canMove){
				Vector2 direction = nextPosition.Direction();
				// No caso da direcao ser um vetor zero ou da moveSpeed ser 0, fica parado
				rigid2D.velocity = moveSpeed * direction;
			}
			if (anim != null) {
				// TO DO Setar umas propriedades e usar o update para pegar as informações e atualizar o animator
				anim.SetBool("moving", rigid2D.velocity != Vector2.zero);
				anim.SetFloat("moveX", rigid2D.velocity.normalized.x);
				anim.SetFloat("moveY", rigid2D.velocity.normalized.y);
				if(Mathf.Abs(rigid2D.velocity.x) - Mathf.Abs(rigid2D.velocity.y) > Mathf.Epsilon){
					anim.SetFloat("directionY", 0f);
					anim.SetFloat("directionX", rigid2D.velocity.normalized.x);
				}else if(Mathf.Abs(rigid2D.velocity.y) - Mathf.Abs(rigid2D.velocity.x) > Mathf.Epsilon){
					anim.SetFloat("directionX", 0f);
					anim.SetFloat("directionY", rigid2D.velocity.normalized.y);
				}
			}
		}
	}
}
