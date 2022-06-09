using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Battle {
    public class Console : MonoBehaviour {
        public static Console Instance { get; private set; }
        [SerializeField] private Text ConsoleText;

        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
            }

            Instance = this;
        }

        private void OnDestroy() {
            if (Instance == this) {
                Instance = null;
            }
        }

        public void UpdateConsole() {
            if (BattleSkillManager.CurrentUser != null) {
                if (BattleSkillManager.CurrentSkill == null) {
                    string reasonCantAct = "cannot act";
                    StatusEffect[] array = BattleSkillManager.CurrentUser.effects.ToArray();
                    if (System.Array.Find<StatusEffect>(array, effect => effect is Confused) != null) {
                        reasonCantAct = "is confused";
                    } else if (System.Array.Find<StatusEffect>(array, effect => effect is Paralyzed) != null) {
                        reasonCantAct = "is paralyzed";
                    } else if (System.Array.Find<StatusEffect>(array, effect => effect is VengefulGhost) != null) {
                        reasonCantAct = "<color=#" + ColorUtility.ToHtmlStringRGBA(BattleSkillManager.CurrentUser.Unit.color) + ">"
                        + "Ghost</color> attacks";
                    }

                    ConsoleText.text = "<color=#" + ColorUtility.ToHtmlStringRGBA(BattleSkillManager.CurrentUser.Unit.color) + ">"
                            + BattleSkillManager.CurrentUser.Unit.name + "</color> " + reasonCantAct;
                } else {
                    ConsoleText.text = "<color=#" + ColorUtility.ToHtmlStringRGBA(BattleSkillManager.CurrentUser.Unit.color) + ">"
                            + BattleSkillManager.CurrentUser.Unit.name + "</color> used " + BattleSkillManager.CurrentSkill.name;
                }
            }
        }
    }
}