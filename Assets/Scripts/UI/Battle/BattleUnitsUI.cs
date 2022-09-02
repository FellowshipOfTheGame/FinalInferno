using System.Collections.Generic;
using FinalInferno.UI.AII;
using UnityEngine;

namespace FinalInferno.UI.Battle {
    /// <summary>
    /// Classe que carrega os herois e inimigos na UI de batalha.
    /// </summary>
    public class BattleUnitsUI : MonoBehaviour {
        public static BattleUnitsUI Instance { get; private set; }

        [Header("Contents")]
        [SerializeField] private Transform heroesContent;
        [SerializeField] private Transform enemiesContent;

        [Header("AII Managers")]
        [SerializeField] private AIIManager heroesManager;
        [SerializeField] private AIIManager enemiesManager;

        [Header("Prefabs")]
        [SerializeField] private GameObject unitPrefab;
        [SerializeField] private GameObject unitItemPrefab;

        [Header("Selection Indicator")]
        [SerializeField] private Sprite heroIndicator;
        [SerializeField] private Sprite enemyIndicator;

        private Dictionary<BattleUnit, UnitItem> unitItemDict = new Dictionary<BattleUnit, UnitItem>();

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else if (Instance != this) {
                Destroy(this);
            }
        }

        private void OnDestroy() {
            if (Instance == this) {
                Instance = null;
            }
        }

        private void Update() {
            // Rotaciona os UnitItems para compensar a inclinação dos managers
            foreach (Transform child in heroesManager.transform) {
                child.transform.localEulerAngles = new Vector3(child.transform.localEulerAngles.x, child.transform.localEulerAngles.y, heroesManager.transform.localEulerAngles.z * -1f);
            }
            foreach (Transform child in enemiesManager.transform) {
                child.transform.localEulerAngles = new Vector3(child.transform.localEulerAngles.x, child.transform.localEulerAngles.y, enemiesManager.transform.localEulerAngles.z * -1f);
            }
        }

        public UnitItem GetUnitItem(BattleUnit battleUnit) {
            return unitItemDict.ContainsKey(battleUnit) ? unitItemDict[battleUnit] : null;
        }

        public BattleUnit LoadUnit(Unit unit, int ppu = 64) {
            // Instancia os objetos de UI e normais e faz um referenciar o outro
            Transform parentTransform = unit.IsHero ? heroesContent : enemiesContent;
            GameObject newUnit = Instantiate(unitPrefab, null);
            GameObject newUnitItem = Instantiate(unitItemPrefab, parentTransform);
            BattleUnit battleUnit = newUnit.GetComponent<BattleUnit>();
            UnitItem battleItem = newUnitItem.GetComponentInChildren<UnitItem>();
            unitItemDict.Add(battleUnit, battleItem);
            battleItem.SetBattleUnit(battleUnit);

            // Define as configurações de renderização
            int sortingLayer = 0;
            foreach (Transform child in parentTransform) {
                // 1 layer pra unidade, 1 pros status effects e 1 pra skill sendo usada na unidade
                sortingLayer += 3;
            }
            battleUnit.GetComponent<SpriteRenderer>().sortingOrder = sortingLayer;
            battleUnit.Configure(unit);
            battleUnit.OnSizeChanged?.Invoke();

            AxisInteractableItem newItem = battleItem.GetComponent<AxisInteractableItem>();
            AIIManager manager = unit.IsHero ? heroesManager : enemiesManager;

            // Ordena o item na lista
            if (manager.lastItem != null) {
                newItem.upItem = manager.lastItem;
                manager.lastItem.downItem = newItem;
            } else {
                manager.firstItem = newItem;
            }
            manager.lastItem = newItem;

            return battleUnit;
        }

