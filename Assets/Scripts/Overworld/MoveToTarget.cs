using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
	public class MoveToTarget : MoveTo{
		[SerializeField] private float maxDistance = 0.5f;
		public Transform target;
		private Rigidbody2D rigid2D;

		// Salva a referencia para o rigidbody
		void Start() {
			rigid2D = GetComponent<Rigidbody2D>();
		}

		override public Vector2 Direction(){
			// Calcula a distancia ate o alvo a ser seguido
			Vector2 target2Dposition = new Vector2(target.position.x, target.position.y);
			float distance = Vector2.Distance(rigid2D.position, target2Dposition);
			// Se estiver acima da distancia maxima permitida, retorna a direcao ao alvo normalizada
			if(distance > maxDistance)
				return (target2Dposition - rigid2D.position).normalized;
			// Caso contrario, fica parado
			return Vector2.zero;
		}
	}
}
