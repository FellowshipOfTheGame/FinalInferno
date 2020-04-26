using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    using UI.Battle;
    using UI.Battle.QueueMenu;
    using UI.Battle.LifeMenu;

    public class BattleManager : MonoBehaviour{
        
        public static BattleManager instance;

        public FinalInferno.UI.FSM.BoolDecision isBattleReady;
        public List<Unit> units;
        public List<BattleUnit> battleUnits;
        public BattleQueue queue;
        [SerializeField] private BattleQueueUI queueUI;

        public BattleUnit currentUnit {get; private set;}

        public BattleUnitsUI unitsUI;

        public UnitsLives[] unitsLives;

        public EnemyContent enemyContent;


        void Awake() {
            // Singleton
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(this);

            queue = new BattleQueue(queueUI);
            units = new List<Unit>();
            battleUnits = new List<BattleUnit>();
            BattleProgress.ResetInfo(Party.Instance);
        }

        #if UNITY_EDITOR
        public void Update(){
            if(Input.GetAxisRaw("Pause") > 0){
                foreach(BattleUnit bUnit in battleUnits){
                    if(bUnit.CurHP > 0){
                        bUnit.DecreaseHP(1.0f);
                        UpdateLives();
                    }
                }
            }
        }
        #endif

        public void PrepareBattle(){
            foreach(Unit unit in units){
                if (unit.IsHero)
                    BattleProgress.addHeroSkills((Hero)unit);

                BattleUnit newUnit = BattleUnitsUI.instance.LoadUnit(unit);
                battleUnits.Add(newUnit);
                queue.Enqueue(newUnit, -newUnit.curSpeed);
                // Debug.Log("Carregou " + unit.name);

                if(!unit.IsHero)
                    newUnit.ChangeColor();
            }
            isBattleReady.UpdateValue(true);
        }

        public void StartBattle(){
            foreach(BattleUnit unit in queue.ToArray()){
                if(unit.OnStartBattle != null)
                    unit.OnStartBattle(unit, new List<BattleUnit>(queue.ToArray()));
            }
            UpdateTurn();
        }

        public void UpdateTurn()
        {
            currentUnit = queue.Dequeue();
            BattleSkillManager.currentTargets.Clear();
            BattleSkillManager.currentSkill = null;
            BattleSkillManager.currentUser = null;
            currentUnit.UpdateStatusEffects();
        }

        public void ShowEnemyInfo()
        {
            enemyContent.ShowEnemyInfo(currentUnit);
        }

        public void UpdateQueue(int cost, bool ignoreCurrentUnit = false)
        {
            if(!ignoreCurrentUnit){
                queue.Enqueue(currentUnit, cost);
                currentUnit = null;
            }

            if (CheckEnd() == VictoryType.Nobody)
            {
                UpdateTurn();
                UpdateLives();
            }else{
                // Só pra fazer com que nao fique uma unidade duplicada na fila
                queue.Sort();
            }
        }

        public void UpdateLives()
        {
            foreach (UnitsLives lives in unitsLives)
                lives.UpdateLives();
        }

        public UnitType Turn(){
            if(currentUnit == null){
                return UnitType.Null;
            }
            return GetUnitType(currentUnit.unit);
        }

        public void Kill(BattleUnit unit){
            // chama a funcao de callback de morte da unidade
            int nEffectsBefore = unit.effects.Count;
            if(unit.OnDeath != null){
                unit.OnDeath(unit, new List<BattleUnit>(battleUnits));
                unit.OnDeath = null;
            }
            int nNewEffects = unit.effects.Count - nEffectsBefore;
            // Se a unidade ainda estiver morta atualiza a fila
            if(unit.CurHP <= 0 && nNewEffects <= 0){
                queue.Remove(unit);
                unitsUI.RemoveUnit(unit);

                // Se a unidade que morreu era a unidade atual, anda a fila
                if (currentUnit == unit){
                    currentUnit = null;
                    // if (CheckEnd() == VictoryType.Nobody)
                    // {
                    //     UpdateTurn();
                    //     UpdateLives();
                    // }
                }
            }
            UpdateLives();
        }

        public void Revive(BattleUnit unit){
            if(!queue.Contains(unit) && currentUnit != unit){
                queue.Enqueue(unit, 0);
                unitsUI.ReinsertUnit(unit);
            }
            foreach (UnitsLives lives in unitsLives)
                lives.UpdateLives();
        }

        public UnitType GetUnitType(Unit unit){
            return (unit.IsHero) ? UnitType.Hero : UnitType.Enemy;
        }

        public VictoryType CheckEnd(){
            List<BattleUnit> team = GetTeam(UnitType.Enemy);
            if(team.Count <= 0) return VictoryType.Heroes;

            team = GetTeam(UnitType.Hero);
            if(team.Count <= 0) return VictoryType.Enemys;

            return VictoryType.Nobody;
        }

        public List<BattleUnit> GetTeam(UnitType type, bool countDead = false, bool deadOnly = false){
            List<BattleUnit> team = new List<BattleUnit>();

            if(!countDead){
                foreach(BattleUnit unit in battleUnits){
                    if (unit.CurHP > 0 && GetUnitType(unit.unit) == type)
                        team.Add(unit);
                }
            }else{
                foreach(BattleUnit unit in battleUnits){
                    if((GetUnitType(unit.unit) == type) && ( !deadOnly ||(deadOnly && unit.CurHP <= 0)))
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
