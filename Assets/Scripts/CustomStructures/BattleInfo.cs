using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    public struct BattleInfo {
        public List<Enemy> enemies;
        public Sprite BGImage;
        public AudioClip BGM;
        public BattleInfo(Enemy[] enemies, Sprite BG, AudioClip BGM) {
            this.enemies = new List<Enemy>(enemies);
            this.BGImage = BG;
            this.BGM = BGM;
        }
        public void CopyValues(BattleInfo other) {
            enemies = new List<Enemy>(other.enemies);
            BGImage = other.BGImage;
            BGM = other.BGM;
        }
        public void Clear() {
            enemies?.Clear();
            BGImage = null;
            BGM = null;
        }
    }
}