using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.Battle
{
    /// <summary>
    /// Classe que carrega os herois e inimigos na UI de batalha.
    /// </summary>
    public class BattleUnitsUI : MonoBehaviour
    {
        public static BattleUnitsUI instance;

        [Header("Contents")]
        [SerializeField] private Transform heroesContent;
        [SerializeField] private Transform enemiesContent;

        [Header("AII Managers")]
        [SerializeField] private AIIManager heroesManager;
        [SerializeField] private AIIManager enemiesManager;

        [Header("Prefab")]
        [SerializeField] private GameObject unitPrefab;

        [Header("Selection Indicator")]
        [SerializeField] private Sprite heroIndicator;
        [SerializeField] private Sprite enemyIndicator;


        void Awake(){
            // Singleton
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(this);
        }

        void Start()
        {
            //LoadTeam(UnitType.Hero, heroesContent, heroesManager);
            //LoadTeam(UnitType.Enemy, enemiesContent, enemiesManager);
        }

        public BattleUnit LoadUnit(Unit unit){
            GameObject newUnit = Instantiate(unitPrefab, (unit.GetType() == typeof(Hero))? heroesContent : enemiesContent);
            AIIManager manager = (unit.GetType() == typeof(Hero))? heroesManager : enemiesManager;
            
            BattleUnit battleUnit = newUnit.GetComponent<BattleUnit>();
            battleUnit.battleItem = newUnit.GetComponent<UnitItem>();

            newUnit.transform.rotation = Quaternion.identity;
            battleUnit.Configure(unit);
            // TO DO: Isso não deve ser necessário depois que todas as unidades tiverem o animator e as animações funcionando
            Image[] unitImages = newUnit.GetComponentsInChildren<Image>();
            unitImages[0].sprite = (unit.GetType() == typeof(Hero))? heroIndicator : enemyIndicator;
            unitImages[0].gameObject.SetActive(false);
            unitImages[1].sprite = unit.battleSprite;
            
            // Ordena o item na lista
            AxisInteractableItem newItem = newUnit.GetComponent<AxisInteractableItem>();
            if (manager.lastItem != null)
            {
                newItem.positiveItem = manager.lastItem;
                manager.lastItem.negativeItem = newItem;
            }
            else
            {
                manager.firstItem = newItem;
            }
            manager.lastItem = newItem;

            return battleUnit;
        }

        private void LoadTeam(UnitType team, Transform content, AIIManager manager)
        {
            List<BattleUnit> units = BattleManager.instance.GetTeam(team);

            foreach (AxisInteractableItem item in content.GetComponentsInChildren<AxisInteractableItem>())
            {
                Destroy(item.gameObject);
            }

            // Variável auxiliar para a ordenação dos itens
            AxisInteractableItem lastItem = null;

            // Passa por todas as unidades da lista, adicionando-as no menu e as ordenando
            foreach (BattleUnit unit in units)
            {
                // Instancia um novo item e o coloca no content
                GameObject newUnit = Instantiate(unitPrefab, content);
                newUnit.transform.rotation = Quaternion.identity;

                unit.battleItem = newUnit.GetComponent<UnitItem>();
                unit.battleItem.unit = unit;

                newUnit.GetComponent<Image>().color = unit.unit.color;

                newUnit.GetComponent<Animator>().runtimeAnimatorController = unit.unit.animator;

                newUnit.GetComponent<SpriteRenderer>().sprite = unit.unit.battleSprite;

                // Ordena o item na lista
                AxisInteractableItem newItem = newUnit.GetComponent<AxisInteractableItem>();
                if (lastItem != null)
                {
                    newItem.positiveItem = lastItem;
                    lastItem.negativeItem = newItem;
                }
                else
                {
                    manager.firstItem = newItem;
                }
                lastItem = newItem;
            }
        }

        public void RemoveUnit(BattleUnit unit)
        {
            if (BattleManager.instance.GetUnitType(unit.unit) == UnitType.Hero)  
                RemoveUnitFromContent(unit, heroesContent, heroesManager);
            else
                RemoveUnitFromContent(unit, enemiesContent, enemiesManager);
        }

        private void RemoveUnitFromContent(BattleUnit unit, Transform content, AIIManager manager)
        {
            UnitItem[] units = content.GetComponentsInChildren<UnitItem>();

            for (int i = 0; i < units.Length; i++){
                if (units[i].unit == unit){
                    AxisInteractableItem item = units[i].GetComponent<AxisInteractableItem>();

                    if (item == manager.firstItem)
                        manager.firstItem = item.negativeItem;

                    if (item.negativeItem != null)
                        item.negativeItem.positiveItem = item.positiveItem;

                    if (item.positiveItem != null)
                        item.positiveItem.negativeItem = item.negativeItem;
                }
            }
        }

    }

}