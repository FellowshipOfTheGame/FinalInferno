using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Battle.LifeMenu
{
    public class EnemiesLives : UnitsLives
    {
        [SerializeField] private GameObject EnemyLife;
        [SerializeField] private GameObject EnemyInfo;

        [SerializeField] private RectTransform livesContent;
        [SerializeField] private RectTransform infosContent;

        void Start()
        {
            team = UnitType.Enemy;
            units = BattleManager.instance.GetTeam(team);
            
            LoadEnemies();
        }

        private void LoadEnemies()
        {
            foreach (UnitLife life in livesContent.GetComponentsInChildren<UnitLife>())
                Destroy(life.gameObject);

            foreach (EnemyInfo info in infosContent.GetComponentsInChildren<EnemyInfo>())
                Destroy(info.gameObject);

            lives = new List<UnitLife>();
            foreach (BattleUnit unit in units)
            {
                UnitLife newLife = Instantiate(EnemyLife, livesContent).GetComponent<UnitLife>();
                newLife.manager = this;
                newLife.thisUnit = unit;
                newLife.AddUpdateToEvent();

                EnemyInfo newInfo = Instantiate(EnemyInfo, infosContent).GetComponent<EnemyInfo>();
                newInfo.manager = this;
                newInfo.thisUnit = unit;
                newInfo.AddUpdateToEvent();
                
                lives.Add(newLife);
            }
            UpdateLives();
        }

    }

}