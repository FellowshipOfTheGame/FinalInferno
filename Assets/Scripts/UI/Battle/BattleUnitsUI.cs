using System.Collections.Generic;
using FinalInferno.UI.AII;
using FinalInferno.EventSystem;
using UnityEngine;

namespace FinalInferno.UI.Battle {
    public class BattleUnitsUI : MonoBehaviour {
        // 1 layer pra unidade, 1 pros status effects e 1 pra skill sendo usada na unidade
        private const int nLayersPerUnit = 3;
        public static BattleUnitsUI Instance { get; private set; }
        [Header("Contents")]
        [SerializeField] private RectTransform heroesContent;
        [SerializeField] private RectTransform enemiesContent;

        [Header("AII Managers")]
        [SerializeField] private AIIManager heroesManager;
        [SerializeField] private AIIManager enemiesManager;

        [Header("Prefabs")]
        [SerializeField] private GameObject unitPrefab;
        [SerializeField] private GameObject unitItemPrefab;

        [Header("Selection Indicator")]
        [SerializeField] private Sprite heroIndicator;
        [SerializeField] private Sprite enemyIndicator;

        [Header("Events")]
        [SerializeField] private VoidEventListenerFI setupFinishedListener;
        [SerializeField] private GenericEventListenerFI<BattleUnit> unitInstantiatedListener;
        [SerializeField] private GenericEventListenerFI<BattleUnit> unitRemovedListener;
        [SerializeField] private GenericEventListenerFI<BattleUnit> unitReinsertedListener;

        private Dictionary<BattleUnit, UnitItem> unitItemDict = new Dictionary<BattleUnit, UnitItem>();

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else if (Instance != this) {
                Destroy(this);
                return;
            }
            StartListeningEvents();
        }

        private void StartListeningEvents() {
            setupFinishedListener.StartListeningEvent();
            unitInstantiatedListener.StartListeningEvent();
            unitRemovedListener.StartListeningEvent();
            unitReinsertedListener.StartListeningEvent();
        }

        public void ForceUpdateLayoutPositions() {
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(enemiesContent);
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(heroesContent);
        }

        private void OnDestroy() {
            if (Instance != this)
                return;
            Instance = null;
            StopListeningEvents();
        }

        private void StopListeningEvents() {
            setupFinishedListener.StopListeningEvent();
            unitInstantiatedListener.StopListeningEvent();
            unitRemovedListener.StopListeningEvent();
            unitReinsertedListener.StopListeningEvent();
        }

        public UnitItem GetUnitItem(BattleUnit battleUnit) {
            return unitItemDict.ContainsKey(battleUnit) ? unitItemDict[battleUnit] : null;
        }

        public void LoadUnit(BattleUnit battleUnit) {
            Transform parentTransform = battleUnit.Unit.IsHero ? heroesContent : enemiesContent;
            GameObject newObject = InstantiateNewUnitItem(parentTransform);
            UnitItem newUnitItem = newObject.GetComponentInChildren<UnitItem>();
            unitItemDict.Add(battleUnit, newUnitItem);
            newUnitItem.SetBattleUnit(battleUnit);
            AdjustRenderingSortingLayer(battleUnit, parentTransform);
            UpdateNewUnitAIIManager(battleUnit, newUnitItem);
        }

