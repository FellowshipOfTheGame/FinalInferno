using System.Collections;
using System.Collections.Generic;
using FinalInferno.UI.AII;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Victory {
    public class VictoryScreenAnimation : MonoBehaviour {
        private BattleChanges changes;

        [Header("Animation")]
        [SerializeField] private float xpPerSecond = 100f;
        [SerializeField] private float minDuration = 1f;
        [SerializeField] private float maxDuration = 5f;
        private float _duration;
        private float _time;

        [Header("Party Level")]
        [SerializeField] private Slider partyLevelSlider;
        [SerializeField] private Image previousXPAmountImage;
        [SerializeField] private Text startingLevelText;
        [SerializeField] private Text nextLevelText;

        [Header("Heroes Skills")]
        [SerializeField] private List<Transform> skillsContents;
        [SerializeField] private List<Image> heroesImages;
        [SerializeField] private float timeBetweenHeroesShown = 0.5f;
        [SerializeField] private float timeBetweenSkillsShown = 0.4f;
        [SerializeField] private RuntimeAnimatorController HeroImageAnimator;
        [SerializeField] private GameObject UpdatedSkill;
        [SerializeField] private SkillInfoLoader Loader;

        private void Awake() {
            foreach (Image img in heroesImages) {
                img.color = Color.clear;
            }
        }

        public void SetPartyStatus() {
            partyLevelSlider.maxValue = BattleProgress.startingExpToNextLevel;
            partyLevelSlider.value = BattleProgress.startingExp;
            previousXPAmountImage.fillAmount = BattleProgress.startingExp / (float)BattleProgress.startingExpToNextLevel;
            startingLevelText.text = $"{BattleProgress.startingLevel}";
            nextLevelText.text = $"{BattleProgress.startingLevel + 1}";
        }

        public void StartAnimation() {
            changes = new BattleChanges(Party.Instance);
            float idealDuration = changes.xpGained / xpPerSecond;
            _duration = Mathf.Clamp(idealDuration, minDuration, maxDuration);
            _time = 0f;
            StartCoroutine(PartyLevel());
            StartCoroutine(HeroesSkills());
        }

        private IEnumerator PartyLevel() {
            while (_time <= _duration) {
                partyLevelSlider.value += Time.fixedDeltaTime * changes.xpGained / _duration;
                if (partyLevelSlider.value >= partyLevelSlider.maxValue)
                    LevelUp();
                _time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            partyLevelSlider.value = (BattleProgress.startingExp + changes.xpGained) % BattleProgress.startingExpToNextLevel;
        }

        private void LevelUp() {
            startingLevelText.text = nextLevelText.text;
            nextLevelText.text = $"{int.Parse(nextLevelText.text) + 1}";
            previousXPAmountImage.fillAmount = 0f;
            partyLevelSlider.value -= partyLevelSlider.maxValue;
            partyLevelSlider.maxValue = Party.Instance.xpNextLevel;
        }

        private IEnumerator HeroesSkills() {
            for (int heroIndex = 0; heroIndex < Party.Capacity; heroIndex++) {
                StartCoroutine(HeroSkill(heroIndex));
                yield return new WaitForSeconds(timeBetweenHeroesShown);
            }
        }

        private IEnumerator HeroSkill(int heroIndex) {
            ShowHeroInfo(heroIndex);
            AIIManager manager = skillsContents[heroIndex].GetComponent<AIIManager>();
            AxisInteractableItem lastItem = null;
            #region Skills that gained Exp
            for (int skillIndex = 0; skillIndex < changes.skillReferences[heroIndex].Count; skillIndex++) {
                if (SkillIsMaxedOut(heroIndex, skillIndex))
                    continue;
                UpdatedSkill newSkill = Instantiate(UpdatedSkill, skillsContents[heroIndex]).GetComponent<UpdatedSkill>();
                newSkill.LoadUpdatedSkill(changes.heroSkills[heroIndex][skillIndex], changes.skillReferences[heroIndex][skillIndex]);
                newSkill.VictorySkillListItem.loader = Loader;
                lastItem = AddNewSkillToAIIManagerAsLast(manager, lastItem, newSkill.VictorySkillListItem);
                yield return new WaitForSeconds(timeBetweenSkillsShown);
                newSkill.StartAnimation();
            }
            #endregion
            #region Skills that were unlocked
            for (int i = 0; i < changes.newSkills[heroIndex].Count; i++) {
                UpdatedSkill newSkill = Instantiate(UpdatedSkill, skillsContents[heroIndex]).GetComponent<UpdatedSkill>();
                newSkill.LoadNewSkill(changes.newSkills[heroIndex][i]);
                newSkill.VictorySkillListItem.loader = Loader;
                lastItem = AddNewSkillToAIIManagerAsLast(manager, lastItem, newSkill.VictorySkillListItem);
                yield return new WaitForSeconds(timeBetweenSkillsShown);
                newSkill.StartAnimation();
            }
            #endregion
        }

        private void ShowHeroInfo(int heroIndex) {
            heroesImages[heroIndex].sprite = changes.heroes[heroIndex].Portrait;
            heroesImages[heroIndex].color = Color.white;
            heroesImages[heroIndex].GetComponent<Animator>().runtimeAnimatorController = HeroImageAnimator;
        }

        private bool SkillIsMaxedOut(int heroIndex, int skillIndex) {
            return changes.heroSkills[heroIndex][skillIndex].level >= changes.skillReferences[heroIndex][skillIndex].MaxLevel;
        }

        private static AxisInteractableItem AddNewSkillToAIIManagerAsLast(AIIManager manager, AxisInteractableItem lastItem, AxisInteractableItem newItem) {
            if (lastItem != null) {
                newItem.upItem = lastItem;
                lastItem.downItem = newItem;
            } else {
                manager.firstItem = newItem;
            }
            lastItem = newItem;
            return lastItem;
        }
    }
}