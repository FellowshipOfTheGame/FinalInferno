using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.Battle.LifeMenu {
    public class EnemiesLives : UnitsLives {
        [SerializeField] private GameObject EnemyLife;

        [SerializeField] private RectTransform livesContent;

        private void Start() {
            team = UnitType.Enemy;
            units = BattleManager.instance.GetTeam(team);

            LoadEnemies();
        }

        private void LoadEnemies() {
            foreach (UnitLife life in livesContent.GetComponentsInChildren<UnitLife>()) {
                Destroy(life.gameObject);
            }

            lives = new List<UnitLife>();
            foreach (BattleUnit unit in units) {
                UnitLife newLife = Instantiate(EnemyLife, livesContent).GetComponent<UnitLife>();
                newLife.manager = this;
                newLife.thisUnit = unit;
                newLife.AddUpdateToEvent();

                lives.Add(newLife);
            }
            UpdateLives();
        }

    }

}