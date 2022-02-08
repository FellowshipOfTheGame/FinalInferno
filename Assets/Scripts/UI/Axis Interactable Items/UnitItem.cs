using FinalInferno.UI.Battle;
using FinalInferno.UI.FSM;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.AII {
    /// <summary>
	/// Item da lista de efeitos.
	/// </summary>
    public class UnitItem : MonoBehaviour {
        /// <summary>
        /// Referência ao efeito do item.
        /// </summary>
        public BattleUnit unit;
        [SerializeField] private RectTransform unitReference;

        /// <summary>
        /// Referência ao item da lista.
        /// </summary>
        [SerializeField] private AxisInteractableItem item;
        [SerializeField] private HeroInfoLoader infoLoader;

        public LayoutElement layout;
        private RectTransform rectTransform;

        [SerializeField] private float stepSize = 0.5f;
        public Vector2 CurrentOffset { get; private set; }
        [HideInInspector] public Vector2 defaultOffset = Vector2.zero;

        private bool showingTarget = false;

        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
            item.OnAct += SetTarget;
        }

        public void Setup() {
            unitReference = unit.Reference;
            item.ActiveReference = unitReference.GetComponent<UnityEngine.UI.Image>();

            unit.OnTurnStart.AddListener(StepForward);
            unit.OnTurnEnd.AddListener(StepBack);

            if (unit.gameObject != gameObject && rectTransform != null) {
                Vector3 newPosition = rectTransform.localToWorldMatrix.MultiplyPoint3x4(Vector3.zero);
                UpdateUnitPosition(newPosition, true);
                // Não sei o motivo mas as unidades tavam dando um passo pra frente
                // Ou isso ou a posição de mundo calculada ta errada seila
                StepBack(unit);
            }
        }

        public void StepForward(BattleUnit battleUnit) {
            if (battleUnit != unit) {
                return;
            }
            // Debug.Log($"unit {battleUnit} stepped forward");

            float xOffset = (unit.Unit.IsHero) ? stepSize : -stepSize;
            Vector3 newPosition = unit.transform.position;
            newPosition.x += xOffset;
            UpdateUnitPosition(newPosition);
        }

        public void StepBack(BattleUnit battleUnit) {
            if (battleUnit != unit) {
                return;
            }
            // Debug.Log($"unit {battleUnit} stepped back");

            float xOffset = (unit.Unit.IsHero) ? -stepSize : stepSize;
            Vector3 newPosition = unit.transform.position;
            newPosition.x += xOffset;
            UpdateUnitPosition(newPosition);
        }

        private void UpdateUnitPosition(Vector3 newPosition, bool force = false) {
            if (force || (newPosition - unit.transform.position).magnitude > float.Epsilon) {
                unit.transform.position = newPosition;
            }
        }

        private void SetTarget() {
            // Debug.Log("Setting target: " + unit.unit.name);
            BattleSkillManager.currentTargets.Clear();
            BattleSkillManager.currentTargets.Add(unit);
        }

        private void UpdateEnemyContent() {
            BattleManager.instance.enemyContent.ShowEnemyInfo(unit);
        }

        private void UpdateHeroContent() {
            infoLoader.Info.LoadInfo(unit);
        }

        private void ResetEnemyContent() {
            BattleManager.instance.enemyContent.ShowAllLives();
        }

        private void ResetHeroContent() {
            // Feito atraves da maquina de estados, ação GetHeroesLivesTrigger
        }

        public void ShowThisAsATarget() {
            if (showingTarget) {
                item.DisableReference();
            } else {
                item.EnableReference();
            }

            showingTarget = !showingTarget;
        }

    }

}
