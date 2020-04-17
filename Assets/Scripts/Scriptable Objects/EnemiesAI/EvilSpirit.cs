using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Evil Spirit", menuName = "ScriptableObject/Enemy/EvilSpirit")]
    public class EvilSpirit : Enemy{
        //funcao que escolhe o ataque a ser utilizado
        public override Skill AttackDecision(){
            float rand = Random.Range(0.0f, 1.0f); //gera um numero aleatorio entre 0 e 1

            if(rand < 0.7f)
                return skills[0]; //decide usar primeira habilidade
            
            return attackSkill; //decide usar ataque basico
        }
    }
}
