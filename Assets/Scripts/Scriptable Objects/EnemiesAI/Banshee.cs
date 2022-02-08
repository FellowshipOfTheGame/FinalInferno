using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.Battle;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Banshee", menuName = "ScriptableObject/Enemy/Banshee")]
    public class Banshee : Enemy {
        //funcao que escolhe qual acao sera feita no proprio turno
        public override Skill SkillDecision(float percentageNotDefense) {
            float rand = Random.Range(0.0f, 1.0f); //gera um numero aleatorio entre 0 e 1
            float percentageDebuff = Mathf.Min((1f / 3f), percentageNotDefense / 3); //porcentagem para o inimigo usar a habilidade de debuff

            if (rand < percentageDebuff) {
                return skills[1]; //decide usar a segunda habilidade(debuff)
            }

            if (rand < percentageNotDefense) {
                return AttackDecision(); //decide atacar
            }

            return defenseSkill; //decide defender
        }

        //funcao que escolhe o alvo de um ataque baseado na ameaca que herois representam
        public override int TargetDecision(List<BattleUnit> team) {
            float sumTotal = 0.0f;
            List<float> percentual = new List<float>();

            //soma a ameaca de todos os herois
            foreach (BattleUnit unit in team) {
                sumTotal += Mathf.Clamp(unit.aggro, Mathf.Epsilon, float.MaxValue);
            }

            //calcula a porcentagem que cada heroi representa da soma total das ameacas
            foreach (BattleUnit unit in team) {
                percentual.Add(Mathf.Clamp(unit.aggro, Mathf.Epsilon, float.MaxValue) / sumTotal);
            }

            //gera um numero aleatorio entre 0 e 1
            float rand = Random.Range(0.0f, 1.0f);

            //escolhe o alvo com probabilidades baseadas na porcentagem que cada heroi representa da soma total das ameacas
            for (int i = 0; i < team.Count; i++) {
                if (rand <= percentual[i] && !((BattleSkillManager.currentSkill == skills[1]) && !team[i].CanAct)) {
                    return i; //decide atacar o heroi i
                }

                rand -= percentual[i];
            }

            return team.Count - 1;
        }
    }

#if UNITY_EDITOR
    [CustomPreview(typeof(Banshee))]
    public class BansheePreview : UnitPreview {
        public override bool HasPreviewGUI() {
            return base.HasPreviewGUI();
        }
        public override void OnInteractivePreviewGUI(Rect r, GUIStyle background) {
            base.OnInteractivePreviewGUI(r, background);
        }
    }
#endif
}
