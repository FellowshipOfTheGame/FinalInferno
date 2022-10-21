namespace FinalInferno {
    [System.Serializable]
    public struct ChangeRule {
        public Quest quest;
        public string eventFlag;
        public string animationFlag;
        public bool newValue;
        public bool IsConditionSatisfied => Quest.IsConditionSatisfied(quest, eventFlag);
        public ChangeRule(Quest _quest, string _eventFlag, string _animationFlag, bool _newValue) {
            quest = _quest;
            eventFlag = _eventFlag;
            animationFlag = _animationFlag;
            newValue = _newValue;
        }
    }
}