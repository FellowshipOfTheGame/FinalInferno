using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;
using FinalInferno.UI.Battle.QueueMenu;

public class BattleManager : MonoBehaviour{
    
    public static BattleManager instance;

    public List<Unit> units;
    public BattleQueue queue;
    public BattleQueueUI queueUI;

    public BattleUnit currentUnit {get; private set;}

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        queue = new BattleQueue();
        foreach(Unit unit in units){
            queue.Enqueue(new BattleUnit(unit), -unit.baseSpeed);
            // Debug.Log("Carregou " + unit.name);
        }
        UpdateTurn();
    }

    public void UpdateTurn()
    {
        currentUnit = queue.Dequeue();
    }

    public void UpdateQueue(int cost)
    {
        queue.Enqueue(currentUnit, cost);
        if (CheckEnd() == VictoryType.Nobody){
            UpdateTurn();
            queueUI.LoadQueue();
        }
    }

    public UnitType Turn(){
        return GetUnitType(currentUnit.unit);
    }

    private UnitType GetUnitType(Unit unit){
        return (IsHero(unit)) ? UnitType.Hero : UnitType.Enemy;
    }


    private bool IsHero(Unit unit){
        return (unit.GetType() == typeof(Hero));
    }

    public VictoryType CheckEnd(){
        int quantityAll = queue.Count();
        int quantityHeros = 0;

        for(int i = 0; i < quantityAll; i++){
            if(IsHero(queue.Peek(i).unit)) quantityHeros++;
        }
        if (IsHero(currentUnit.unit)) quantityHeros++;

        if(quantityHeros == quantityAll+1) return VictoryType.Heroes;
        if(quantityHeros == 0) return VictoryType.Enemys;
        else return VictoryType.Nobody;
    }

    public List<BattleUnit> GetTeam(UnitType type){
        List<BattleUnit> team = new List<BattleUnit>();

        if (GetUnitType(currentUnit.unit) == type)
            team.Add(currentUnit);

        foreach(BattleUnit unit in queue.list){
            if (GetUnitType(unit.unit) == type)
                team.Add(unit);
        }

        return team;
    }
}
