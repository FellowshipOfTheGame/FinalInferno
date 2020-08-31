using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FinalInferno.UI.AII;
using FinalInferno.UI.FSM;

namespace FinalInferno.UI.Saves
{
    public class Slot : MonoBehaviour
    {
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

        void Awake()
        {
            Item.OnEnter += SetSlotTypeValue;
        }

        public void LoadSlot(SavePreviewInfo info, int number)
        {
            if (emptySlot = (info.level <= 0))
            {
                EmptySlotGO.SetActive(true);
                PreviewInfoGO.SetActive(false);
            }
            else
            {
                EmptySlotGO.SetActive(false);
                PreviewInfoGO.SetActive(true);
                InfosText.text = "Level " + info.level + "\n" + ParseAreaName(info.mapName);

                for (int i = 0; i < 4; i++)
                    HeroesImages[i].sprite = info.heroes[i].QueueSprite;
            }

            slotNumber = number;
        }

        private string ParseAreaName(string saveName){
            string actualName = saveName.Replace(" ", string.Empty);   
            actualName = actualName.Replace("StartingArea", "PlainsBeyondHell");
            Regex r = new Regex(@"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])");
            actualName = r.Replace(actualName, " ");
            string[] words = actualName.Split(' ');
            for(int i = 0; i < words.Length; i++){
                words[i] = words[i].TrimStart('0');
            }
            actualName = string.Join(" ", words);
            return actualName;
        }

        private void SetSlotTypeValue()
        {
            SaveLoader.SaveSlot = slotNumber;
            decision.UpdateValue(emptySlot);
        }
    }
}