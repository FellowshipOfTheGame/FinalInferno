using System.Collections;
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

        //funcao que escolhe qual acao sera feita no proprio turno
        public override Skill SkillDecision(float percentageNotDefense){
            float rand = Random.Range(0.0f, 1.0f); //gera um numero aleatorio entre 0 e 1
            float percentageBuff = Mathf.Min(0.3f, percentageNotDefense/3); //porcentagem para o inimigo usar a habilidade de buff

            if(rand < percentageBuff - BattleManager.instance.enemyBuff*percentageBuff/3);
                return skills[1]; //decide usar a segunda habilidade(buff)

            if(rand < percentageNotDefense)
                return AttackDecision(); //decide atacar

            return defenseSkill; //decide defender
        }
    }
}
