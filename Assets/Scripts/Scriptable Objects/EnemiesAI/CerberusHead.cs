using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;
using FinalInferno.UI.Battle;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "CerberusHead", menuName = "ScriptableObject/Enemy/CerberusHead")]
    public class CerberusHead : Enemy{
        // OBS.: A IA parte do pressuposto que as 3 cabeças do cérbero são as unicas unidades no time inimigo
        // O posicionamento de unidades também será feito levando isso em conta
        // O nível do cérbero deve ser incrementado de 1 em 1 para versões mais fortes
        // O nível das habilidades do cérbero devem ser incrementadas de 3 em 3 para cada versão
        private const int maxHeads = 3;
        public static int heads = 0;
        private static int hellFireCD = 0;
        private static List<GameObject> battleUnits = new List<GameObject>();
        private static BattleUnit topHead = null;
        private static BattleUnit middleHead = null;
        private static BattleUnit bottomHead = null;

        [SerializeField] private Sprite bodySprite;
        [SerializeField] private RuntimeAnimatorController animatorMiddleHead;
        [SerializeField] private RuntimeAnimatorController animatorFrontHead;
        public override RuntimeAnimatorController Animator {
            get{
                switch(heads){
                    case 1:
                        return animator;
                    case 2:
                        return animatorMiddleHead;
                    case 3:
                        return animatorFrontHead;
                    default:
                        return null;
                }
            }
        }
        [SerializeField] private Sprite portraitMiddleHead; //
        [SerializeField] private Sprite portraitFrontHead; //
        public override Sprite Portrait {
            get{
                switch(heads){
                    case 1:
                        return portrait;
                    case 2:
                        return portraitMiddleHead;
                    case 3:
                        return portraitFrontHead;
                    default:
                        return null;
                }
            }
        }
        [SerializeField] private Sprite battleSpriteMiddleHead;
        [SerializeField] private Sprite battleSpriteFrontHead;
        public override Sprite BattleSprite {
            get{
                //Debug.Log("Usou o getter certo");
                switch(heads){
                    default:
                    case 0:
                        heads = 1;
                        battleUnits.Clear();
                        foreach(BattleUnit bUnit in FindObjectsOfType<BattleUnit>()){
                            if(bUnit.unit == this && bUnit.name == this.name){
                                bUnit.name += (" " + heads);
                                battleUnits.Add(bUnit.gameObject);
                                topHead = bUnit;
                                break;
                            }
                        }
                        return battleSprite;
                    case 1:
                        heads++;
                        foreach(BattleUnit bUnit in FindObjectsOfType<BattleUnit>()){
                            if(bUnit.unit == this && bUnit.name == this.name){
                                bUnit.name += (" " + heads);
                                battleUnits.Add(bUnit.gameObject);
                                middleHead = bUnit;
                                break;
                            }
                        }
                        return battleSpriteMiddleHead;
                    case 2:
                        heads++;
                        foreach(BattleUnit bUnit in FindObjectsOfType<BattleUnit>()){
                            if(bUnit.unit == this && bUnit.name == this.name){
                                bUnit.name += (" " + heads);
                                battleUnits.Add(bUnit.gameObject);
                                bottomHead = bUnit;
                                break;
                            }
                        }

                        //Faz os sprites ficarem na mesma posição
                        CompositeBattleUnit composite = middleHead.gameObject.AddComponent<CompositeBattleUnit>();
                        if(composite){
                            composite.AddApendage(topHead);
                            composite.AddApendage(bottomHead);
                        }
                        // Altera o layout group para que os elementos fiquem mais proximos
                        UnityEngine.UI.VerticalLayoutGroup layoutGroup = middleHead.battleItem.layout.transform.parent.GetComponent<UnityEngine.UI.VerticalLayoutGroup>();
                        if(layoutGroup){
                            layoutGroup.spacing = 0;
                        }
                        // Cria um game object para ter o sprite do corpo
                        GameObject bodyObj = new GameObject();
                        bodyObj.name = "Cerberus's body";
                        bodyObj.transform.SetParent(middleHead.transform);
                        bodyObj.transform.position = Vector3.zero;
                        bodyObj.transform.rotation = Quaternion.identity;
                        SpriteRenderer sr = bodyObj.gameObject.AddComponent<SpriteRenderer>();
                        if(sr){
                            sr.sprite = bodySprite;
                            sr.sortingOrder = 0;
                            sr.flipX = true;
                            sr.color = middleHead.GetComponent<SpriteRenderer>().color;
                        }
                        return battleSpriteFrontHead;
                }
            }
        }
        public override float BoundsSizeX { get => (battleSprite.bounds.size.x); }
        public override float BoundsSizeY { get => (battleSprite.bounds.size.y/8f); }
        [SerializeField] private Sprite queueSpriteMiddleHead;
        [SerializeField] private Sprite queueSpriteFrontHead;
        public override Sprite QueueSprite {
            get{
                switch(heads){
                    case 1:
                        return queueSprite;
                    case 2:
                        return queueSpriteMiddleHead;
                    case 3:
                        return queueSpriteFrontHead;
                    default:
                        return null;
                }
            }
        }

        public override void ResetParameters(){
            hellFireCD = 0;
        }

        //funcao que escolhe o ataque a ser utilizado
        public override Skill AttackDecision(){
            if(hellFireCD < 1){
                float rand = Random.Range(0.0f, 1.0f); //gera um numero aleatorio entre 0 e 1

                if(rand < 0.9f/heads){
                    hellFireCD = (heads-1); 
                    skills[0].Level = ((level-1) * 3) + 4 - heads;
                    return skills[0]; //decide usar primeira habilidade
                }
            } else
                hellFireCD--;

            return attackSkill; //decide usar ataque basico
        }

        //funcao que escolhe qual acao sera feita no proprio turno
        public override Skill SkillDecision(float percentageNotDefense){
            float rand = Random.Range(0.0f, 1.0f); //gera um numero aleatorio entre 0 e 1
            float percentageDebuff = Mathf.Min((1f/3f), percentageNotDefense/3f); //porcentagem para o inimigo usar a habilidade de buff
            List<BattleUnit> team;
            bool fearCD = false;

            team = BattleManager.instance.GetTeam(UnitType.Enemy);
            heads = team.Count;

            team = BattleManager.instance.GetTeam(UnitType.Hero);
            foreach (BattleUnit hero in team){
                if(!hero.CanAct) fearCD = true;
            }

            if(!fearCD && rand < percentageDebuff){
                skills[1].Level = ((level - 1) * 3) + 4 - heads;
                return skills[1]; //decide usar a segunda habilidade(debuff)
            }

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
                    for(int i = 0; i < heads && team.Count > 0; i++){
                        int targetIdx = TargetDecision(team);
                        targets.Add(team[targetIdx]);
                        team.RemoveAt(targetIdx);
                    }
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

            heads = 0;
            return targets;
        }
    }
}
