using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;

//engloba todas as "skills"
[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObject/Skill", order = 4)]
public class Skill : ScriptableObject{
    public new string name; //nome da "skill"
    public int cost; //tempo que a "skill" custara ao conjurador
    public TargetType target; //tipo de alvo da "skill"
    public Element attribute; //elemento da "skill"
    public List<SkillEffect> effects; //lista de efeitos que a "skill" causa

    //funcao que define como a skill sera usada
    public void Use(BattleUnit user, List<BattleUnit> targets){
        foreach (BattleUnit trgt in targets) {
            foreach (SkillEffect effect in effects) {
                effect.Apply(user, trgt);
            }
        }
    }
}
