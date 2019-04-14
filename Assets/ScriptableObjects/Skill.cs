using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//engloba todas as "skills"
[CreateAssetMenu(menuName = "SOs/Skill")]
public class Skill : ScriptableObject{
    public string name; //nome da "skill"
    public int cost; //tempo que a "skill" custara ao conjurador
    //public TARGETTYPE(enum); //tipo de alvo da "skill"
    //public ELEMENT(enum) attribute; //elemento da "skill"
    public List<SkillEffect> effects; //lista de efeitos que a "skill" causa

    //funcao que define como a skill sera usada
    /*public void Use(List<BattleUnit>){

    }*/
}
