using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using FinalInferno.EventSystem;

namespace FinalInferno {
    [Serializable]
    public class BattleQueue : IEnumerable<BattleUnit> {
        private List<BattleUnit> list = new List<BattleUnit>();
        public int Count => list.Count;
        public UnityEvent<BattleUnit> OnUpdateQueue { get; private set; } = new UnityEvent<BattleUnit>();
        [SerializeField] private EventFI stopQueuePreviewEvent;
        [SerializeField] private IntEventFI startQueuePreviewEvent;
        [SerializeField] private BattleUnitEventFI removedUnitEvent;
        [SerializeField] private BattleUnitEventFI reinsertedUnitEvent;

        public void ReinsertToQueue(BattleUnit element) {
            Enqueue(element, 0);
            reinsertedUnitEvent.Raise(element);
        }

        public void Enqueue(BattleUnit element, int additionalValue) {
            element.actionPoints += additionalValue;
            int newIndex = CalculateNewPosition(element.actionPoints);
            list.Insert(newIndex, element);
            OnUpdateQueue?.Invoke(BattleManager.instance.CurrentUnit);
        }

        private int CalculateNewPosition(int actionPoints) {
            int predictedIndex;
            for (predictedIndex = 0; predictedIndex < list.Count && actionPoints >= list[predictedIndex].actionPoints; predictedIndex++)
                continue;
            return predictedIndex;
        }

        public int PreviewPosition(int actionPoints) {
            int predictedIndex = CalculateNewPosition(actionPoints);
            startQueuePreviewEvent.Raise(predictedIndex);
            return predictedIndex;
        }

        public void StopPreview() {
            stopQueuePreviewEvent.Raise();
        }

        public bool Contains(BattleUnit unit) {
            return list.Contains(unit);
        }

        public BattleUnit Dequeue() {
            BattleUnit firstUnit = list[0];
            int currentActionPoints = firstUnit.actionPoints;
            foreach (BattleUnit battleUnit in list) {
                battleUnit.actionPoints -= currentActionPoints;
            }
            list.RemoveAt(0);
            OnUpdateQueue?.Invoke(firstUnit);
            return firstUnit;
        }

        public BattleUnit Peek(int position) {
            return list.Count > 0 && position >= 0 && position < list.Count ? list[position] : null;
        }

        public void Remove(BattleUnit unit) {
            list.Remove(unit);
            removedUnitEvent.Raise(unit);
            OnUpdateQueue?.Invoke(BattleManager.instance.CurrentUnit);
        }

        public void Clear() {
            list.Clear();
            OnUpdateQueue?.Invoke(BattleManager.instance.CurrentUnit);
        }

        public void Sort() {
            list.Sort(CompareUnits);
            OnUpdateQueue?.Invoke(BattleManager.instance.CurrentUnit);
        }

        private int CompareUnits(BattleUnit first, BattleUnit second) {
            if (first.actionPoints == second.actionPoints)
                return second.CurSpeed - first.CurSpeed;
            return first.actionPoints - second.actionPoints;
        }

        public IEnumerator<BattleUnit> GetEnumerator() {
            return ((IEnumerable<BattleUnit>)list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)list).GetEnumerator();
        }

        public BattleUnit[] ToArray() {
            return list.ToArray();
        }
    }
}
