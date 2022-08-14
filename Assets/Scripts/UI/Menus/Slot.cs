using System.Text;
using System.Text.RegularExpressions;
using FinalInferno.UI.AII;
using FinalInferno.UI.FSM;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Saves {
    public class Slot : MonoBehaviour {
        [Header("Slot Type GameObjects")]
        [SerializeField] private GameObject EmptySlotGO;
        [SerializeField] private GameObject PreviewInfoGO;

        [Header("UI Preview Items")]
        [SerializeField] private TMP_Text InfosText;
        [SerializeField] private Image[] HeroesImages;

        [Header("Axis Interactable Item")]
        [SerializeField] private AxisInteractableItem Item;
        [SerializeField] private BoolDecision decision;

        private int slotNumber = -1;
        private bool emptySlot;
        private readonly Regex cleanupRegex = new Regex(@"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])");

        private void Awake() {
            Item.OnEnter += SetSlotType;
            Item.OnAct += SetSlotValue;
        }

        public void LoadSlot(SavePreviewInfo info, int number) {
            emptySlot = info.level <= 0;
            if (emptySlot) {
                EmptySlotGO.SetActive(true);
                PreviewInfoGO.SetActive(false);
            } else {
                EmptySlotGO.SetActive(false);
                PreviewInfoGO.SetActive(true);
                InfosText.text = $"Level {info.level}\n{ParseAreaName(info.mapName)}";
                for (int i = 0; i < 4; i++) {
                    HeroesImages[i].sprite = info.heroes[i].QueueSprite;
                }
            }
            slotNumber = number;
        }

        private string ParseAreaName(string saveName) {
            StringBuilder stringBuilder = new StringBuilder(saveName);
            stringBuilder.Replace(" ", string.Empty);
            stringBuilder.Replace("StartingArea", "PlainsBeyondHell");
            string actualName = cleanupRegex.Replace(stringBuilder.ToString(), " ");
            string[] words = actualName.Split(' ');
            stringBuilder = new StringBuilder(string.Empty, actualName.Length);
            for (int i = 0; i < words.Length; i++) {
                if (i > 0)
                    stringBuilder.Append(" ");
                stringBuilder.Append(words[i].TrimStart('0'));
            }
            return stringBuilder.ToString();
        }

        private void SetSlotValue() {
            SaveLoader.SaveSlot = slotNumber;
        }

        private void SetSlotType() {
            decision.UpdateValue(emptySlot);
        }
    }
}