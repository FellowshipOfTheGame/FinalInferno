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
        [SerializeField] private float xpPerSecond = 100f;
        [SerializeField] private float minDuration = 1f, maxDuration = 5f;
        private float _duration, _time;

        public void SetPartyStatus()
        {
            partyLevelSlider.maxValue = BattleProgress.startingExp + BattleProgress.xpToNextLevel;
            partyLevelSlider.value = BattleProgress.startingExp;
            currentXPImage.fillAmount = BattleProgress.startingExp / (float)(BattleProgress.startingExp + BattleProgress.xpToNextLevel);
            startingLevelText.text = startingLevel.ToString();
            nextLevelText.text = (startingLevel + 1).ToString();

        }

        public void StartAnimation() 
        {
            changes = new BattleChanges(Party.Instance);
            float idealDuration = changes.xpGained / xpPerSecond;
            _duration = Mathf.Clamp(idealDuration, minDuration, maxDuration);
            _time = 0f;
            StartCoroutine(PartyLevel());
        }

        private IEnumerator PartyLevel()
        {
            while (_time <= _duration)
            {
                partyLevelSlider.value += Time.fixedDeltaTime * changes.xpGained / _duration;
                if (partyLevelSlider.value >= partyLevelSlider.maxValue)
                    LevelUp();
                _time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();   
            }
            partyLevelSlider.value = (BattleProgress.startingExp + changes.xpGained) % (BattleProgress.startingExp + BattleProgress.xpToNextLevel);
        }

        private void LevelUp()
        {
            startingLevelText.text = nextLevelText.text;
            nextLevelText.text = (int.Parse(nextLevelText.text)+1).ToString();
            partyLevelSlider.value = 0f;
            partyLevelSlider.maxValue = Party.Instance.xpNext;
        }
    }
}