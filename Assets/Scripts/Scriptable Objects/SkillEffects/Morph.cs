using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Morph", menuName = "ScriptableObject/SkillEffect/Morph")]
    public class Morph : SkillEffect {
        [SerializeField] private UnitIndex unitIndex;
        // value1 = index of unit to morph into
        // value2 = percentage of max health after morphing
        public override string Description {
            get {
                if (unitIndex == null || unitIndex.UnitList == null || unitIndex.UnitList.Count <= 0) {
                    Debug.LogError($"SkillEffect Morph can't access UnitIndex list");
                    return "ERROR";
                }

                int index = Mathf.Clamp((int)value1, 0, unitIndex.UnitList.Count - 1);
                float percentage = Mathf.Clamp(value2, 0, 1f);
                string targetUnit = unitIndex.UnitList[index].name;

                return $"Morph into {targetUnit} with {percentage * 100}% of Max HP";
            }
        }

        public override void Apply(BattleUnit source, BattleUnit target) {
            if (unitIndex == null || unitIndex.UnitList == null || unitIndex.UnitList.Count <= 0) {
                Debug.LogError($"SkillEffect Morph can't access UnitIndex list");
                return;
            }

            int index = Mathf.Clamp((int)value1, 0, unitIndex.UnitList.Count - 1);
            float percentage = Mathf.Clamp(value2, 0, 1f);
            int previousHP = target.CurHP;

            (unitIndex.UnitList[index] as Enemy)?.LevelEnemy();
            target.Configure(unitIndex.UnitList[index], true, percentage);
            UI.Battle.BattleUnitsUI.Instance.UpdateBattleUnitSize(target, BattleManager.instance.CameraPPU);

            int hpDifference = target.CurHP - previousHP;
            if (hpDifference != 0) {
                target.ShowDamage(Mathf.Abs(hpDifference), (hpDifference > 0), 1.0f);
            }
        }
    }
}
