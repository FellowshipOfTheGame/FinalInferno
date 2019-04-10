using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : ScriptableObject{
    public int level;
    public int hpMax;
    public int baseDmg;
    public int baseDef;
    public int baseMagicDef;
    public int baseSpeed;
    public Color color;
    public List<Skill> skills;
    //public StatsTable statsTable;
    public Animator animator;
}
