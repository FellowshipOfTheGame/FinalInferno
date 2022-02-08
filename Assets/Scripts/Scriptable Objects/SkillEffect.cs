using UnityEngine;

namespace FinalInferno {
    //engloba cada efeitos que uma "skill" pode causar]
    public abstract class SkillEffect : ScriptableObject {

        [SerializeField] private string displayName;
        public string DisplayName => displayName;
        [SerializeField] private Sprite icon;
        public Sprite Icon => icon;
        public abstract string Description { get; }
        public float value1, value2;

        public abstract void Apply(BattleUnit source, BattleUnit target);
    }
}