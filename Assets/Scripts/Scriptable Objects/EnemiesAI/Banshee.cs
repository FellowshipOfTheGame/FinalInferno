using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;
using FinalInferno.UI.Battle;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Banshee", menuName = "ScriptableObject/Enemy/Banshee", order = 3)]
    public class Banshee : Enemy{
        //funcao que escolhe qual acao sera feita no proprio turno
        public override Skill SkillDecision(float percentualDefense){
            float rand = Random.Range(0.0f, 1.0f);

            if(rand < 0.4f)
                return skills[0];
            
            if(rand < percentualDefense)
                return AttackDecision();

            return defenseSkill;
        }
    }
}
