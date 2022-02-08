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
            if (BattleSkillManager.currentUser != null) {
                if (BattleSkillManager.currentSkill == null) {
                    string reasonCantAct = "cannot act";
                    StatusEffect[] array = BattleSkillManager.currentUser.effects.ToArray();
                    if (System.Array.Find<StatusEffect>(array, effect => effect is Confused) != null) {
                        reasonCantAct = "is confused";
                    } else if (System.Array.Find<StatusEffect>(array, effect => effect is Paralyzed) != null) {
                        reasonCantAct = "is paralyzed";
                    } else if (System.Array.Find<StatusEffect>(array, effect => effect is VengefulGhost) != null) {
                        reasonCantAct = "<color=#" + ColorUtility.ToHtmlStringRGBA(BattleSkillManager.currentUser.Unit.color) + ">"
                        + "Ghost</color> attacks";
                    }

                    ConsoleText.text = "<color=#" + ColorUtility.ToHtmlStringRGBA(BattleSkillManager.currentUser.Unit.color) + ">"
                            + BattleSkillManager.currentUser.Unit.name + "</color> " + reasonCantAct;
                } else {
                    ConsoleText.text = "<color=#" + ColorUtility.ToHtmlStringRGBA(BattleSkillManager.currentUser.Unit.color) + ">"
                            + BattleSkillManager.currentUser.Unit.name + "</color> used " + BattleSkillManager.currentSkill.name;
                }
            }
        }
    }
}