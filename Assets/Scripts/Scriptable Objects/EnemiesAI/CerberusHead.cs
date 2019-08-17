using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;
using FinalInferno.UI.Battle;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "CerberusHead", menuName = "ScriptableObject/Enemy/CerberusHead", order = 4)]
    public class CerberusHead : Enemy{
        private static int heads = 0;
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
            List<BattleUnit> team = new List<BattleUnit>();
            bool fearCD = false;

            team = BattleManager.instance.GetTeam(UnitType.Enemy);
            heads = team.Count; 

            team = BattleManager.instance.GetTeam(UnitType.Hero);
            foreach (BattleUnit hero in team){
                if(!hero.CanAct) fearCD = true;
            }

            if(!fearCD && rand < percentageDebuff - BattleManager.instance.enemyBuff*percentageDebuff/3);
                return skills[1]; //decide usar a segunda habilidade(debuff)

            if(rand < percentageNotDefense)
                return AttackDecision(); //decide atacar

            return defenseSkill; //decide defender
        }

        protected override List<BattleUnit> GetTargets(TargetType type){
            List<BattleUnit> targets = new List<BattleUnit>();
            List<BattleUnit> team = new List<BattleUnit>();

            switch (type)
            {
                case TargetType.Self:
                    targets.Add(BattleManager.instance.currentUnit);
                    break;
                case TargetType.MultiAlly:
                    targets = BattleManager.instance.GetTeam(UnitType.Enemy);
                    break;
                case TargetType.MultiEnemy:
                    team = BattleManager.instance.GetTeam(UnitType.Hero);
                    for(int i = 0; i < heads; i++)
                        targets.Add(team[TargetDecision(team)]);
                    break;
                case TargetType.SingleAlly:
                    team = BattleManager.instance.GetTeam(UnitType.Enemy);
                    targets.Add(team[Random.Range(0, team.Count-1)]);
                    break;
                case TargetType.SingleEnemy:
                    team = BattleManager.instance.GetTeam(UnitType.Hero);
                    targets.Add(team[TargetDecision(team)]);
                    break;
            }

            return targets;
        }
    }
}
