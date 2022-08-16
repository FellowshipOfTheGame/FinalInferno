﻿using FinalInferno.UI.Battle;
using FinalInferno.UI.FSM;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.AII {
    public class UnitItem : MonoBehaviour {
        public BattleUnit BattleUnit { get; private set; }
        [SerializeField] private RectTransform unitReference;
        [SerializeField] private AxisInteractableItem item;
        [SerializeField] private LayoutElement layout;
        private RectTransform rectTransform;
        [SerializeField] private float stepSize = 0.5f;
        private bool showingTarget = false;
        private int ppu;

        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
            item.OnAct += SetTarget;
        }

        private void SetTarget() {
            BattleSkillManager.CurrentTargets.Clear();
            BattleSkillManager.CurrentTargets.Add(BattleUnit);
        }

        public void SetBattleUnit(BattleUnit observedBattleUnit, int currentPPU = 64) {
            ppu = currentPPU;
            BattleUnit = observedBattleUnit;
            observedBattleUnit.OnSetupFinished.AddListener(Setup);
            observedBattleUnit.OnSizeChanged.AddListener(UpdateBattleUnitSize);
        }

        public void UpdateBattleUnitSize() {
            layout.preferredWidth = BattleUnit.Unit.BoundsSizeX * ppu;
            layout.preferredHeight = BattleUnit.Unit.BoundsSizeY * ppu;
        }

        public void Setup() {
            unitReference = BattleUnit.Reference;
            item.ActiveReference = unitReference.GetComponent<Image>();
            SetUnitTurnCallbacks();
            SetUnitSelectionCallbacks();
            SetUnitPosition();
            if (BattleUnit.Unit is ICompositeUnit)
                SetupCompositeUnit(BattleUnit.Unit as ICompositeUnit);
        }

        private void SetUnitTurnCallbacks() {
            BattleUnit.OnTurnStart.AddListener(StepForward);
            BattleUnit.OnTurnEnd.AddListener(StepBack);
        }

        public void StepForward(BattleUnit battleUnit) {
            if (battleUnit != BattleUnit)
                return;
            float xOffset = BattleUnit.Unit.IsHero ? stepSize : -stepSize;
            Vector3 newPosition = BattleUnit.transform.position;
            newPosition.x += xOffset;
            UpdateUnitPosition(newPosition);
        }

        private void UpdateUnitPosition(Vector3 newPosition) {
            if ((newPosition - BattleUnit.transform.position).magnitude > float.Epsilon)
                BattleUnit.transform.position = newPosition;
        }

        public void StepBack(BattleUnit battleUnit) {
            if (battleUnit != BattleUnit)
                return;
            float xOffset = BattleUnit.Unit.IsHero ? -stepSize : stepSize;
            Vector3 newPosition = BattleUnit.transform.position;
            newPosition.x += xOffset;
            UpdateUnitPosition(newPosition);
        }

        private void SetUnitSelectionCallbacks() {
            BattleUnit.OnUnitSelected.AddListener(ShowThisAsATarget);
            BattleUnit.OnUnitDeselected.AddListener(StopShowingThisAsATarget);
        }

        public void ShowThisAsATarget() {
            item.EnableReference();
            showingTarget = true;
        }

        public void StopShowingThisAsATarget() {
            item.DisableReference();
            showingTarget = false;
        }

        private void SetUnitPosition() {
            if (BattleUnit.gameObject == gameObject || rectTransform == null)
                return;
            Vector3 newPosition = rectTransform.localToWorldMatrix.MultiplyPoint3x4(Vector3.zero);
            BattleUnit.transform.position = newPosition;
        }

        private void SetupCompositeUnit(ICompositeUnit compositeUnit) {
            CompositeUnitInfo compositeUnitInfo = compositeUnit.GetCompositeUnitInfo(BattleUnit);
            if (compositeUnitInfo.mainUnit == null)
                return;
            MoveAllUnitsToMainUnitPosition(compositeUnit, compositeUnitInfo);
            AddTurnCallbackToOtherUnits(compositeUnitInfo);
        }

        private void MoveAllUnitsToMainUnitPosition(ICompositeUnit compositeUnit, CompositeUnitInfo compositeUnitInfo) {
            if (compositeUnit.IsMainUnit(BattleUnit)) {
                foreach (BattleUnit appendage in compositeUnitInfo.appendages) {
                    appendage.transform.position = BattleUnit.transform.position;
                }
            } else {
                BattleUnit.transform.position = compositeUnitInfo.mainUnit.transform.position;
            }
        }

        private void AddTurnCallbackToOtherUnits(CompositeUnitInfo compositeUnitInfo) {
            foreach (BattleUnit unit in compositeUnitInfo) {
                if (unit == BattleUnit)
                    continue;
                UnitItem otherItem = BattleUnitsUI.Instance.GetUnitItem(unit);
                BattleUnit.OnTurnStart.AddListener(_ => otherItem.StepForward(unit));
                BattleUnit.OnTurnEnd.AddListener(_ => otherItem.StepBack(unit));
            }
        }

        public void ToggleShowTarget() {
            if (showingTarget) {
                StopShowingThisAsATarget();
            } else {
                ShowThisAsATarget();
            }
        }
    }

}
