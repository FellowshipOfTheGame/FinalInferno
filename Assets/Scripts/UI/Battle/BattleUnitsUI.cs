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
        [Header("Contents")]
        [SerializeField] private Transform heroesContent;
        [SerializeField] private Transform enemiesContent;

        [Header("AII Managers")]
        [SerializeField] private AIIManager heroesManager;
        [SerializeField] private AIIManager enemiesManager;

        [Header("Prefab")]
        [SerializeField] private GameObject unitPrefab;

        void Start()
        {
            LoadTeam(UnitType.Hero, heroesContent, heroesManager);
            LoadTeam(UnitType.Enemy, enemiesContent, enemiesManager);
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
                newUnit.GetComponent<UnitItem>().unit = unit;

                newUnit.GetComponent<Image>().color = unit.unit.color;

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