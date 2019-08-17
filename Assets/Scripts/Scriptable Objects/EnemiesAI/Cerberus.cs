using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;
using FinalInferno.UI.Battle;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Overseer", menuName = "ScriptableObject/Enemy/Overseer", order = 2)]
    public class Cerberus : Enemy{
        private static int heads = 0;
        private static int fearCD = 0;
        private static int hellFireCD = 0;

        //funcao que escolhe o ataque a ser utilizado
        public override Skill AttackDecision(){
            if(hellFireCD < 1){
                float rand = Random.Range(0.0f, 1.0f); //gera um numero aleatorio entre 0 e 1

                if(rand < 0.9f/heads){
                    hellFireCD = heads-1; 
                    return skills[0]; //decide usar primeira habilidade
                }
            }
            else hellFireCD--;

            return attackSkill; //decide usar ataque basico
        }

        //funcao que escolhe qual acao sera feita no proprio turno
        public override Skill SkillDecision(float percentageNotDefense){
            float rand = Random.Range(0.0f, 1.0f); //gera um numero aleatorio entre 0 e 1
            float percentageDebuff = Mathf.Min(0.3f, percentageNotDefense/3); //porcentagem para o inimigo usar a habilidade de buff

            if(rand < percentageDebuff - BattleManager.instance.enemyBuff*percentageDebuff/3);
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
