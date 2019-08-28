using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.Battle
{
    public static class BattleSkillManager
    {
        public static Skill currentSkill = null;
        public static BattleUnit currentUser = null;
        public static List<BattleUnit> currentTargets = new List<BattleUnit>();

        public static void UseSkill()
        {
            if(currentSkill == null){
                Debug.Log("Deu null!");
                return; // Nao sei pq isso e necessario, mas essa funcao ta sendo chamada no inicio da batalha pra todas as unidades
            }

            SkillVFX.nTargets = currentTargets.Count;
            if(currentSkill.GetType() == typeof(PlayerSkill)){
                ((PlayerSkill)currentSkill).GiveExp(currentTargets);
            }

            Transform canvasTransform = GameObject.FindObjectOfType<Canvas>().transform;
            foreach(BattleUnit target in currentTargets){
                GameObject obj = GameObject.Instantiate(currentSkill.VisualEffect, target.transform.parent);
                obj.GetComponent<SpriteRenderer>().sortingOrder = target.GetComponent<SpriteRenderer>().sortingOrder + 1;
            }
        }

        public static TargetType GetSkillType()
        {
            return currentSkill != null ? currentSkill.target : TargetType.Null;
        }
    }

}