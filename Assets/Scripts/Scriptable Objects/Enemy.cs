using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//englobas os inimigos do jogador
[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObject/Enemy", order = 3)]
public class Enemy : Unit{
        
    //inteligencia atificial do inimigo na batalha
    public Skill AIEnemy(){
        float rand = Random.Range(0, 2);

        if(rand > 1)
            return attackSkill;
        else
            return defenseSkill;
    }
}
