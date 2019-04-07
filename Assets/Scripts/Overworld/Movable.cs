using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movable : MonoBehaviour {
	public float moveSpeed;
	public MoveTo nextPosition;
	private Rigidbody2D rigid2D;

	// Salva a referencia para o rigdigbody
	void Start(){
		rigid2D = GetComponent<Rigidbody2D>();
	}

	// Atualiza a velocidade atual de acordo com a direcao definida pelo script de MoveTo
	void FixedUpdate(){
		if(nextPosition != null){
			Vector2 direction = nextPosition.Direction();
			// No caso da direcao ser um vetor zero ou da moveSpeed ser 0, fica parado
			rigid2D.velocity = moveSpeed * direction;
		}
	}
}
