using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;

//representa todos os buffs/debuffs, dano etc que essa unidade recebe
public class BattleUnit : MonoBehaviour{
    public Unit unit; //referencia para os atributos base dessa unidade
    public int curHP; //vida atual dessa unidade, descontando dano da vida maxima
    public int curDmg; //dano atual dessa unidade, contando status de buff/debuff
    public int curDef; //defesa atual dessa unidade, contando status de buff/debuff
    public int curMagicDef; //defesa magica atual dessa unidade, contando status de buff/debuff
    public int curSpeed; //velocidade atual dessa unidade, contando status de buff/debuff
    public int actionPoints; //define a posicao em que essa unidade agira no combate
    public List<StatusEffect> effects; //lista de status fazendo efeito nessa unidade

    public void ApplyEffects(){

    }

    public void StartListening(){

    }

    public void Act(){

    }

    public void TakeDamage(int atk, float multiplier, DamageType type, Element element) {
    }
}
