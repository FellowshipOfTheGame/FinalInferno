using System.Collections.Generic;
using System.Collections;

namespace FinalInferno {
    using UI.Battle.QueueMenu;

    public class BattleQueue : IEnumerable<BattleUnit> {
        private BattleQueueUI queueUI;
        private List<BattleUnit> list;
        public int Count => list.Count;

        public BattleQueue(BattleQueueUI ui) {
            queueUI = ui;
            list = new List<BattleUnit>();
        }

        public void Enqueue(BattleUnit element, int additionalValue) {
            element.actionPoints += additionalValue;
            int newIndex = CalculateNewPosition(element.actionPoints);
            list.Insert(newIndex, element);
            queueUI.UpdateQueue(BattleManager.instance.CurrentUnit);
        }

        private int CalculateNewPosition(int actionPoints) {
            int predictedIndex;
            for (predictedIndex = 0; predictedIndex < list.Count && actionPoints >= list[predictedIndex].actionPoints; predictedIndex++)
                ;
            return predictedIndex;
        }

        public int PreviewPosition(int actionPoints) {
            int predictedIndex = CalculateNewPosition(actionPoints);
            queueUI.StartPreview(predictedIndex);
            return predictedIndex;
        }

        public void StopPreview() {
            queueUI.StopPreview();
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
            queueUI.UpdateQueue(firstUnit);
            return firstUnit;
        }

        public BattleUnit Peek(int position) {
            return list.Count > 0 && position >= 0 && position < list.Count ? list[position] : null;
        }

        public void Remove(BattleUnit unit) {
            list.Remove(unit);
            queueUI.UpdateQueue(BattleManager.instance.CurrentUnit);
        }

        public void Clear() {
            list.Clear();
            queueUI.UpdateQueue(BattleManager.instance.CurrentUnit);
        }

        public void Sort() {
            list.Sort(CompareUnits);
            queueUI.UpdateQueue(BattleManager.instance.CurrentUnit);
        }

        private int CompareUnits(BattleUnit first, BattleUnit second) {
            if (first.actionPoints == second.actionPoints)
                return second.curSpeed - first.curSpeed;
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
