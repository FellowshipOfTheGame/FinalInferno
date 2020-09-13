using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FinalInferno.BattleProgress;
using UnityEngine.UI;
using FinalInferno.UI.AII;

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
        [SerializeField] private Image previousXPAmountImage;
        [SerializeField] private Text startingLevelText;
        [SerializeField] private Text nextLevelText;

        [Header("Heroes Skills")]
        [SerializeField] private List<Transform> skillsContents;
        [SerializeField] private List<Image> heroesImages;
        [SerializeField] private float timeBetweenHeroesShown = .5f;
        [SerializeField] private float timeBetweenSkillsShown = .4f;
        [SerializeField] private RuntimeAnimatorController HeroImageAnimator;
        [SerializeField] private GameObject UpdatedSkill;
        [SerializeField] private SkillInfoLoader Loader;


        void Awake(){
            foreach(Image img in heroesImages){
                img.color = Color.clear;
            }
        }

        public void SetPartyStatus()
        {
            partyLevelSlider.maxValue = /*BattleProgress.startingExp +*/ BattleProgress.xpToNextLevel;
            partyLevelSlider.value = BattleProgress.startingExp;
            previousXPAmountImage.fillAmount = BattleProgress.startingExp / (float)(/*BattleProgress.startingExp +*/ BattleProgress.xpToNextLevel);
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
                    //yield return new WaitForSeconds(levelUpTime);
                }
                _time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();   
            }
            partyLevelSlider.value = (BattleProgress.startingExp + changes.xpGained) % (/*BattleProgress.startingExp*/ + BattleProgress.xpToNextLevel);
        }

        private void LevelUp()
        {
            // TO DO: Fazer um efeito mais chamativo para indicar que o nível subiu, assim como tem pras skills
            startingLevelText.text = nextLevelText.text;
            nextLevelText.text = (int.Parse(nextLevelText.text)+1).ToString();
            previousXPAmountImage.fillAmount = 0f;
            partyLevelSlider.value -= partyLevelSlider.maxValue;
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
            heroesImages[heroIndex].sprite = changes.heroes[heroIndex].Portrait;
            heroesImages[heroIndex].color = Color.white;
            heroesImages[heroIndex].GetComponent<Animator>().runtimeAnimatorController = HeroImageAnimator;

            AIIManager manager = skillsContents[heroIndex].GetComponent<AIIManager>();
            AxisInteractableItem lastItem = null;

            for (int i = 0; i < changes.skillReferences[heroIndex].Count; i++)
            {
                if(changes.heroSkills[heroIndex][i].level < changes.skillReferences[heroIndex][i].MaxLevel){
                    UpdatedSkill newSkill = Instantiate(UpdatedSkill, skillsContents[heroIndex]).GetComponent<UpdatedSkill>();
                    newSkill.LoadUpdatedSkill(changes.heroSkills[heroIndex][i], changes.skillReferences[heroIndex][i]);

                    newSkill.GetComponent<VictorySkillListItem>().loader = Loader;

                    AxisInteractableItem newItem = newSkill.GetComponent<AxisInteractableItem>();
                    if (lastItem != null)
                    {
                        newItem.upItem = lastItem;
                        lastItem.downItem = newItem;
                    }
                    else
                    {
                        manager.firstItem = newItem;
                    }
                    lastItem = newItem;

                    yield return new WaitForSeconds(timeBetweenSkillsShown);
                    newSkill.StartAnimation();
                }
            }

            for (int i = 0; i < changes.newSkills[heroIndex].Count; i++)
            {
                UpdatedSkill newSkill = Instantiate(UpdatedSkill, skillsContents[heroIndex]).GetComponent<UpdatedSkill>();
                newSkill.LoadNewSkill(changes.newSkills[heroIndex][i]);

                AxisInteractableItem newItem = newSkill.GetComponent<AxisInteractableItem>();
                if (lastItem != null)
                {
                    newItem.upItem = lastItem;
                    lastItem.downItem = newItem;
                }
                else
                {
                    manager.firstItem = newItem;
                }
                lastItem = newItem;

                yield return new WaitForSeconds(timeBetweenSkillsShown);
                newSkill.StartAnimation();
            }
        }
    }
}