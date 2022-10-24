using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "NewStoryQuest", menuName = "ScriptableObject/Story Quest")]
    public class StoryQuest : Quest {
        [SerializeField] private StoryQuest previousQuest;
        [SerializeField] private StoryQuest nextQuest;

        public override void ResetQuest() {
            if (previousQuest == null)
                StartQuest();
            else
                base.ResetQuest();
        }
    }
}
