using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.Battle {
    public static class BattleSkillManager {
        public static bool skillUsed = false;
        public static Skill currentSkill = null;
        public static BattleUnit currentUser = null;
        public static List<BattleUnit> currentTargets = new List<BattleUnit>();

        public static void UseSkill() {
            if (currentSkill == null) {
                Debug.Log("Deu null!");
                return; // Nao sei pq isso e necessario, mas essa funcao ta sendo chamada no inicio da batalha pra todas as unidades
                // Isso agora ta aqui pra quando a animação de ataque for chamada por conta do confuse
            }

            if (skillUsed) {
                // Isso nunca deveria acontecer, mas usar o ataque com qualquer hero tava ativando o evento de animação de usar o ataque 2 vezes
                // apesar da animação em si só acontecer uma vez o que não faz sentido ja que funcionava normal usando skill ou com o ataque do inimigo
                Debug.LogError("Tentou usar uma skill duas vezes no mesmo turno");
                return;
            }
            skillUsed = true;

            SkillVFX.nTargets = currentTargets.Count;
            if (currentSkill.GetType() == typeof(PlayerSkill)) {
                ((PlayerSkill)currentSkill).GiveExp(currentTargets);
            }

            // Transform canvasTransform = GameObject.FindObjectOfType<Canvas>().transform;
            // Debug.Log("targets = " + string.Join(", ", currentTargets));
            foreach (BattleUnit target in currentTargets) {
                // Debug.Log("instanciou skill " + currentSkill.name + " no alvo " + target.name);
                GameObject obj = GameObject.Instantiate(currentSkill.VisualEffect, target.transform);
                obj.GetComponent<SkillVFX>().SetTarget(target);
            }
        }

        public static TargetType GetSkillType() {
            return currentSkill != null ? currentSkill.target : TargetType.Null;
        }
    }

}