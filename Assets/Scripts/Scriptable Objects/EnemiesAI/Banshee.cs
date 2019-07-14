using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;
using FinalInferno.UI.Battle;

namespace FinalInferno{
    public class Banshee : Enemy{
        public override Skill SkillDecision(){
            float rand = Random.Range(0.0f, 1.0f);

            if(rand < 0.4f)
                return AttackDecision();
            
            if(rand < 0.9f)
                return skills[0];

            return defenseSkill;
        }
    }
}