        private GameObject InstantiateNewUnitItem(Transform parentTransform) {
            GameObject instantiatedObject = Instantiate(unitItemPrefab, parentTransform);
            Vector3 eulerAngles = instantiatedObject.transform.localEulerAngles;
            instantiatedObject.transform.localEulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, parentTransform.localEulerAngles.z * -1f);
            return instantiatedObject;
        }

        private static void AdjustRenderingSortingLayer(BattleUnit battleUnit, Transform parentTransform) {
            int sortingLayer = 0;
            foreach (Transform child in parentTransform) {
                sortingLayer += nLayersPerUnit;
            }
            battleUnit.GetComponent<SpriteRenderer>().sortingOrder = sortingLayer;
        }

        private void UpdateNewUnitAIIManager(BattleUnit battleUnit, UnitItem newUnitItem) {
            AxisInteractableItem newItem = newUnitItem.GetComponent<AxisInteractableItem>();
            AIIManager manager = battleUnit.Unit.IsHero ? heroesManager : enemiesManager;
            if (manager.lastItem != null) {
                newItem.upItem = manager.lastItem;
                manager.lastItem.downItem = newItem;
            } else {
                manager.firstItem = newItem;
            }
            manager.lastItem = newItem;
        }

        public void UpdateTargetList() {
            Unit currentUnit = BattleSkillManager.CurrentUser.Unit;
            Skill currentSkill = BattleSkillManager.CurrentSkill;
            List<BattleUnit> targetUnits = new List<BattleUnit>(BattleSkillManager.CurrentTargets);
            AIIManager manager = GetTargetsAIIManager(currentUnit, currentSkill);
            manager.ClearItems();
            foreach (BattleUnit unit in targetUnits) {
                AxisInteractableItem newItem = unitItemDict[unit].GetComponent<AxisInteractableItem>();
                AddItemToAIIManagerAsLast(manager, newItem);
            }
        }

        private AIIManager GetTargetsAIIManager(Unit currentUnit, Skill currentSkill) {
            bool useOwnManager = ShouldUseOwnManagerForTargeting(currentSkill);
            return ((currentUnit.IsHero && useOwnManager) || (!currentUnit.IsHero && !useOwnManager)) ? heroesManager : enemiesManager;
        }

        private static bool ShouldUseOwnManagerForTargeting(Skill currentSkill) {
            return currentSkill.target == TargetType.AllAlliesLiveOrDead ||
                   currentSkill.target == TargetType.AllDeadAllies ||
                   currentSkill.target == TargetType.SingleDeadAlly ||
                   currentSkill.target == TargetType.AllLiveAllies ||
                   currentSkill.target == TargetType.Self ||
                   currentSkill.target == TargetType.SingleLiveAlly;
        }

        private static void AddItemToAIIManagerAsLast(AIIManager manager, AxisInteractableItem newItem) {
            newItem.upItem = null;
            newItem.downItem = null;
            if (manager.lastItem != null) {
                newItem.upItem = manager.lastItem;
                manager.lastItem.downItem = newItem;
            } else {
                manager.firstItem = newItem;
            }
            manager.lastItem = newItem;
        }

        public void RemoveUnit(BattleUnit battleUnit) {
            Transform content = battleUnit.Unit.IsHero ? heroesContent : enemiesContent;
            AIIManager manager = battleUnit.Unit.IsHero ? heroesManager : enemiesManager;
            RemoveUnitFromContent(battleUnit, content, manager);
        }

        private void RemoveUnitFromContent(BattleUnit removedUnit, Transform content, AIIManager manager) {
            UnitItem[] units = content.GetComponentsInChildren<UnitItem>();
            for (int i = 0; i < units.Length; i++) {
                if (units[i].BattleUnit != removedUnit)
                    continue;
                AxisInteractableItem item = units[i].GetComponent<AxisInteractableItem>();
                RemoveItemFromAIIManager(manager, item);
                return;
            }
        }

        private static void RemoveItemFromAIIManager(AIIManager manager, AxisInteractableItem item) {
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

        public void ReinsertUnit(BattleUnit unit) {
            if (unit.Unit.IsHero) {
                ReinsertUnitInContent(unit, heroesContent, heroesManager);
            } else {
                ReinsertUnitInContent(unit, enemiesContent, enemiesManager);
            }
        }

        private void ReinsertUnitInContent(BattleUnit unit, Transform content, AIIManager manager) {
            UnitItem[] units = content.GetComponentsInChildren<UnitItem>();
            foreach (UnitItem item in units) {
                if (item.BattleUnit != unit)
                    continue;
                AxisInteractableItem reinsertedItem = item.GetComponent<AxisInteractableItem>();
                ReinstertItemToAIIManager(manager, units, reinsertedItem);
                return;
            }
        }

        private static void ReinstertItemToAIIManager(AIIManager manager, UnitItem[] units, AxisInteractableItem reinsertedItem) {
            AxisInteractableItem previousItem = null;
            AxisInteractableItem nextItem = manager.firstItem;
            int thisItemIndex = System.Array.IndexOf(units, reinsertedItem.GetComponent<UnitItem>());
            int nextItemIndex = System.Array.IndexOf(units, nextItem.GetComponent<UnitItem>());
            while (nextItem != manager.lastItem && (nextItemIndex < thisItemIndex)) {
                previousItem = nextItem;
                nextItem = nextItem.downItem;
            }
            AdjustReinsertedItemReferences(manager, reinsertedItem, previousItem, nextItem);
        }

        private static void AdjustReinsertedItemReferences(AIIManager manager, AxisInteractableItem reinsertedItem, AxisInteractableItem previousItem, AxisInteractableItem nextItem) {
            reinsertedItem.upItem = previousItem;
            if (previousItem == null) {
                manager.firstItem = reinsertedItem;
            } else {
                previousItem.downItem = reinsertedItem;
            }
            reinsertedItem.downItem = nextItem;
            if (nextItem == null) {
                manager.lastItem = reinsertedItem;
            } else {
                reinsertedItem.downItem = nextItem;
            }
        }
    }
}