using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
	public class MoveToInput : MoveTo {
		private Vector2 input;

		// O objeto deve estar parado inicialmente
		void Start(){
			input = Vector2.zero;
		}

		// A direcao a ser seguida e determinada apenas pelo input
		override public Vector2 Direction(){
			return input.normalized;
		}

		// Update is called once per frame
		void Update() {
			if (!PauseMenu.isPaused)
			{
				input.x = Input.GetAxisRaw("Horizontal");
				input.y = Input.GetAxisRaw("Vertical");
			}
			else
				input = Vector2.zero;
		}
}
}
