using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//englobas os inimigos do jogador
[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObject/Enemy", order = 3)]
public class Enemy : Unit{
    //public AIEnemy ai; //inteligencia atificial do inimigo na batalha
    
    public override Color DialogueColor { get { return color; } }
    public override string DialogueName { get { return (name == null)? "" : name; } }
}
