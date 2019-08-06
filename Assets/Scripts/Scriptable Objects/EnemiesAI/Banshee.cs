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

        //funcao que escolhe o alvo de um ataque baseado na ameaca que herois representam
        public override int TargetDecision(List<BattleUnit> team){
            float sumTotal = 0.0f;
            List<float> percentual = new List<float>();

            //soma a ameaca de todos os herois
            foreach (BattleUnit unit in team){
                sumTotal += unit.aggro;
            }
        
            //calcula a porcentagem que cada heroi representa da soma total das ameacas
            foreach (BattleUnit unit in team){
                percentual.Add(unit.aggro/sumTotal);
            }

            //gera um numero aleatorio entre 0 e 1
            float rand = Random.Range(0.0f, 1.0f);

            //escolhe o alvo com probabilidades baseadas na porcentagem que cada heroi representa da soma total das ameacas
            for(int i = 0; i < team.Count; i++){
                if(rand <= percentual[i] && !((BattleSkillManager.currentSkill == skills[1]) && !team[i].CanAct))
                    return i; //decide atacar o heroi i
                
                rand -= percentual[i];
            }
            
            return team.Count-1;
        }
    }
}
