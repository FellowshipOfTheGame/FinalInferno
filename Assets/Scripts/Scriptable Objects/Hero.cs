using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//engloba os tipos/classes de heroi, personagem do jogador
[CreateAssetMenu(fileName = "Hero", menuName = "ScriptableObject/Hero", order = 2)]
public class Hero : Unit{
    public Sprite spriteOW; //"sprite" do heroi no "Over Wolrd"
    public Sprite skillBG; //"sprite" de fundo da arvore de "skills" 
    public Sprite portrait; //

}
