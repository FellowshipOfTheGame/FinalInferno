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

        [Header("Prefabs")]
        [SerializeField] private GameObject unitPrefab;
        [SerializeField] private GameObject unitItemPrefab;

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

        void Update(){
            // Rotaciona os UnitItems para compensar a inclinação dos managers
            foreach(Transform child in heroesManager.transform){
                child.transform.localEulerAngles = new Vector3(child.transform.localEulerAngles.x, child.transform.localEulerAngles.y, heroesManager.transform.localEulerAngles.z * -1f);
            }
            foreach(Transform child in enemiesManager.transform){
                child.transform.localEulerAngles = new Vector3(child.transform.localEulerAngles.x, child.transform.localEulerAngles.y, enemiesManager.transform.localEulerAngles.z * -1f);
            }
        }

        public BattleUnit LoadUnit(Unit unit, int ppu = 64){
            // Instancia os objetos de UI e normais e faz um referenciar o outro
            GameObject newUnit = Instantiate(unitPrefab, null);
            GameObject newUnitItem = Instantiate(unitItemPrefab, (unit.IsHero)? heroesContent : enemiesContent);
            BattleUnit battleUnit = newUnit.GetComponent<BattleUnit>();
            UnitItem battleItem = newUnitItem.GetComponentInChildren<UnitItem>();
            battleUnit.battleItem = battleItem;
            battleItem.unit = battleUnit;

            // Define as configurações de renderização
            int sortingLayer = 0;
            SpriteRenderer sr = battleUnit.GetComponent<SpriteRenderer>();
            foreach(Transform child in ((unit.IsHero)? heroesContent : enemiesContent)){
                // 1 layer pra unidade, 1 pros status effects e 1 pra skill sendo usada na unidade
                sortingLayer += 3;
            }
            sr.sortingOrder = sortingLayer;
            battleUnit.Configure(unit);
            battleItem.Setup(ppu);
            // Nesse ponto aqui sr.sprite corresponde a um sprite de batalha da unidade
            // Reposiciona o indicador da unidade de acordo com o tamanho do sprite de batalha
            AxisInteractableItem newItem = battleUnit.battleItem.GetComponent<AxisInteractableItem>();
            RectTransform referenceTransform = newItem.transform.parent.Find("Active Reference").GetComponent<RectTransform>();
            referenceTransform.anchoredPosition += (sr.sprite.pixelsPerUnit * battleUnit.OverheadPosition);
            battleUnit.battleItem.layout.preferredWidth = unit.BoundsSizeX * ppu;
            battleUnit.battleItem.layout.preferredHeight = unit.BoundsSizeY * ppu;

            AIIManager manager = (unit.IsHero)? heroesManager : enemiesManager;
            
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

        public void UpdateTargetList(){
            AIIManager manager;
            Unit currentUnit = BattleSkillManager.currentUser.Unit;
            Skill currentSkill = BattleSkillManager.currentSkill;
            bool useOwnManager = (currentSkill.target == TargetType.AllAllies ||
                                  currentSkill.target == TargetType.DeadAllies ||
                                  currentSkill.target == TargetType.DeadAlly ||
                                  currentSkill.target == TargetType.MultiAlly ||
                                  currentSkill.target == TargetType.Self ||
                                  currentSkill.target == TargetType.SingleAlly);
            manager = ((currentUnit.IsHero && useOwnManager) || (!currentUnit.IsHero && !useOwnManager))? heroesManager : enemiesManager;

            // Obtem a lista de possiveis alvos para a skill em questão
            List<BattleUnit> targetUnits = new List<BattleUnit>(BattleSkillManager.currentTargets);
            // Teoricamente essa lista já foi construída usando o método FilterTargets
            // A filtragem acontece no script SkillItem.cs
            // targetUnits = BattleSkillManager.currentSkill.FilterTargets(BattleSkillManager.currentUser, targetUnits);

            manager.ClearItems();
            foreach(BattleUnit unit in targetUnits){
                AxisInteractableItem newItem = unit.battleItem.GetComponent<AxisInteractableItem>();
                newItem.upItem = null;
                newItem.downItem = null;

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
            }
        }

        // Função obsoleta, preciso deletar soon tm
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

                newUnit.GetComponent<Image>().color = unit.Unit.color;

                unit.Configure(unit.Unit);

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
            if (unit.Unit.IsHero)  
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
            if (unit.Unit.IsHero)  
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