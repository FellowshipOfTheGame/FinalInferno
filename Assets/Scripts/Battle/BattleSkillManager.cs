using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    public static class BattleSkillManager {
        private static bool skillUsed = false;
        public static Skill CurrentSkill { get; private set; } = null;
        public static BattleUnit CurrentUser { get; private set; } = null;
        public static List<BattleUnit> CurrentTargets { get; private set; } = new List<BattleUnit>();

        public static void EndTurn() {
            skillUsed = false;
            CurrentSkill = null;
            CurrentUser = null;
            CurrentTargets.Clear();
        }

        public static void BeginUnitTurn(BattleUnit unit) {
            CurrentUser = unit;
        }

        public static void SelectSkill(Skill skillSelected) {
            CurrentSkill = skillSelected;
        }

        public static TargetType GetSkillType() {
            return CurrentSkill != null ? CurrentSkill.target : TargetType.Null;
        }

        public static void SetTargets(List<BattleUnit> targets) {
            CurrentTargets = new List<BattleUnit>(targets);
        }

        public static void UseSelectedSkill() {
            if (CurrentSkill == null) {
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

            GiveExpToPlayerSkill();
            InstantiateSkillVFXOnTargets();
        }

        private static void GiveExpToPlayerSkill() {
            if (CurrentSkill is PlayerSkill)
                ((PlayerSkill)CurrentSkill).GiveExp(CurrentTargets);
        }

        private static void InstantiateSkillVFXOnTargets() {
            SkillVFX.nTargets = CurrentTargets.Count;
            foreach (BattleUnit target in CurrentTargets) {
                GameObject obj = Object.Instantiate(CurrentSkill.VisualEffect, target.transform);
                obj.GetComponent<SkillVFX>().SetTarget(target);
            }
        }
    }

}