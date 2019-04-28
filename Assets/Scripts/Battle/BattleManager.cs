using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;

public class BattleManager : MonoBehaviour{
    public List<Unit> units;
    public BattleQueue queue;

    void Start(){
        foreach(Unit unit in units){
            queue.Enqueue(new BattleUnit(unit));
        }
    }

    void Update(){
        
    }

    public void Turn(){
        BattleUnit battleUnitCur = queue.Dequeue();
        bool isHero = IsHero(battleUnitCur.unit);
    }

    public bool IsHero(Unit unit){
        return (unit.GetType() == typeof(Hero));
    }

    public VictoryType CheckEnd(){
        int quantityAll = queue.Count();
        int quantityHeros = 0;

        for(int i = 0; i < quantityAll; i++){
            if(IsHero(queue.Peek(i).unit)) quantityHeros++;
        }

        if(quantityHeros == quantityAll) return VictoryType.Heroes;
        if(quantityHeros == 0) return VictoryType.Enemys;
        else return VictoryType.Nobody;
    }
}
