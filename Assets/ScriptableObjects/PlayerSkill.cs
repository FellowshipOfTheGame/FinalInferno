using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SOs/PlayerSkill")]
public class PlayerSkill : Skill{
   public int level;
   public long xp;
   public long xpNext;
   public string description;
   public bool active;
    //(...)
}
