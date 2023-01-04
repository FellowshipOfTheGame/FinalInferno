using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Character", menuName = "ScriptableObject/Character")]
    public class Character : ScriptableObject {
        public Hero archetype;
        public int hpCur;
        public Vector2 position;
        private CharacterOW overworldInstance;
        public CharacterOW OverworldInstance {
            get => overworldInstance ? overworldInstance : null;
            set => overworldInstance = CanChangeInstanceValue(value) ? value : overworldInstance;
        }

        private bool CanChangeInstanceValue(CharacterOW value) {
            return value == null || overworldInstance == null;
        }

        public void SaveOverworldPosition() {
            if (OverworldInstance == null)
                return;
            Vector3 instancePosition = OverworldInstance.transform.position;
            position = new Vector2(instancePosition.x, instancePosition.y);
        }

        public void LoadOverworldPosition() {
            if (OverworldInstance == null)
                return;
            Transform instanceTransform = OverworldInstance.transform;
            instanceTransform.position = new Vector3(position.x, position.y, instanceTransform.position.z);
        }

        public void LevelUp(int newLevel) {
            archetype.LevelUp(newLevel);
            hpCur = archetype.hpMax;
        }

        public void ResetCharacter() {
            position = Vector2.zero;
            archetype.ResetHero();
            hpCur = archetype.hpMax;
        }
    }
}