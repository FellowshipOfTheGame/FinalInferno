using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "NewStoryQuest", menuName = "ScriptableObject/Story Quest")]
    public class StoryQuest : Quest {
        [SerializeField] private StoryQuest previousQuest;
        public StoryQuest PreviousQuest => previousQuest;
        [SerializeField] private StoryQuest nextQuest;
        public StoryQuest NextQuest => nextQuest;

        public override void ResetQuest() {
            if (previousQuest == null)
                StartQuest();
            else
                base.ResetQuest();
        }

        public override void SetFlag(string eventName, bool value) {
            base.SetFlag(eventName, value);
            if (!nextQuest || nextQuest.IsActive)
                return;
            if (IsComplete)
                Party.Instance.StartQuest(nextQuest);
        }

        public override void CompleteQuest() {
            base.CompleteQuest();
            if (!nextQuest)
                return;
            Party.Instance.StartQuest(nextQuest);
        }
    }
}
