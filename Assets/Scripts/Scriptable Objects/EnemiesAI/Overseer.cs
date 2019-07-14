using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;
using FinalInferno.UI.Battle;

namespace FinalInferno{
    public class Overseer : Enemy{
        public override Skill AttackDecision(){
            float rand = Random.Range(0.0f, 1.0f);

            if(rand < 0.9f)
                return skills[0];
            
            return attackSkill;
        }

        public override Skill SkillDecision(){
            float rand = Random.Range(0.0f, 1.0f);

            if(rand < 0.6f)
                return AttackDecision();

            if(rand < 0.9f)
                return skills[1]; 

            return defenseSkill;
        }
    }
}
