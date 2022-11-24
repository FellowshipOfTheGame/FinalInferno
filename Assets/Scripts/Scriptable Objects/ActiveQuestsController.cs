using System.Collections.Generic;

namespace FinalInferno {
    public class ActiveQuestsController {
        private HashSet<Quest> activeQuests = new HashSet<Quest>();
        private HashSet<Quest> allQuests = new HashSet<Quest>();
        private Dictionary<string, Quest> questIdDictionary = new Dictionary<string, Quest>();

        public ActiveQuestsController(Quest[] quests) {
            allQuests.Clear();
            activeQuests.Clear();
            foreach (Quest quest in quests) {
                allQuests.Add(quest);
                questIdDictionary.Add(quest.SerializedID, quest);
                if (quest.IsActive)
                    activeQuests.Add(quest);
            }
        }

        public void ResetProgress() {
            activeQuests.Clear();
            foreach (Quest quest in allQuests) {
                quest.ResetQuest();
                if (quest.IsActive)
                    activeQuests.Add(quest);
            }
        }

        public void StartQuest(Quest newQuest) {
            activeQuests.Add(newQuest);
            newQuest.StartQuest();
        }

        public void CompleteQuest(Quest doneQuest) {
            doneQuest.CompleteQuest();
            if (!doneQuest.IsActive)
                activeQuests.Remove(doneQuest);
        }

        public QuestInfo[] GetActiveQuestInfo() {
            QuestInfo[] result = new QuestInfo[activeQuests.Count];
            int index = 0;
            foreach (Quest quest in activeQuests) {
                QuestInfo questInfo = new QuestInfo();
                questInfo.SaveQuestInfo(quest);
                result[index++] = questInfo;
            }
            return result;
        }

        public void LoadQuestProgress(QuestInfo[] questsInfo) {
            foreach (QuestInfo questInfo in questsInfo) {
                Quest quest = questIdDictionary[questInfo.name];
                quest.StartQuest();
                questInfo.LoadQuestInfo(quest);
                activeQuests.Add(quest);
            }
        }
    }
}
