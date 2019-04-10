using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SOs/Party")]
public class Party : ScriptableObject{
    //public type questInfo;
    //public List<Quest> quests;
    public int level;
    public long xp;
    public long xpNext;
    public List<Hero> heroes;
    
    /*public void LevelUp(){

    }*/

    /*public void Save(){

    }*/

    /*public void Load(){

    }*/
}
