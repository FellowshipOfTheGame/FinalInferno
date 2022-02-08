using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Overseer", menuName = "ScriptableObject/Enemy/Overseer")]
    public class Overseer : Enemy {
        //funcao que escolhe o ataque a ser utilizado
        public override Skill AttackDecision() {
            float rand = Random.Range(0.0f, 1.0f); //gera um numero aleatorio entre 0 e 1

            if (rand < 0.7f) {
                return skills[0]; //decide usar primeira habilidade
            }

            return attackSkill; //decide usar ataque basico
        }
    }

#if UNITY_EDITOR
    [CustomPreview(typeof(Overseer))]
    public class OverseerPreview : UnitPreview {
        public override bool HasPreviewGUI() {
            return base.HasPreviewGUI();
        }
        public override void OnInteractivePreviewGUI(Rect r, GUIStyle background) {
            base.OnInteractivePreviewGUI(r, background);
        }
    }
#endif
}
