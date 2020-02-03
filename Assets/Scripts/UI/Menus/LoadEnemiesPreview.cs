using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI
{
    public class LoadEnemiesPreview : MonoBehaviour
    {
        [SerializeField] private GameObject EnemyImage;

        public void LoadPreview()
        {
            // Calcula o level dos inimigos
            // Avalia os parametros das quests
            int questParam = 0;
            if(AssetManager.LoadAsset<Quest>("MainQuest").PartyReference.events["CerberusDead"]) questParam++;

            int enemyLevel = questParam * 10;
            if(Mathf.Clamp(Party.Instance.level - (questParam * 10), 0, 10) > 5)
                enemyLevel += 5;

            CerberusHead.heads = 0;
            foreach (Enemy enemy in ChangeSceneUI.battleEnemies)
            {
                enemy.LevelEnemy(enemyLevel);

                if (enemy is CerberusHead)
                    CerberusHead.heads++;
                    
                GameObject newEnemy = Instantiate(EnemyImage, transform);
                newEnemy.GetComponent<Image>().sprite = enemy.QueueSprite;
            }
            CerberusHead.heads = 0;
        }
    }
}