using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Battle Info Reference", menuName = "ScriptableObject/Battle Info Reference")]
    public class BattleInfoReference : ScriptableObject {
        public List<Enemy> Enemies => battleInfo.enemies;
        public Sprite BGImage => battleInfo.BGImage;
        public AudioClip BGM => battleInfo.BGM;
        private BattleInfo battleInfo = new BattleInfo();

        public void SetValues(BattleInfo other) {
            battleInfo.CopyValues(other);
        }

        public void SetValues(Enemy[] enemies, Sprite image, AudioClip bgm) {
            battleInfo.enemies = new List<Enemy>(enemies);
            battleInfo.BGImage = image;
            battleInfo.BGM = bgm;
        }

        public void Clear() {
            battleInfo.enemies?.Clear();
            battleInfo.BGImage = null;
            battleInfo.BGM = null;
        }
    }
}