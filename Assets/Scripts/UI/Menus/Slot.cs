using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        public void LoadSlot(SavePreviewInfo info)
        {
            if (info.level == 0)
            {
                EmptySlotGO.SetActive(true);
            }
            else
            {
                PreviewInfoGO.SetActive(true);
                InfosText.text = "Level " + info.level + "\n" + info.mapName;

                for (int i = 0; i < 4; i++)
                    HeroesImages[i].sprite = info.heroes[i].queueSprite;
            }
        }
    }
}