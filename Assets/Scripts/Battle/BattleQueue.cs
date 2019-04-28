using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleQueue{
    List<BattleUnit> list;

    //construtor
    public BattleQueue(){
        list = new List<BattleUnit>();
    }

    //insere uma BattleUnit na fila em ordem de actionPoints
    public void Enqueue(BattleUnit element, int additionalValue){
        element.actionPoints += additionalValue;
        
        int i;
        for(i = 0; i < list.Count && element.actionPoints >= list[i].actionPoints; i++);

        list.Insert(i, element);
    }

    //calcula a posição que o heroi ira entrar na fila pelo valor de actionPoints
    public int PreviewPosition(int actionPoints){
        int i;
        for(i = 0; i < list.Count && actionPoints >= list[i].actionPoints; i++);

        return i;
    }

    //retira e retorna a proxima BattleUnit que agira no combate
    public BattleUnit Dequeue(){
        BattleUnit bU = list[0];
        list.RemoveAt(0);
        return bU;
    }

    //retorna a BattleUnit da fila na posicao desejada
    public BattleUnit Peek(int position){
        if (list.Count == 0){ 
            Debug.Log("Queue is empty.");
            return null;
        }
        else
            return list[position];
    }

    //esvazia a fila
    public void Clear(){
        list.Clear();
    }

    // public void Print(){
    //     Debug.Log("INICIO:");
    //     foreach(BattleUnit bU in list){
    //         Debug.Log();
    //     }

    //     Debug.Log("FIM:");
    // }

    //retorna a quantidade de BattleUnit na fila
    public int Count(){
        return list.Count;
    }
}
