﻿using System.Collections;
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
            int enemyLevel = int.MinValue;
            foreach (Enemy enemy in ChangeSceneUI.battleEnemies)
            {
                // Esse cálculo supõe que todos os inimigos dever ter o mesmo nível
                // Caso algo complexo como inimigos tendo níveis diferentes seja necessario
                // a condicional precisa ser removida e apenas a versão sem parametro da fução seria chamada
                if(enemyLevel == int.MinValue){
                    enemyLevel = enemy.LevelEnemy();
                }else{
                    enemy.LevelEnemy(enemyLevel);
                }

                if (enemy is CerberusHead)
                    CerberusHead.heads++;
                    
                GameObject newEnemy = Instantiate(EnemyImage, transform);
                newEnemy.GetComponent<Image>().sprite = enemy.QueueSprite;
                newEnemy.GetComponent<Image>().color = enemy.color;
            }
            CerberusHead.heads = 0;
        }
    }
}