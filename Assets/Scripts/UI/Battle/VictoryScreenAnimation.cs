using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FinalInferno.BattleProgress;
using UnityEngine.UI;

namespace FinalInferno.UI.Victory
{
    public class VictoryScreenAnimation : MonoBehaviour
    {
        private BattleChanges changes;

        [Header("Animation")]
        [SerializeField] private float xpPerSecond = 100f;
        [SerializeField] private float minDuration = 1f, maxDuration = 5f;
        [SerializeField] private float levelUpTime = .5f;
        private float _duration, _time;

        [Header("Party Level")]
        [SerializeField] private Slider partyLevelSlider;
        [SerializeField] private Image currentXPImage;
        [SerializeField] private Text startingLevelText;
        [SerializeField] private Text nextLevelText;

        [Header("Heroes Skills")]
        [SerializeField] private List<Transform> skillsContents;
        [SerializeField] private List<Image> heroesImages;
        [SerializeField] private float timeBetweenHeroesShown = .5f;
        [SerializeField] private float timeBetweenSkillsShown = .4f;
        [SerializeField] private RuntimeAnimatorController HeroImageAnimator;
        [SerializeField] private GameObject UpdatedSkill;


        public void SetPartyStatus()
        {
            partyLevelSlider.maxValue = /*BattleProgress.startingExp +*/ BattleProgress.xpToNextLevel;
            partyLevelSlider.value = BattleProgress.startingExp;
            currentXPImage.fillAmount = BattleProgress.startingExp / (float)(/*BattleProgress.startingExp +*/ BattleProgress.xpToNextLevel);
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
            StartCoroutine(HeroesSkills());
        }

        private IEnumerator PartyLevel()
        {
            while (_time <= _duration)
            {
                partyLevelSlider.value += Time.fixedDeltaTime * changes.xpGained / _duration;
                if (partyLevelSlider.value >= partyLevelSlider.maxValue)
                {
                    LevelUp();
                    yield return new WaitForSeconds(levelUpTime);
                }
                _time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();   
            }
            partyLevelSlider.value = (BattleProgress.startingExp + changes.xpGained) % (/*BattleProgress.startingExp*/ + BattleProgress.xpToNextLevel);
        }

        private void LevelUp()
        {
            startingLevelText.text = nextLevelText.text;
            nextLevelText.text = (int.Parse(nextLevelText.text)+1).ToString();
            currentXPImage.fillAmount = 0f;
            partyLevelSlider.value = 0f;
            partyLevelSlider.maxValue = Party.Instance.xpNext/* - partyLevelSlider.maxValue*/;
        }

        private IEnumerator HeroesSkills()
        {
            for (int i = 0; i < Party.Capacity; i++)
            {
                StartCoroutine(HeroSkill(i));
                yield return new WaitForSeconds(timeBetweenHeroesShown);
            }
        }

        private IEnumerator HeroSkill(int heroIndex)
        {
            heroesImages[heroIndex].sprite = changes.heroes[heroIndex].portrait;
            heroesImages[heroIndex].GetComponent<Animator>().runtimeAnimatorController = HeroImageAnimator;

            for (int i = 0; i < changes.skillReferences[heroIndex].Count; i++)
            {
                GameObject newSkill = Instantiate(UpdatedSkill, skillsContents[heroIndex]);
                newSkill.GetComponent<UpdatedSkill>().LoadUpdatedSkill(changes.skillReferences[heroIndex][i]);
                yield return new WaitForSeconds(timeBetweenSkillsShown);
            }
        }
    }
}