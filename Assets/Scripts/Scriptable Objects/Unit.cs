using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//engloba todas as unidades que entraram em batalha
public abstract class Unit : ScriptableObject{
    public int level; //nivel dessa unidade
    public int hpMax; //vida maxima dessa unidade
    public int baseDmg; //dano base dessa unidade, sem status de buff/debuff
    public int baseDef; //defesa base dessa unidade, sem status de buff/debuff
    public int baseMagicDef; //defesa magica base dessa unidade, sem status de buff/debuff
    public int baseSpeed; //velocidade base dessa unidade, sem status de buff/debuff
    public Color color; //cor dessa unidade, utilizado para inimigos que tem o mesmo sprite mas nivel de poder diferente 
    public List<Skill> skills; //lista de "skills" da unidade
    //public StatsTable statsTable; //tabela com as statisticas de atributos da unidade baseadas no nivel da unidade
    public Animator animator; //"animator" dessa unidade 
}
