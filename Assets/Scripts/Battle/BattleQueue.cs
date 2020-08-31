using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    using UI.Battle.QueueMenu;

    public class BattleQueue{
        private BattleQueueUI queueUI;
        private List<BattleUnit> list;

        //construtor
        public BattleQueue(BattleQueueUI ui){
            queueUI = ui;
            list = new List<BattleUnit>();
        }

        //insere uma BattleUnit na fila em ordem de actionPoints
        public void Enqueue(BattleUnit element, int additionalValue){
            element.actionPoints += additionalValue;
            
            int i;
            for(i = 0; i < list.Count && element.actionPoints >= list[i].actionPoints; i++);

            list.Insert(i, element);
            queueUI.UpdateQueue(BattleManager.instance.currentUnit);
        }

        //calcula a posição que o heroi ira entrar na fila pelo valor de actionPoints
        public int PreviewPosition(int actionPoints){
            int i;
            for(i = 0; i < list.Count && actionPoints >= list[i].actionPoints; i++);

            queueUI.StartPreview(i);
            return i;
        }

        public void StopPreview(){
            queueUI.StopPreview();
        }

        public bool Contains(BattleUnit unit){
            return list.Contains(unit);
        }

        //retira e retorna a proxima BattleUnit que agira no combate
        public BattleUnit Dequeue(){
            BattleUnit bU = list[0];
            int currentActionPoints = bU.actionPoints;
            foreach(BattleUnit bUnit in list){
                bUnit.actionPoints -= currentActionPoints;
            }
            list.RemoveAt(0);

            queueUI.UpdateQueue(bU);
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
            queueUI.UpdateQueue(BattleManager.instance.currentUnit);
        }

        //esvazia a fila
        public void Clear(){
            list.Clear();
            queueUI.UpdateQueue(BattleManager.instance.currentUnit);
        }

        public void Sort(){
            list.Sort(CompareUnits);
            queueUI.UpdateQueue(BattleManager.instance.currentUnit);
        }

        private int CompareUnits(BattleUnit x, BattleUnit y){
            if(x.actionPoints == y.actionPoints){
                return y.curSpeed - x.curSpeed;
            }
            return x.actionPoints - y.actionPoints;
        }

        //retorna a quantidade de BattleUnit na fila
        public int Count{
            get => list.Count;
        }

        public IEnumerator<BattleUnit> GetEnumerator(){
            return list.GetEnumerator();
        }

        public BattleUnit[] ToArray(){
            return list.ToArray();
        }
    }
}
