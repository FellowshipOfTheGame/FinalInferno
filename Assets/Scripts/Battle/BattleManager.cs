using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    using UI.Battle;
    using UI.Battle.QueueMenu;
    using UI.Battle.LifeMenu;

    public class BattleManager : MonoBehaviour{
        
        public static BattleManager instance;

        public List<Unit> units;
        private List<BattleUnit> battleUnits;
        public BattleQueue queue;
        public BattleQueueUI queueUI;

        public BattleUnit currentUnit {get; private set;}

        public BattleUnitsUI unitsUI;

        public UnitsLives[] unitsLives;

        public EnemyContent enemyContent;
        public int enemyBuff; //numero de buffs ativos dos inimigos
        public int enemyDebuff; //numero de debuffs ativos dos inimigos


        void Awake() {
            // Singleton
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(this);

            queue = new BattleQueue();
            units = new List<Unit>();
            battleUnits = new List<BattleUnit>();
            enemyBuff = 0;
            enemyDebuff = 0;
        }

        public void StartBattle(){
            foreach(Unit unit in units){
                BattleUnit newUnit = BattleUnitsUI.instance.LoadUnit(unit);
                battleUnits.Add(newUnit);
                queue.Enqueue(newUnit, -newUnit.curSpeed);
                // Debug.Log("Carregou " + unit.name);
            }
            foreach(BattleUnit unit in queue.list){
                if(unit.OnStartBattle != null)
                    unit.OnStartBattle(unit, queue.list);
            }
            UpdateTurn();
        }

        public void UpdateTurn()
        {
            currentUnit = queue.Dequeue();
            currentUnit.UpdateStatusEffects();
            // ShowEnemyInfo();
        }

        public void ShowEnemyInfo()
        {
            enemyContent.ShowEnemyInfo(currentUnit);
        }

        public void UpdateQueue(int cost)
        {
            queue.Enqueue(currentUnit, cost);
            currentUnit = null;

            if (CheckEnd() == VictoryType.Nobody)
            {
                UpdateTurn();
                UpdateLives();
            }
        }

        public void UpdateLives()
        {
            queueUI.LoadQueue();
            foreach (UnitsLives lives in unitsLives)
                lives.UpdateLives();
        }

        public UnitType Turn(){
            return GetUnitType(currentUnit.unit);
        }

        public void Kill(BattleUnit unit){
            queue.Remove(unit);
            unitsUI.RemoveUnit(unit);
            // TO DO: chama a funcao de callback de morte da unidade
        }

        public UnitType GetUnitType(Unit unit){
            return (unit.IsHero) ? UnitType.Hero : UnitType.Enemy;
        }

        public VictoryType CheckEnd(){
            int quantityAll = queue.Count();
            int quantityHeros = 0;

            for(int i = 0; i < quantityAll; i++){
                if((queue.Peek(i).unit).IsHero) quantityHeros++;
            }
            if (currentUnit && currentUnit.unit.IsHero) quantityHeros++;

            if(quantityHeros == quantityAll+1) return VictoryType.Heroes;
            if(quantityHeros == 0) return VictoryType.Enemys;
            else return VictoryType.Nobody;
        }

        public List<BattleUnit> GetTeam(UnitType type, bool countDead = false, bool deadOnly = false){
            List<BattleUnit> team = new List<BattleUnit>();

            if(!countDead){
                if (GetUnitType(currentUnit.unit) == type)
                    team.Add(currentUnit);

                foreach(BattleUnit unit in queue.list){
                    if (unit != currentUnit && GetUnitType(unit.unit) == type)
                        team.Add(unit);
                }
            }else{
                if (GetUnitType(currentUnit.unit) == type && !deadOnly)
                    team.Add(currentUnit);

                foreach(BattleUnit unit in battleUnits){
                    if(unit != currentUnit && (GetUnitType(unit.unit) == type) && ( !deadOnly ||(deadOnly && unit.CurHP < 0)))
                        team.Add(unit);
                }
            }

            return team;
        }

        public List<BattleUnit> GetTeam(BattleUnit battleUnit){
            return GetTeam(GetUnitType(battleUnit.unit));
        }

        public BattleUnit GetBattleUnit(Unit unit){
            foreach(BattleUnit bUnit in battleUnits){
                if(bUnit.unit == unit) return bUnit;
            }
            return null;
        }
    }
}
