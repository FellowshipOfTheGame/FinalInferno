namespace FinalInferno {
    [System.Serializable]
    public struct DialogueEntry {
        public Quest quest;
        public string eventFlag;
        public Fog.Dialogue.Dialogue dialogue;
        public DialogueEntry(Quest _quest, string _eventFlag, Fog.Dialogue.Dialogue _dialogue) {
            quest = _quest;
            eventFlag = _eventFlag;
            dialogue = _dialogue;
        }
    }

}