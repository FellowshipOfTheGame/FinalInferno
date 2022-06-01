using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "BlasphemousRessurection", menuName = "ScriptableObject/SkillEffect/BlasphemousRessurection")]
    public class BlasphemousRessurection : SkillEffect {
        public override string Description => "Bring Xander back to life after death once";

        [Space(10)]
        [SerializeField] private GameObject visualEffect;

        public override void Apply(BattleUnit source, BattleUnit target) {
            if (target.Unit.name == "Xander") {
                target.OnDeath += Desecrate;
            }
        }

        private void Desecrate(BattleUnit user, List<BattleUnit> targets, bool shouldOverride1 = false, float value1 = 0f, bool shouldOverride2 = false, float value2 = 0f) {
            ShowVFX(user);
            user.Revive();
            user.Heal(user.MaxHP, 1.0f);
        }

        private void ShowVFX(BattleUnit user) {
            if (!visualEffect) {
                return;
            }
            GameObject obj = Instantiate(visualEffect, user.transform);
            obj.GetComponent<SkillVFX>().SetTargetCallback(user);
        }
    }
}
