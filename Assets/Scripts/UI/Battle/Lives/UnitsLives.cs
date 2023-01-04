using System.Collections.Generic;
using UnityEngine;
using FinalInferno.EventSystem;

namespace FinalInferno.UI.Battle.LifeMenu {
    public class UnitsLives : MonoBehaviour, IEventListenerFI {
        public delegate void LifeUpdate();
        public event LifeUpdate OnUpdate;
        protected List<BattleUnit> units;
        protected List<UnitLifeImage> lives;
        public UnitType team;
        [SerializeField] private GameObject unitLifePrefab;
        [SerializeField] private RectTransform parentTransform;
        [SerializeField] private EventFI updateLivesEvent;
        private bool shouldUpdate = false;

        private void Start() {
            units = BattleManager.instance.GetTeam(team, true);
            LoadTeam();
        }

        protected void LoadTeam() {
            DestroyExistingUnitLives();
            lives = new List<UnitLifeImage>(units.Count);
            foreach (BattleUnit unit in units) {
                InstantiateNewUnitLife(unit);
            }
            UpdateLives();
        }

        private void DestroyExistingUnitLives() {
            foreach (UnitLifeImage life in parentTransform.GetComponentsInChildren<UnitLifeImage>()) {
                Destroy(life.gameObject);
            }
        }

        private void InstantiateNewUnitLife(BattleUnit unit) {
            UnitLifeImage newLife = Instantiate(unitLifePrefab, parentTransform).GetComponent<UnitLifeImage>();
            newLife.manager = this;
            newLife.thisUnit = unit;
            newLife.AddUpdateToEvent();
            lives.Add(newLife);
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