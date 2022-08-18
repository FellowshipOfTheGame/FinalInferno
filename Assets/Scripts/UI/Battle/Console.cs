using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Battle {
    public class Console : MonoBehaviour {
        public static Console Instance { get; private set; }
        [SerializeField] private Text ConsoleText;
        private static Unit CurrentUnit => BattleSkillManager.CurrentUser.Unit;

        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnDestroy() {
            if (Instance == this) {
                Instance = null;
            }
        }

        public void UpdateConsole() {
            if (BattleSkillManager.CurrentUser == null)
                return;
            string actionString = GetActionString();
            ConsoleText.text = $"<color=#{GetCurrentUnitColorString()}>{CurrentUnit.name}</color> {actionString}";
        }

        private static string GetActionString() {
            return BattleSkillManager.CurrentSkill switch {
                null => GetReasonCantAct(),
                _ => $"used {BattleSkillManager.CurrentSkill.name}",
            };
        }

        private static string GetReasonCantAct() {
            StatusEffect[] array = BattleSkillManager.CurrentUser.effects.ToArray();
            if (System.Array.Find(array, effect => effect is Confused) != null) {
                return "is confused";
            } else if (System.Array.Find(array, effect => effect is Paralyzed) != null) {
                return "is paralyzed";
            } else if (System.Array.Find(array, effect => effect is VengefulGhost) != null) {
                return $"<color=#{GetCurrentUnitColorString()}>Ghost</color> attacks";
            }
            return "cannot act";
        }

        private static string GetCurrentUnitColorString() {
            return ColorUtility.ToHtmlStringRGBA(CurrentUnit.color);
        }
    }
}