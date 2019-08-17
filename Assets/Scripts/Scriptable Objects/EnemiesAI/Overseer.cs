﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;
using FinalInferno.UI.Battle;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Overseer", menuName = "ScriptableObject/Enemy/Overseer", order = 2)]
    public class Overseer : Enemy{
        //funcao que escolhe o ataque a ser utilizado
        public override Skill AttackDecision(){
            float rand = Random.Range(0.0f, 1.0f); //gera um numero aleatorio entre 0 e 1

            if(rand < 0.9f)
                return skills[0]; //decide usar primeira habilidade
            
            return attackSkill; //decide usar ataque basico
        }
    }
}
