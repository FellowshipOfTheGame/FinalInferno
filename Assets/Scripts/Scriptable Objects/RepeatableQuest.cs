using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "NewRepeatableQuest", menuName = "ScriptableObject/Repeatable Quest")]
    public class RepeatableQuest : Quest {
        public override void CompleteQuest() {
            base.CompleteQuest();
            ResetQuestIfRepeatable();
        }

        private void ResetQuestIfRepeatable() {
            Party.Instance.activeQuests.Remove(this);
            ResetQuest();
        }
    }
}
