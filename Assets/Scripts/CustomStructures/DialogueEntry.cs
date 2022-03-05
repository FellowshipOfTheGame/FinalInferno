﻿using System.Collections.Generic;

namespace FinalInferno {
    [System.Serializable]
    public struct DialogueEntry {
        public Quest quest;
        public string eventFlag;
        public Fog.Dialogue.Dialogue dialogue;
        public bool IsConditionSatisfied => quest?.GetFlag(eventFlag) ?? false;
        public DialogueEntry(Quest _quest, string _eventFlag, Fog.Dialogue.Dialogue _dialogue) {
            quest = _quest;
            eventFlag = _eventFlag;
            dialogue = _dialogue;
        }

        public static Fog.Dialogue.Dialogue GetLastUnlockedDialogue(List<DialogueEntry> dialogues) {
            Fog.Dialogue.Dialogue selectedDialogue = null;
            foreach (DialogueEntry entry in dialogues) {
                if (entry.IsConditionSatisfied) {
                    selectedDialogue = entry.dialogue;
                } else {
                    break;
                }
            }
            return selectedDialogue;
        }
    }

}