using UnityEngine;

namespace FinalInferno.UI.Battle.QueueMenu {
    public class HideConsoleItem : MonoBehaviour {
        private const string hideConsoleAnimString = "HideConsole";
        private const string hideDetailsAnimString = "HideSkillDetails";
        [SerializeField] private AII.AxisInteractableItem item;
        [SerializeField] private Animator consoleAnim;

        private void Awake() {
            item = this.GetComponentIfNull(item);
            if (item)
                item.OnEnter += HideConsole;
        }

        private void HideConsole() {
            if (!consoleAnim)
                return;
            consoleAnim.SetTrigger(hideDetailsAnimString);
            consoleAnim.SetTrigger(hideConsoleAnimString);
        }
    }

}