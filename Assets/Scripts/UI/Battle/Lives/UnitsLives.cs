using System.Collections.Generic;
using UnityEngine;
using FinalInferno.EventSystem;

namespace FinalInferno.UI.Battle.LifeMenu {
    /// <summary>
    /// Classe responsável por gerenciar o menu de vidas.
    /// </summary>
    public class UnitsLives : MonoBehaviour, IEventListenerFI {
        public delegate void LifeUpdate();
        public event LifeUpdate OnUpdate;

        [SerializeField] private EventFI updateLivesEvent;

        protected List<BattleUnit> units;

        public List<UnitLife> lives;

        public UnitType team;
        private bool shouldUpdate = false;

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

        protected void UpdateLives() {
            OnUpdate?.Invoke();
        }

        private void Update() {
            if (!shouldUpdate)
                return;
            UpdateLives();
            shouldUpdate = false;
        }

        private void OnEnable() {
            updateLivesEvent.AddListener(this);
        }

        private void OnDisable() {
            updateLivesEvent.RemoveListener(this);
        }

        public void OnEventRaised() {
            shouldUpdate = true;
        }
    }

}