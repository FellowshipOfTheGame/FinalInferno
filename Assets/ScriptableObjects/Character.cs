using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SOs/Character")]
public class Character : ScriptableObject{
    public Hero archetype;
    //public type skillInfo;
    public int hpCur;
    public Vector2 position;

    /*public void LevelUp(int level){

    }*/

    /*public void LoadSkills(int level){
        
    }*/
}
