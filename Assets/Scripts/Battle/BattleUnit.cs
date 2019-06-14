﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using FinalInferno.UI.Battle;

namespace FinalInferno{
    //representa todos os buffs/debuffs, dano etc que essa unidade recebe
    [RequireComponent(typeof(Animator)),RequireComponent(typeof(SpriteRenderer)),RequireComponent(typeof(FinalInferno.UI.AII.UnitItem))]
    public class BattleUnit : MonoBehaviour{
        public delegate void SkillDelegate(BattleUnit user, List<BattleUnit> targets, bool shouldOverride = false, float value1 = 0f, float value2 = 0f);
        public Unit unit; //referencia para os atributos base dessa unidade
        public int MaxHP { private set; get; }
        private int curHP; //vida atual dessa unidade, descontando dano da vida maxima
        public int CurHP { get{ return curHP; } private set{ curHP = Mathf.Clamp(value, 0, MaxHP); } }
        public int curDmg; //dano atual dessa unidade, contando status de buff/debuff
        public int curDef; //defesa atual dessa unidade, contando status de buff/debuff
        public int curMagicDef; //defesa magica atual dessa unidade, contando status de buff/debuff
        public int curSpeed; //velocidade atual dessa unidade, contando status de buff/debuff
        public int actionPoints; //define a posicao em que essa unidade agira no combate
        public float DebuffResistance { get; private set; } // resistencia a debuffs em geral
        private float damageResistance = 0.0f; // resistencia a danos em geral
        public List<StatusEffect> effects; //lista de status fazendo efeito nessa unidade
        private List<Skill> activeSkills; // lista de skills ativas que essa unidade pode usar
        public ReadOnlyCollection<Skill> ActiveSkills { get{ return activeSkills.AsReadOnly(); } }
        public SkillDelegate OnEndBattle = null;
        public SkillDelegate OnStartBattle = null;

        private Animator animator;

        void Awake(){
            animator = GetComponent<Animator>();
            activeSkills = new List<Skill>();
        }

        public void Configure(Unit unit){
            // Seta configuracoes de renderizacao
            GetComponent<SpriteRenderer>().sprite = unit.battleSprite;
            animator.runtimeAnimatorController = unit.animator;
            GetComponent<FinalInferno.UI.AII.UnitItem>().unit = this;

            if(unit.GetType() == typeof(Enemy)){
                Enemy enemy = (Enemy)unit;
                // Checa se o level do inimigo esta correto
                // Caso nao esteja, aumenta o nivel do inimigo
                // TO DO
            }


            // Aplica os status base da unidade
            this.unit = unit;
            MaxHP = unit.hpMax;
            CurHP = unit.hpMax;
            curDmg = unit.baseDmg;
            curDef = unit.baseDef;
            curMagicDef = unit.baseMagicDef;
            curSpeed = unit.baseSpeed;
            actionPoints = 0; 

            effects = new List<StatusEffect>();

            // Percorre a lista de skills da unidade
            foreach(Skill skill in unit.skills){
                switch(skill.Type){
                    case SkillType.Active:
                        activeSkills.Add(skill);
                        break;
                    case SkillType.PassiveOnSpawn:
                        // Aplica o efeito das skills relevantes na unidade
                        // TO DO
                        break;
                    case SkillType.PassiveOnStart:
                        // Adiciona a skill no callback de inicio de batalha
                        OnStartBattle += skill.Use;
                        break;
                    case SkillType.PassiveOnEnd:
                        // Adiciona a skill no callback de fim de batalha
                        OnEndBattle += skill.Use;
                        break;
                }
            }
        }

        public void ApplyEffects(){
        }

        public void StartListening(){

        }

        public void Act(){
        }

        public void UpdateStatusEffects(){
            foreach (StatusEffect effect in effects.ToArray()){
                effect.Update();
            }
        }

        public void TakeDamage(int atk, float multiplier, DamageType type, Element element) {
            float atkDifference = atk - ( (type == DamageType.Physical)? curDef : ((type == DamageType.Magical)? curMagicDef : 0));
            atkDifference = Mathf.Max(atkDifference, 1);
            int damage = Mathf.FloorToInt(atkDifference * multiplier * 1/*elementmultiplier*/ * (Mathf.Clamp(1.0f - damageResistance, 0.0f, 1.0f)) * 10);
            CurHP -= damage;

            if(CurHP <= 0){
                BattleManager.instance.Kill(this);
                //Destroy(this);
            }

            // else
            // TO DO: Chama a funcao de callback de dano tomado
        }

        public void AddEffect(StatusEffect statusEffect){
            effects.Add(statusEffect);
            switch(statusEffect.Type){
                case StatusType.Buff:
                    // TO DO: chama o callback de receber buff com o status effect atual (value1 = index do status effect novo)
                    break;
                case StatusType.Debuff:
                    // TO DO: chama o callback de receber debuff com o status effect atual (value1 = index do status effect novo)
                    break;
            }
        }

        public void SkillSelected(){
            // TO DO: Esse trigger deve ser setado por fora
            animator.SetTrigger("UseSkill");
            // TO DO: Essa função deve ser chamada pela animação de usar skill usando evento
            BattleManager.instance.UpdateQueue(BattleSkillManager.currentSkill.cost);
            // TODO: Instancia o prefab da skill como filho de cada um dos alvos
        }

        public void UseSkill(){
            // TO DO: Essa função não vai ser responsabilidade do BattleUnit e sim do prefab da animação da skill
            // e ao inves de chamar a função da skill em todos os alvos vai chamar so no alvo que ela é filha
            BattleSkillManager.UseSkill();
            FinalInferno.UI.FSM.AnimationEnded.EndAnimation();
        }
    }
}
