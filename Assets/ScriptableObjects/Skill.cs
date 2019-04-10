using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SOs/Skill")]
public class Skill : ScriptableObject{
    public string name;
    public int cost;
    //public TARGETTYPE(enum);
    //public ELEMENT(enum) attribute; 
    public List<SkillEffect> effects;

    /*public void Use(List<BattleUnit>){

    }*/
}
