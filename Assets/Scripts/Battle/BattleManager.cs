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
        public List<BattleUnit> battleUnits;
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
            BattleProgress.ResetInfo(Party.Instance);

            foreach(Unit unit in units){
                BattleUnit newUnit = BattleUnitsUI.instance.LoadUnit(unit);
                battleUnits.Add(newUnit);
                queue.Enqueue(newUnit, -newUnit.curSpeed);
                // Debug.Log("Carregou " + unit.name);

                if(unit.IsHero)
                    BattleProgress.addHeroSkills((Hero)unit);
            }
            List<BattleUnit> auxList = new List<BattleUnit>(queue.list);
            foreach(BattleUnit unit in queue.list){
                if(unit.OnStartBattle != null)
                    unit.OnStartBattle(unit, auxList);
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
            queueUI.UpdateQueue();
            foreach (UnitsLives lives in unitsLives)
                lives.UpdateLives();
        }

        public UnitType Turn(){
            return GetUnitType(currentUnit.unit);
        }

        public void Kill(BattleUnit unit){
            // chama a funcao de callback de morte da unidade
            if(unit.OnDeath != null){
                unit.OnDeath(unit, new List<BattleUnit>(battleUnits));
                unit.OnDeath = null;
            }
            // Se a unidade ainda estiver morta atualiza a fila
            if(unit.CurHP <= 0){
                queue.Remove(unit);
                unitsUI.RemoveUnit(unit);

                // Se a unidade que morreu era a unidade atual, anda a fila
                if (currentUnit == unit){
                    currentUnit = null;
                    if (CheckEnd() == VictoryType.Nobody)
                    {
                        UpdateTurn();
                        UpdateLives();
                    }
                }
            }
        }

        public void Revive(BattleUnit unit){
            queue.Enqueue(unit, 0);
            unitsUI.ReinsertUnit(unit);
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
                if (currentUnit != null && GetUnitType(currentUnit.unit) == type)
                    team.Add(currentUnit);

                foreach(BattleUnit unit in queue.list){
                    if (unit != currentUnit && GetUnitType(unit.unit) == type)
                        team.Add(unit);
                }
            }else{
                if (currentUnit != null && GetUnitType(currentUnit.unit) == type && !deadOnly)
                    team.Add(currentUnit);

                foreach(BattleUnit unit in battleUnits){
                    if(unit != currentUnit && (GetUnitType(unit.unit) == type) && ( !deadOnly ||(deadOnly && unit.CurHP < 0)))
                        team.Add(unit);
                }
            }

            return team;
        }

        public List<BattleUnit> GetTeam(BattleUnit battleUnit, bool countDead = false, bool deadOnly = false){
            return GetTeam(GetUnitType(battleUnit.unit), countDead, deadOnly);
        }

        public List<BattleUnit> GetEnemies(BattleUnit battleUnit, bool countDead = false, bool deadOnly = false){
            return GetTeam(((battleUnit.unit.IsHero)? UnitType.Enemy : UnitType.Hero), countDead, deadOnly);
        }

        public List<BattleUnit> GetEnemies(UnitType type, bool countDead = false, bool deadOnly = false){
            return GetTeam(((type == UnitType.Enemy)? UnitType.Hero : UnitType.Enemy), countDead, deadOnly);
        }

        public BattleUnit GetBattleUnit(Unit unit){
            foreach(BattleUnit bUnit in battleUnits){
                if(bUnit.unit == unit) return bUnit;
            }
            return null;
        }
    }
}
