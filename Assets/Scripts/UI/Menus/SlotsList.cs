using System.Text;
using FinalInferno.UI.AII;
using FinalInferno.UI.FSM;
using TMPro;
using UnityEngine;

namespace FinalInferno.UI.Saves {
    public class SlotsList : MonoBehaviour {
        private const string redColorString = "<color=#840000>";
        private const string greenColorString = "<color=#006400>";
        [SerializeField] private Slot[] saveSlots;

        [Header("Content reference")]
        [SerializeField] private RectTransform slotsContent;

        [Header("Manager")]
        [SerializeField] private AIIManager manager;

        [Header("Click decision")]
        [SerializeField] private ButtonClickDecision clickDecision;
        [SerializeField] private LoadPreviews loadPreviewsActions;

        [Header("Autosave info")]
        [SerializeField] private TextMeshProUGUI textMesh;
        private bool lastAutoSaveStatus;
        private int lastSlotNumber;
        private bool ShouldUpdateAutoSaveString => SaveLoader.AutoSave != lastAutoSaveStatus || SaveLoader.SaveSlot != lastSlotNumber;

        private void Update() {
            if (textMesh == null || !ShouldUpdateAutoSaveString)
                return;
            StringBuilder stringBuilder = new StringBuilder($"{redColorString}Autosave is </color>");
            stringBuilder.Append(SaveLoader.AutoSave ? $"{greenColorString}on</color>" : $"{redColorString}off</color>");
            if (SaveLoader.AutoSave) {
                stringBuilder.Append($"{redColorString}, curent slot is {SaveLoader.SaveSlot + 1}</color>");
            }
            textMesh.text = stringBuilder.ToString();
            lastAutoSaveStatus = SaveLoader.AutoSave;
            lastSlotNumber = SaveLoader.SaveSlot;
        }

        private void Awake() {
            loadPreviewsActions.list = this;
            lastAutoSaveStatus = !SaveLoader.AutoSave;
            lastSlotNumber = -1;
        }

        public void UpdateSlotsContent(SavePreviewInfo[] slots) {
            int maxLength = Mathf.Min(SaveFile.NSaveSlots, saveSlots.Length, slots.Length);
            for (int i = 0; i < maxLength; i++) {
                saveSlots[i].LoadSlot(slots[i], i);
                saveSlots[i].transform.SetParent(slotsContent);
            }
        }
    }
}