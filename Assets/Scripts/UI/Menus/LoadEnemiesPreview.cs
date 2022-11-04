using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI {
    public class LoadEnemiesPreview : MonoBehaviour {
        [SerializeField] private GameObject EnemyImage;
        [SerializeField] private BattleInfoReference battleInfoReference;

        public void LoadPreview() {
            int previousCerberusHeadCount = CerberusHead.heads;
            CerberusHead.heads = 0;
            int enemyLevel = Enemy.CalculateEnemyLevel();
            foreach (Enemy enemy in battleInfoReference.Enemies) {
                enemy.LevelEnemy(enemyLevel);
                if (enemy is CerberusHead)
                    CerberusHead.heads++;
                InstantiateEnemyPortrait(enemy);
            }
            CerberusHead.heads = previousCerberusHeadCount;
        }

        private void InstantiateEnemyPortrait(Enemy enemy) {
            GameObject newEnemy = Instantiate(EnemyImage, transform);
            Image enemyImage = newEnemy.GetComponent<Image>();
            enemyImage.sprite = enemy.QueueSprite;
            enemyImage.color = enemy.color;
        }
    }
}