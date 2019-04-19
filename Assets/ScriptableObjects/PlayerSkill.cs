using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//engloba todas as "skills" dos personagens do jogador, que ganham nivel
[CreateAssetMenu(fileName = "PlayerSkill", menuName = "ScriptableObject/PlayerSkill", order = 5)]
public class PlayerSkill : Skill{
   public int level; //inivel da "skill"
   public long xp; //experiencia da "skill"
   public long xpNext; //experiencia necessaria para a "skill" subir de nivel
   public string description; //descricao da "skill" que aparecera para o jogador durante a batalha
   public bool active; //sinaliza se a "skill" esta ativa ou nao
    //(...) 
}
