using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FinalInferno{
	public class MoveToInput : MoveTo {
		private Vector2 input;
		private bool isActive;
		[SerializeField] private InputActionReference movementAction;

		// O objeto deve estar parado inicialmente
		void Start(){
			input = Vector2.zero;
		}

		public override void Activate(){
			input = Vector2.zero;
			isActive = true;
		}

		public override void Deactivate(){
			input = Vector2.zero;
			isActive = false;
		}

		// A direcao a ser seguida e determinada apenas pelo input
		override public Vector2 Direction(){
			return input.normalized;
		}

		// Update is called once per frame
		void Update() {
			if(isActive){
				input = movementAction.action.ReadValue<Vector2>();
				// input.x = UnityEngine.Input.GetAxisRaw("Horizontal");
				// input.y = UnityEngine.Input.GetAxisRaw("Vertical");
			}
		}
}
}
