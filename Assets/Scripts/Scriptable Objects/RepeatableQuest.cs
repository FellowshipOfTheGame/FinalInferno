using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "NewRepeatableQuest", menuName = "ScriptableObject/Repeatable Quest")]
    public class RepeatableQuest : Quest {
        public override void CompleteQuest() {
            base.CompleteQuest();
            if (IsComplete)
                ResetQuest();
        }
    }
}
