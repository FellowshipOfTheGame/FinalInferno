using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//engloba os personagens do jogador
[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObject/Character", order = 1)]
public class Character : ScriptableObject{
    public Hero archetype; //classe desse personagem
    //public type skillInfo; //
    public int hpCur; //vida atual do personagem, descontando dano da vida maxima
    public Vector2 position; //posicao do personagem no "Over Wolrd"

    //funcao que ajusta todos os atributos e "skills" do persoangem quando sobe de nivel
    /*public void LevelUp(int level){

    }*/

    //quando comeca o jogo, carrega todas as "skills" do personagem baseado no seu nivel
    /*public void LoadSkills(int level){
        
    }*/
}
