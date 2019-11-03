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
            CerberusHead.heads = 0;
            foreach (Enemy enemy in ChangeSceneUI.battleEnemies)
            {
                if (enemy is CerberusHead)
                    CerberusHead.heads++;
                    
                GameObject newEnemy = Instantiate(EnemyImage, transform);
                newEnemy.GetComponent<Image>().sprite = enemy.QueueSprite;
            }
            CerberusHead.heads = 0;
        }
    }
}