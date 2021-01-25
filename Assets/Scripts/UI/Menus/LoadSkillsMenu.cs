using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.SkillsMenu
{
    public class LoadSkillsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject SkillItem;
        [SerializeField] private List<Image> HeroesImages;
        [SerializeField] private List<AIIManager> SkillsContents;
        [SerializeField] private LoadSkillDescription loader;

        private void Start()
        {
            LoadContents();
        }

        public void LoadContents()
        {
            bool firstSkill = true;
            for (int i = 0; i < Party.Instance.characters.Count; i++)
            {
                Hero hero = Party.Instance.characters[i].archetype;
                HeroesImages[i].sprite = hero.Portrait;

                foreach (SkillsMenuSkillItem item in SkillsContents[i].transform.GetComponentsInChildren<SkillsMenuSkillItem>())
                {
                    Destroy(item.gameObject);
                }

                AIIManager manager = SkillsContents[i];
                AxisInteractableItem lastItem = null;

                foreach (PlayerSkill skill in hero.skills)
                {
                    GameObject newSkill = Instantiate(SkillItem, SkillsContents[i].transform);
                    newSkill.GetComponent<SkillsMenuSkillItem>().LoadSkill(skill, loader, SkillsContents[i].GetComponent<HeroSkillsContent>(), i);
                    if(firstSkill){
                        firstSkill = false;
                        loader.LoadSkillInfo(skill, i);
                    }

                    AxisInteractableItem newItem = newSkill.GetComponent<AxisInteractableItem>();
                    if (lastItem == null)
                    {
                        manager.firstItem = newItem;
                    }
                    else
                    {
                        lastItem.downItem = newItem;
                        newItem.upItem = lastItem;
                    }
                    lastItem = newItem;
                }

                // Por algum motivo a ancoragem ficava errada, e precisa ser consertada aqui
                Vector3 curPos = (manager.transform as RectTransform).localPosition;
                (manager.transform as RectTransform).anchorMax = new Vector2(1, 1);
                (manager.transform as RectTransform).anchorMin = new Vector2(0, 1);
                (manager.transform as RectTransform).pivot = new Vector2(0.5f, 1f);
                (manager.transform as RectTransform).localPosition = new Vector3(curPos.x, 0f, curPos.z);
            }
        }
    }
}