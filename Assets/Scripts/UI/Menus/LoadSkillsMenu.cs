using System.Collections.Generic;
using System.Linq;
using FinalInferno.UI.AII;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.SkillsMenu {
    public class LoadSkillsMenu : MonoBehaviour {
        [SerializeField] private GameObject SkillItem;
        [SerializeField] private List<Image> HeroesImages;
        [SerializeField] private List<AIIManager> SkillsContents;
        [SerializeField] private LoadSkillDescription loader;
        private bool firstSkill = true;

        private void Start() {
            LoadContents();
        }

        public void LoadContents() {
            firstSkill = true;
            for (int heroIndex = 0; heroIndex < Party.Instance.characters.Count; heroIndex++) {
                Hero hero = Party.Instance.characters[heroIndex].archetype;
                HeroesImages[heroIndex].sprite = hero.Portrait;
                ClearExistingItems(heroIndex);
                AIIManager manager = SkillsContents[heroIndex];
                InstantiateHeroSkillItems(heroIndex, manager);
                FixAnchoring(manager);
            }
        }

        private void ClearExistingItems(int heroIndex) {
            foreach (SkillsMenuSkillItem item in SkillsContents[heroIndex].transform.GetComponentsInChildren<SkillsMenuSkillItem>()) {
                Destroy(item.gameObject);
            }
        }

        private void InstantiateHeroSkillItems(int heroIndex, AIIManager manager) {
            Hero hero = Party.Instance.characters[heroIndex].archetype;
            AxisInteractableItem lastItem = null;
            foreach (PlayerSkill skill in hero.skills.Cast<PlayerSkill>()) {
                GameObject newSkill = Instantiate(SkillItem, SkillsContents[heroIndex].transform);
                newSkill.GetComponent<SkillsMenuSkillItem>().LoadSkill(skill, loader, SkillsContents[heroIndex].GetComponent<HeroSkillsContent>(), heroIndex);
                LoadInfoIfFirstSkill(heroIndex, skill);
                lastItem = UpdateManagerOrder(manager, lastItem, newSkill);
            }
        }

        private void LoadInfoIfFirstSkill(int heroIndex, PlayerSkill skill) {
            if (!firstSkill)
                return;
            firstSkill = false;
            loader.LoadSkillInfo(skill, heroIndex);
        }

        private static AxisInteractableItem UpdateManagerOrder(AIIManager manager, AxisInteractableItem lastItem, GameObject newSkill) {
            AxisInteractableItem newItem = newSkill.GetComponent<AxisInteractableItem>();
            if (lastItem == null) {
                manager.firstItem = newItem;
            } else {
                lastItem.downItem = newItem;
                newItem.upItem = lastItem;
            }
            lastItem = newItem;
            return lastItem;
        }

        private static void FixAnchoring(AIIManager manager) {
            Vector3 curPos = (manager.transform as RectTransform).localPosition;
            (manager.transform as RectTransform).anchorMax = new Vector2(1, 1);
            (manager.transform as RectTransform).anchorMin = new Vector2(0, 1);
            (manager.transform as RectTransform).pivot = new Vector2(0.5f, 1f);
            (manager.transform as RectTransform).localPosition = new Vector3(curPos.x, 0f, curPos.z);
        }
    }
}