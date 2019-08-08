using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FinalInferno.BattleProgress;
using UnityEngine.UI;

namespace FinalInferno.UI.Victory
{
    public class VictoryScreenAnimation : MonoBehaviour
    {
        [Header("Party Level")]
        [SerializeField] private Slider partyLevelSlider;
        [SerializeField] private Image currentXPImage;
        [SerializeField] private Text startingLevelText;
        [SerializeField] private Text nextLevelText;

        private BattleChanges changes;
        [SerializeField] private float minDuration = 1f, maxDuration = 5f;

        public void SetPartyStatus()
        {
            partyLevelSlider.value = BattleProgress.startingExp / (BattleProgress.startingExp + BattleProgress.xpToNextLevel);
            currentXPImage.fillAmount = BattleProgress.startingExp / (BattleProgress.startingExp + BattleProgress.xpToNextLevel);
            startingLevelText.text = startingLevel.ToString();
            nextLevelText.text = (startingLevel + 1).ToString();

        }

        public void StartAnimation() 
        {
            changes = new BattleChanges(Party.Instance);
            StartCoroutine(PartyLevel());
        }

        private IEnumerator PartyLevel()
        {
            
            yield return null;
        }
    }
}