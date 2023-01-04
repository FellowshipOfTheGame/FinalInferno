namespace FinalInferno {
    [System.Serializable]
    public struct QuestEvent {
        public Quest quest;
        public string eventFlag;
        public bool IsConditionSatisfied => Quest.IsConditionSatisfied(quest, eventFlag);

        public void SetFlag(bool value) {
            if (!quest)
                return;
            quest.SetFlag(eventFlag, value);
        }

        public bool GetFlag() {
            return quest ? quest.GetFlag(eventFlag) : false;
        }
    }

}