        public void UpdateTargetList() {
            AIIManager manager;
            Unit currentUnit = BattleSkillManager.CurrentUser.Unit;
            Skill currentSkill = BattleSkillManager.CurrentSkill;
            bool useOwnManager = ShouldUseOwnManagerForTargeting(currentSkill);
            manager = ((currentUnit.IsHero && useOwnManager) || (!currentUnit.IsHero && !useOwnManager)) ? heroesManager : enemiesManager;

            // Obtem a lista de possiveis alvos para a skill em questão
            List<BattleUnit> targetUnits = new List<BattleUnit>(BattleSkillManager.CurrentTargets);

            manager.ClearItems();
            foreach (BattleUnit unit in targetUnits) {
                AxisInteractableItem newItem = unitItemDict[unit].GetComponent<AxisInteractableItem>();
                newItem.upItem = null;
                newItem.downItem = null;

                // Ordena o item na lista
                if (manager.lastItem != null) {
                    newItem.upItem = manager.lastItem;
                    manager.lastItem.downItem = newItem;
                } else {
                    manager.firstItem = newItem;
                }
                manager.lastItem = newItem;
            }
        }

        private static bool ShouldUseOwnManagerForTargeting(Skill currentSkill) {
            return currentSkill.target == TargetType.AllAlliesLiveOrDead ||
                   currentSkill.target == TargetType.AllDeadAllies ||
                   currentSkill.target == TargetType.SingleDeadAlly ||
                   currentSkill.target == TargetType.AllLiveAllies ||
                   currentSkill.target == TargetType.Self ||
                   currentSkill.target == TargetType.SingleLiveAlly;
        }

        public void RemoveUnit(BattleUnit unit) {
            if (unit.Unit.IsHero) {
                RemoveUnitFromContent(unit, heroesContent, heroesManager);
            } else {
                RemoveUnitFromContent(unit, enemiesContent, enemiesManager);
            }
        }

        private void RemoveUnitFromContent(BattleUnit unit, Transform content, AIIManager manager) {
            UnitItem[] units = content.GetComponentsInChildren<UnitItem>();

            for (int i = 0; i < units.Length; i++) {
                if (units[i].BattleUnit == unit) {
                    AxisInteractableItem item = units[i].GetComponent<AxisInteractableItem>();

                    if (item == manager.firstItem) {
                        manager.firstItem = item.downItem;
                    }

                    if (item.downItem != null) {
                        item.downItem.upItem = item.upItem;
                    }

                    if (item.upItem != null) {
                        item.upItem.downItem = item.downItem;
                    }

                    if (item == manager.lastItem) {
                        manager.lastItem = item.upItem;
                    }
                }
            }
        }

        public void ReinsertUnit(BattleUnit unit) {
            // Essa função só pode ser chamada se tiver certeza que a unidade foi removida com RemoveUnit
            if (unit.Unit.IsHero) {
                ReinsertUnitInContent(unit, heroesContent, heroesManager);
            } else {
                ReinsertUnitInContent(unit, enemiesContent, enemiesManager);
            }
        }

        private void ReinsertUnitInContent(BattleUnit unit, Transform content, AIIManager manager) {
            UnitItem[] units = content.GetComponentsInChildren<UnitItem>();
            AxisInteractableItem thisItem = null;
            foreach (UnitItem item in units) {
                if (item.BattleUnit == unit) {
                    thisItem = item.GetComponent<AxisInteractableItem>();
                    break;
                }
            }
            AxisInteractableItem previousItem = null;
            AxisInteractableItem nextItem = manager.firstItem;

            while (nextItem != manager.lastItem && (System.Array.IndexOf(units, nextItem.GetComponent<UnitItem>()) < System.Array.IndexOf(units, thisItem.GetComponent<UnitItem>()))) {
                previousItem = nextItem;
                nextItem = nextItem.downItem;
            }

            thisItem.upItem = previousItem;
            if (previousItem == null) {
                manager.firstItem = thisItem;
            } else {
                previousItem.downItem = thisItem;
            }
            thisItem.downItem = nextItem;
            if (nextItem == null) {
                manager.lastItem = thisItem;
            } else {
                thisItem.downItem = nextItem;
            }
        }

    }

}