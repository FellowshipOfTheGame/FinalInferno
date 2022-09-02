using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FinalInferno.EventSystem;
using FinalInferno.UI.Battle;
using FinalInferno.UI.FSM;

namespace FinalInferno {
    public class BattleManager : MonoBehaviour {
        public static BattleManager instance = null;
        public BoolDecision isBattleReady;
        [SerializeField] private List<Unit> units;
        public List<Unit> Units => units;
        public List<BattleUnit> battleUnits;
        public BattleQueue queue;
        public BattleUnit CurrentUnit { get; private set; }
        public int MaxBaseSpeed { get; private set; } = Unit.maxStatValue;
        public int MinBaseSpeed { get; private set; } = 0;
        public int CameraPPU { get; private set; } = 64;

        [Header("UI Elements")]
        public BattleUnitsUI unitsUI;
        [SerializeField] private RectTransform heroesLayout;
        [SerializeField] private RectTransform enemiesLayout;
        [Header("Events")]
        [SerializeField] private EventFI updateLivesEvent;
        [Header("Input References")]
        [SerializeField] private InputActionReference debugAction;

        private void Awake() {
            if (instance == null) {
                instance = this;
            } else if (instance != this) {
                Destroy(this);
                return;
            }

            InitVariablesAndBattleProgress();
        }

        private void InitVariablesAndBattleProgress() {
            units = new List<Unit>();
            battleUnits = new List<BattleUnit>();
            BattleProgress.ResetInfo(Party.Instance);
        }

        private void OnDestroy() {
            if (instance == this)
                instance = null;
        }

        public void Update() {
            if (!StaticReferences.DebugBuild || !debugAction.action.triggered)
                return;

            SetAllUnitsTo1HP();
        }

        private void SetAllUnitsTo1HP() {
            foreach (BattleUnit bUnit in battleUnits) {
                if (bUnit.CurHP > 0)
                    bUnit.DecreaseHP(1.0f);
            }
            updateLivesEvent.Raise();
        }

        public void InitUnitsList(List<Enemy> enemyList) {
            Units.Clear();
            foreach (Character character in Party.Instance.characters) {
                Units.Add(character.archetype);
            }
            Units.AddRange(enemyList);
        }

        public void PrepareBattle() {
            CameraPPU = Camera.main.gameObject.GetComponent<UnityEngine.U2D.PixelPerfectCamera>().assetsPPU;
            MaxBaseSpeed = 0;
            MinBaseSpeed = Unit.maxStatValue;

            foreach (Unit unit in Units) {
                UpdateSpeedLimits(unit);
                SaveProgressIfHeroUnit(unit);
                BattleUnit newUnit = BattleUnitsUI.Instance.LoadUnit(unit, CameraPPU);
                battleUnits.Add(newUnit);
                SetupIfEnemyUnit(unit, newUnit);
            }
            UpdateUIElements();
            foreach (BattleUnit battleUnit in battleUnits) {
                InsertNewUnitInQueue(battleUnit);
            }

            isBattleReady.UpdateValue(true);
        }

        private void UpdateSpeedLimits(Unit unit) {
            MaxBaseSpeed = Mathf.Min(Mathf.Max(unit.baseSpeed, MaxBaseSpeed), Unit.maxStatValue);
            MinBaseSpeed = Mathf.Max(Mathf.Min(unit.baseSpeed, MinBaseSpeed), 0);
        }

        private static void SaveProgressIfHeroUnit(Unit unit) {
            if (unit is Hero)
                BattleProgress.AddHeroSkills((Hero)unit);
        }

        private static void SetupIfEnemyUnit(Unit unit, BattleUnit newUnit) {
            if (!(unit is Enemy))
                return;
            newUnit.ChangeColor();
            (newUnit.Unit as Enemy).ResetParameters();
        }

        private void UpdateUIElements() {
            ForceUpdateLayoutPositions();
            foreach (BattleUnit battleUnit in battleUnits) {
                battleUnit.OnSetupFinished?.Invoke();
            }
        }

        private void ForceUpdateLayoutPositions() {
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(enemiesLayout);
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(heroesLayout);
        }

        private void InsertNewUnitInQueue(BattleUnit battleUnit) {
            float relativeUnitSpeed = (battleUnit.CurSpeed - MinBaseSpeed) / (MaxBaseSpeed - MinBaseSpeed);
            float meanSkillCost = (Skill.maxCost + Skill.baseCost) / 2.0f;
            float initiative = Mathf.Clamp(relativeUnitSpeed, 0f, 1f) * meanSkillCost;
            queue.Enqueue(battleUnit, -Mathf.FloorToInt(initiative));
        }

