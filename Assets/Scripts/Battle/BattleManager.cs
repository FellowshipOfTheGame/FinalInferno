using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    using UI.Battle;
    using UI.Battle.QueueMenu;
    using UI.Battle.LifeMenu;

    public class BattleManager : MonoBehaviour{
        
        public static BattleManager instance = null;

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

        void OnDestroy(){
            if(instance == this){
                instance = null;
            }
        }

        // #if UNITY_EDITOR
        public void Update(){
            if(StaticReferences.DebugBuild){
                if(Input.GetAxisRaw("Pause") > 0){
                    foreach(BattleUnit bUnit in battleUnits){
                        if(bUnit.CurHP > 0){
                            bUnit.DecreaseHP(1.0f);
                            UpdateLives();
                        }
                    }
                }
            }
        }
        // #endif

        public void PrepareBattle(){
            int ppu = Camera.main.gameObject.GetComponent<UnityEngine.U2D.PixelPerfectCamera>().assetsPPU;
            foreach(Unit unit in units){
                if(unit.IsHero){
                    // Precisa ser salvo antes do LoadUnit para registrar exp das habilidades OnSpawn
                    BattleProgress.addHeroSkills((Hero)unit);
                }

                BattleUnit newUnit = BattleUnitsUI.instance.LoadUnit(unit, ppu);
                battleUnits.Add(newUnit);
                queue.Enqueue(newUnit, -newUnit.curSpeed);
                // Debug.Log("Carregou " + unit.name);

                if(!unit.IsHero){
                    newUnit.ChangeColor();
                    (newUnit.Unit as Enemy).ResetParameters();
                }
            }
            isBattleReady.UpdateValue(true);
        }

        public void StartBattle(){
            foreach(BattleUnit unit in queue.ToArray()){
                if(unit.OnStartBattle != null)
                    unit.OnStartBattle(unit, new List<BattleUnit>(queue.ToArray()));
            }
            UpdateTurn(true); // Status effects aplicados no começo da partida não devem perder um turno
        }

        public void EndTurn(){
            currentUnit = null;
            BattleSkillManager.currentTargets.Clear();
            BattleSkillManager.currentSkill = null;
            BattleSkillManager.currentUser = null;
            BattleSkillManager.skillUsed = false;
        }

        public void UpdateTurn(bool ignoreStatusEffects = false)
        {
            currentUnit = queue.Dequeue();
            if(!ignoreStatusEffects){
                currentUnit.UpdateStatusEffects();
            }
        }

        public void ShowEnemyInfo()
        {
            enemyContent.ShowEnemyInfo(currentUnit);
        }

        public void UpdateQueue(int cost, bool ignoreCurrentUnit = false)
        {
            if(!ignoreCurrentUnit){
                BattleUnit bu = currentUnit;
                currentUnit = null;
                queue.Enqueue(bu, cost);
            }

            EndTurn();
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
            return GetUnitType(currentUnit.Unit);
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

                // Se a unidade que morreu era a unidade atual coloca referencia nula para unidade atual
                if (currentUnit == unit){
                    currentUnit = null;
                }
            }
            UpdateLives();
        }

        public void Revive(BattleUnit unit){
            if(!queue.Contains(unit) && currentUnit != unit){
                queue.Enqueue(unit, 0);
                unitsUI.ReinsertUnit(unit);
            }
            UpdateLives();
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
                    if (unit.CurHP > 0 && GetUnitType(unit.Unit) == type)
                        team.Add(unit);
                }
            }else{
                foreach(BattleUnit unit in battleUnits){
                    if((GetUnitType(unit.Unit) == type) && ( !deadOnly ||(deadOnly && unit.CurHP <= 0)))
                        team.Add(unit);
                }
            }

            return team;
        }

        public List<BattleUnit> GetTeam(BattleUnit battleUnit, bool countDead = false, bool deadOnly = false){
            return GetTeam(GetUnitType(battleUnit.Unit), countDead, deadOnly);
        }

        public List<BattleUnit> GetEnemies(BattleUnit battleUnit, bool countDead = false, bool deadOnly = false){
            return GetTeam(((battleUnit.Unit.IsHero)? UnitType.Enemy : UnitType.Hero), countDead, deadOnly);
        }

        public List<BattleUnit> GetEnemies(UnitType type, bool countDead = false, bool deadOnly = false){
            return GetTeam(((type == UnitType.Enemy)? UnitType.Hero : UnitType.Enemy), countDead, deadOnly);
        }

        public BattleUnit GetBattleUnit(Unit unit){
            foreach(BattleUnit bUnit in battleUnits){
                if(bUnit.Unit == unit) return bUnit;
            }
            return null;
        }
    }
}
