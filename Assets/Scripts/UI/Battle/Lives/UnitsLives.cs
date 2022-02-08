using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.Battle.LifeMenu {
    /// <summary>
    /// Classe responsável por gerenciar o menu de vidas.
    /// </summary>
    public class UnitsLives : MonoBehaviour {
        public delegate void LifeUpdate();
        public event LifeUpdate OnUpdate;

        protected List<BattleUnit> units;

        public List<UnitLife> lives;

        public UnitType team;

        private void Start() {
            units = BattleManager.instance.GetTeam(team, true);
            LoadTeam();
        }

        protected void LoadTeam() {
            for (int i = 0; i < units.Count; i++) {
                lives[i].thisUnit = units[i];
            }

            UpdateLives();
        }

        public void UpdateLives() {
            OnUpdate?.Invoke();
        }

    }

}