using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;
using FinalInferno.UI.Battle;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "CerberusHead", menuName = "ScriptableObject/Enemy/CerberusHead", order = 4)]
    public class CerberusHead : Enemy{
        private const int maxHeads = 3;
        private static int heads = 0;
        private static int hellFireCD = 0;
        private static List<GameObject> battleUnits = new List<GameObject>();

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
                Debug.Log("Usou o getter certo");
                switch(heads){
                    default:
                    case 0:
                        // Nesse ponto aqui reseta-se as variaveis estaticas
                        heads = 1;
                        hellFireCD = 0;
                        battleUnits.Clear();
                        foreach(BattleUnit bUnit in FindObjectsOfType<BattleUnit>()){
                            if(bUnit.unit == this){
                                bUnit.name += (" " + heads);
                                battleUnits.Add(bUnit.gameObject);
                                break;
                            }
                        }
                        return battleSprite;
                    case 1:
                        heads++;
                        foreach(BattleUnit bUnit in FindObjectsOfType<BattleUnit>()){
                            if(bUnit.unit == this){
                                bUnit.name += (" " + heads);
                                battleUnits.Add(bUnit.gameObject);
                                break;
                            }
                        }
                        return battleSpriteMiddleHead;
                    case 2:
                        heads++;
                        foreach(BattleUnit bUnit in FindObjectsOfType<BattleUnit>()){
                            if(bUnit.unit == this){
                                bUnit.name += (" " + heads);
                                battleUnits.Add(bUnit.gameObject);
                                break;
                            }
                        }
                        //TO DO fazer os sprites ficarem na mesma posição
                        //battleUnits[0].transform.position = battleUnits[1].transform.position;
                        //RectTransform aux = battleUnits[0].GetComponent<UnityEngine.UI.Image>().rectTransform;
                        //RectTransform parent = aux.parent as RectTransform;
                        //RectTransform middleParent = battleUnits[1].GetComponent<UnityEngine.UI.Image>().rectTransform.parent as RectTransform;
                        //Camera camera = FindObjectOfType<Camera>();
                        
                        //aux.position = camera.WorldToScreenPoint(camera.ScreenToWorldPoint(battleUnits[1].GetComponent<UnityEngine.UI.Image>().rectTransform.position));
                        //aux.SetParent(middleParent);
                        //aux.anchoredPosition = battleUnits[1].GetComponent<UnityEngine.UI.Image>().rectTransform.anchoredPosition;
                        //aux.SetParent(parent, true);

                        //battleUnits[2].transform.position = battleUnits[1].transform.position;
                        //aux = battleUnits[2].GetComponent<UnityEngine.UI.Image>().rectTransform;
                        //parent = aux.parent as RectTransform;
                        //aux.position = camera.WorldToScreenPoint(camera.ScreenToWorldPoint(battleUnits[1].GetComponent<UnityEngine.UI.Image>().rectTransform.position));
                        //aux.SetParent(middleParent);
                        //aux.anchoredPosition = battleUnits[1].GetComponent<UnityEngine.UI.Image>().rectTransform.anchoredPosition;
                        //aux.SetParent(parent, true);
                        return battleSpriteFrontHead;
                }
            }
        }
        public override float BoundsSizeX { get => (battleSprite.bounds.size.x/3f); }
        public override float BoundsSizeY { get => (battleSprite.bounds.size.y/3f); }
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

            if(!fearCD && rand < percentageDebuff - BattleManager.instance.enemyBuff*percentageDebuff/3)
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

            heads = 0;
            return targets;
        }
    }
}
