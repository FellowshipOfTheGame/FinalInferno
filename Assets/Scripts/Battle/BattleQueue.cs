﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class BattleQueue{
        public List<BattleUnit> list;

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
            int currentActionPoints = bU.actionPoints;
            foreach(BattleUnit bUnit in list){
                bUnit.actionPoints -= currentActionPoints;
            }
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

        public void Remove(BattleUnit unit){
            list.Remove(unit);
        }

        //esvazia a fila
        public void Clear(){
            list.Clear();
        }

        public void Sort(){
            list.Sort(CompareUnits);
        }

        private int CompareUnits(BattleUnit x, BattleUnit y){
            return x.actionPoints - y.actionPoints;
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
}
