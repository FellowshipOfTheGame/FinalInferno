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
            GameObject newUnit = Instantiate(unitPrefab, (unit.IsHero)? heroesContent : enemiesContent);
            int sortingLayer = 0;
            foreach(Transform child in ((unit.IsHero)? heroesContent : enemiesContent)){
                sortingLayer++;
                sortingLayer++;
            }
            BattleUnit battleUnit = newUnit.GetComponentInChildren<BattleUnit>();
            battleUnit.GetComponent<SpriteRenderer>().sortingOrder = sortingLayer;
            battleUnit.Configure(unit);
            Sprite unitSprite = battleUnit.GetComponent<SpriteRenderer>().sprite;
            AxisInteractableItem newItem = battleUnit.GetComponent<AxisInteractableItem>();

            AIIManager manager = (unit.IsHero)? heroesManager : enemiesManager;
            
            battleUnit.battleItem = battleUnit.GetComponent<UnitItem>();

            
            battleUnit.battleItem.layout.preferredWidth = unit.BoundsSizeX * 64;
            battleUnit.battleItem.layout.preferredHeight = unit.BoundsSizeY * 64;

            newUnit.transform.rotation = Quaternion.identity;

            // Image[] unitImages = newUnit.GetComponentsInChildren<Image>();
            // unitImages[0].sprite = (unit.GetType() == typeof(Hero))? heroIndicator : enemyIndicator;
            // unitImages[1].sprite = unitSprite;
            
            // Ordena o item na lista
            if (manager.lastItem != null)
            {
                newItem.upItem = manager.lastItem;
                manager.lastItem.downItem = newItem;
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

                unit.Configure(unit.unit);

                // Ordena o item na lista
                AxisInteractableItem newItem = newUnit.GetComponent<AxisInteractableItem>();
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
            }
        }

        public void RemoveUnit(BattleUnit unit)
        {
            if (unit.unit.IsHero)  
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
                        manager.firstItem = item.downItem;

                    if (item.downItem != null)
                        item.downItem.upItem = item.upItem;

                    if (item.upItem != null)
                        item.upItem.downItem = item.downItem;

                    if (item == manager.lastItem)
                        manager.lastItem = item.upItem;
                }
            }
        }

        public void ReinsertUnit(BattleUnit unit){
            // Essa função só pode ser chamada se tiver certeza que a unidade foi removida com RemoveUnit
            if (unit.unit.IsHero)  
                ReinsertUnitInContent(unit, heroesContent, heroesManager);
            else
                ReinsertUnitInContent(unit, enemiesContent, enemiesManager);
        }

        private void ReinsertUnitInContent(BattleUnit unit, Transform content, AIIManager manager){
            UnitItem[] units = content.GetComponentsInChildren<UnitItem>();
            AxisInteractableItem thisItem = null;
            foreach(UnitItem item in units){
                if(item.unit == unit){
                    thisItem = item.GetComponent<AxisInteractableItem>();
                    break;
                }
            }
            AxisInteractableItem previousItem = null;
            AxisInteractableItem nextItem = manager.firstItem;

            while(nextItem != manager.lastItem && (System.Array.IndexOf(units, nextItem.GetComponent<UnitItem>()) < System.Array.IndexOf(units, thisItem.GetComponent<UnitItem>())) ){
                previousItem = nextItem;
                nextItem = nextItem.downItem;
            }

            thisItem.upItem = previousItem;
            if(previousItem == null){
                manager.firstItem = thisItem;
            }else{
                previousItem.downItem = thisItem;
            }
            thisItem.downItem = nextItem;
            if(nextItem == null){
                manager.lastItem = thisItem;
            }else{
                thisItem.downItem = nextItem;
            }
        }

    }

}