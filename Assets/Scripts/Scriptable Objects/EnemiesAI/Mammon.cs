using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Mammon ", menuName = "ScriptableObject/Enemy/Mammon")]
    public class Mammon : Enemy {
        // OBS.: A IA parte do pressuposto que o Mammon é a unica unidade no time inimigo
        // skills[0] = Mammon's Bribe; skills[1] = Savings; skills[2] = The Great Depression; skills[3] = inflation(passive)
        private static int financialSecurity = 0;
        private static bool marketCrashed = false;

        public override void ResetParameters() {
            financialSecurity = 0;
            marketCrashed = false;
        }

        public override Skill SkillDecision(float percentageNotDefense) {
            float rand = Random.Range(0.0f, 1.0f); //gera um numero aleatorio entre 0 e 1
            float percentageDebuff = Mathf.Min((1f / 3f), percentageNotDefense / 3f); //porcentagem para o inimigo usar a habilidade de debuff
            BattleUnit battleUnit = BattleManager.instance.GetTeam(UnitType.Enemy)[0];
            float hpPercent = battleUnit.CurHP / (float)battleUnit.MaxHP;

            // Se o hp passar do threshold, usa a skill de regen
            // habilidade pode ser usada 3 vezes, cada uso move o threshold
            if (financialSecurity < 3 && hpPercent <= (0.75f - (0.25f * financialSecurity))) {
                financialSecurity++;
                return skills[1];
            }

            // Se o hp passar do threshold, usa a skill Great Depression
            // Só pode ser usado uma vez por batalha
            if (!marketCrashed && hpPercent <= (1f / 3f)) {
                marketCrashed = true;
                return skills[2];
            }


            bool bribeCD = false;
            // Se algum hero estiver paralizado considera que a habilidade esta em cooldown
            foreach (BattleUnit unit in BattleManager.instance.GetTeam(UnitType.Hero)) {
                if (!unit.CanAct) {
                    bribeCD = true;
                }
            }

            if (!bribeCD && rand < percentageDebuff) {
                return skills[0]; // Usa bribe
            }

            if (rand < percentageNotDefense) {
                return AttackDecision();
            }

            return defenseSkill;
        }
    }

#if UNITY_EDITOR
    [CustomPreview(typeof(Mammon))]
    public class MammonPreview : UnitPreview {
        public override bool HasPreviewGUI() {
            return base.HasPreviewGUI();
        }
        public override void OnInteractivePreviewGUI(Rect r, GUIStyle background) {
            base.OnInteractivePreviewGUI(r, background);
        }
    }
#endif
}