        public void StartBattle() {
            foreach (BattleUnit unit in queue.ToArray()) {
                unit.OnStartBattle?.Invoke(unit, new List<BattleUnit>(queue.ToArray()));
            }
            UpdateCurrentUnit();
        }

        private void UpdateCurrentUnit() {
            CurrentUnit = queue.Dequeue();
            if (CurrentUnit != null)
                CurrentUnit.OnTurnStart?.Invoke(CurrentUnit);
        }

        public void StartNextTurn() {
            UpdateCurrentUnit();
            if (CurrentUnit == null)
                return;
            CurrentUnit.UpdateAggro();
            CurrentUnit.UpdateStatusEffects();
        }

        public void UpdateQueue(int cost) {
            BattleUnit previousUnit = CurrentUnit;
            if (CurrentUnit != null)
                CurrentUnit.OnTurnEnd?.Invoke(CurrentUnit);
            CurrentUnit = null;
            queue.Enqueue(previousUnit, cost);
            EndTurn();
        }

        public void EndTurn() {
            CurrentUnit = null;
            BattleSkillManager.EndTurn();
        }

        public UnitType GetCurrentUnitType() {
            return CurrentUnit == null ? UnitType.Null : CurrentUnit.Unit.UnitType;
        }

        public void Kill(BattleUnit unit) {
            int nNewEffects = ApplyDeathCallbacks(unit);
            if (unit.CurHP <= 0 && nNewEffects <= 0)
                RemoveDeadUnitReferences(unit);
            updateLivesEvent.Raise();
        }

        private int ApplyDeathCallbacks(BattleUnit unit) {
            int nEffectsBefore = unit.effects.Count;
            if (unit.OnDeath != null) {
                unit.OnDeath(unit, new List<BattleUnit>(battleUnits));
                unit.OnDeath = null;
            }
            int nNewEffects = unit.effects.Count - nEffectsBefore;
            return nNewEffects;
        }

        private void RemoveDeadUnitReferences(BattleUnit unit) {
            queue.Remove(unit);
            unitsUI.RemoveUnit(unit);
            if (CurrentUnit != unit)
                return;
            CurrentUnit.OnTurnEnd?.Invoke(CurrentUnit);
            CurrentUnit = null;
        }

        public void Revive(BattleUnit unit) {
            if (!queue.Contains(unit) && CurrentUnit != unit)
                ReinsertRevivedUnitInQueue(unit);
            updateLivesEvent.Raise();
        }

        private void ReinsertRevivedUnitInQueue(BattleUnit unit) {
            unit.actionPoints = Mathf.FloorToInt(unit.Unit.attackSkill.cost);
            queue.Enqueue(unit, 0);
            unitsUI.ReinsertUnit(unit);
        }

        public VictoryType CheckEnd() {
            List<BattleUnit> team = GetTeam(UnitType.Enemy);
            if (team.Count <= 0)
                return VictoryType.Heroes;

            team = GetTeam(UnitType.Hero);
            return team.Count <= 0 ? VictoryType.Enemies : VictoryType.Nobody;
        }

        public List<BattleUnit> GetTeam(UnitType type, bool countDead = false, bool deadOnly = false) {
            List<BattleUnit> team = new List<BattleUnit>();

            foreach (BattleUnit battleUnit in battleUnits) {
                if (battleUnit.Unit.UnitType != type)
                    continue;

                if (ShouldAddUnitToTeamList(battleUnit, countDead, deadOnly))
                    team.Add(battleUnit);
            }

            return team;
        }

        private static bool ShouldAddUnitToTeamList(BattleUnit battleUnit, bool countDead, bool deadOnly) {
            return !countDead && battleUnit.CurHP > 0 ||
                    countDead && (!deadOnly || (deadOnly && battleUnit.CurHP <= 0));
        }

        public List<BattleUnit> GetTeam(BattleUnit battleUnit, bool countDead = false, bool deadOnly = false) {
            return GetTeam(battleUnit.Unit.UnitType, countDead, deadOnly);
        }

        public List<BattleUnit> GetEnemies(BattleUnit battleUnit, bool countDead = false, bool deadOnly = false) {
            return GetTeam(battleUnit.Unit is Hero ? UnitType.Enemy : UnitType.Hero, countDead, deadOnly);
        }

        public BattleUnit GetBattleUnit(Unit unit) {
            foreach (BattleUnit bUnit in battleUnits) {
                if (bUnit.Unit == unit)
                    return bUnit;
            }
            return null;
        }
    }
}
