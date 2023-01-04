using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/End Battle Callbacks")]
    public class EndBattleCallback : Action {
        public override void Act(StateController controller) {
            ReviveHeroesAndRemoveLeftoverEffects();
            CallEndBattleCallbacks();
            long xpReward = RegisterEnemyKillsAndCalculateExp();
            SaveHpChangesAndExpGained(xpReward);
        }

        private static void ReviveHeroesAndRemoveLeftoverEffects() {
            foreach (BattleUnit battleUnit in BattleManager.instance.battleUnits) {
                if (battleUnit.Unit.IsHero && battleUnit.CurHP <= 0)
                    battleUnit.Revive();
                battleUnit.ResetMaxHP();
                if (battleUnit.Unit.IsHero)
                    continue;
                foreach (StatusEffect effect in battleUnit.effects.ToArray()) {
                    effect.ForceRemove();
                }
            }
        }

        private static void CallEndBattleCallbacks() {
            foreach (BattleUnit battleUnit in BattleManager.instance.battleUnits) {
                battleUnit.OnEndBattle?.Invoke(battleUnit, BattleManager.instance.GetTeam(UnitType.Hero, true));
            }
        }

        private static long RegisterEnemyKillsAndCalculateExp() {
            long xpReward = 0;
            int cerberusCount = 0;
            foreach (BattleUnit battleUnit in BattleManager.instance.battleUnits) {
                if (!(battleUnit.Unit is Enemy))
                    continue;
                Enemy enemy = (Enemy)battleUnit.Unit;
                if (enemy is CerberusHead)
                    cerberusCount++;
                xpReward += enemy.BaseExp;
                if (cerberusCount != 0 && cerberusCount % 3 != 1)
                    continue;
                Party.Instance.RegisterKill(enemy);
            }
            return xpReward;
        }

        private static void SaveHpChangesAndExpGained(long xpReward) {
            foreach (Character character in Party.Instance.characters) {
                character.hpCur = Mathf.Max(BattleManager.instance.GetBattleUnit(character.archetype).CurHP, 1);
            }
            Party.Instance.GiveExp(xpReward);
        }
    }
}
