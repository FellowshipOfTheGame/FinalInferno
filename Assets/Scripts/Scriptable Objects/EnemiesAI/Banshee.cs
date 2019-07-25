using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;
using FinalInferno.UI.Battle;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Banshee", menuName = "ScriptableObject/Enemy/Banshee", order = 3)]
    public class Banshee : Enemy{
        //funcao que escolhe qual acao sera feita no proprio turno
        public override Skill SkillDecision(float percentageNotDefense){
            float rand = Random.Range(0.0f, 1.0f); //gera um numero aleatorio entre 0 e 1
            float percentageBuff = Mathf.Min(0.3f, percentageNotDefense/3); //porcentagem para o inimigo usar a habilidade de debuff

            if(rand < percentageBuff - BattleManager.instance.enemyDebuff*percentageBuff/3);
                return skills[1]; //decide usar a segunda habilidade(debuff)

            if(rand < percentageNotDefense)
                return AttackDecision(); //decide atacar

            return defenseSkill; //decide defender
        }
    }
}
