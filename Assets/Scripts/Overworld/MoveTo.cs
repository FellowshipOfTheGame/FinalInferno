using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
	[RequireComponent(typeof(Movable))]
	public abstract class MoveTo : MonoBehaviour {
		// Funcao que retorna a direcao na qual o objeto deve se mover
		// Apropriada para movimentacao baseada em velocidade de Rigidbody2D
		public abstract Vector2 Direction();
		// Talvez seja possivel fazer scripts que definem uma posicao ao inves de direcao
		// Mas isso é mais apropriado para movimentacao com translacoes, entao nao usaremos
		//public abstract Vector2 Position();
		public virtual void Activate() {}
		public virtual void Deactivate() {}

		public void Reset(){
			GetComponent<Movable>().nextPosition = this;
		}
	}
}
