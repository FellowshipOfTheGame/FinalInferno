using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using FinalInferno.UI.Battle;
using FinalInferno.UI.AII;

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
        public float ActionCostReduction{ // Redução porcentual do cust de ações dessa unidade
            get{
                float maxReduction = 0.5f;
                return Mathf.Clamp(maxReduction * (curSpeed / (Unit.maxStatValue * 1.0f)), 0.0f, maxReduction);
            }
        }
        public int actionPoints; //define a posicao em que essa unidade agira no combate
        public float DebuffResistance { get; private set; } // resistencia a debuffs em geral
        private float damageResistance = 0.0f; // resistencia a danos em geral
        public List<StatusEffect> effects; //lista de status fazendo efeito nessa unidade
        private List<Skill> activeSkills; // lista de skills ativas que essa unidade pode usar
        public ReadOnlyCollection<Skill> ActiveSkills { get{ return activeSkills.AsReadOnly(); } }
        public SkillDelegate OnEndBattle = null;
        public SkillDelegate OnStartBattle = null;
        public UnitItem battleItem;

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
            if(unit.IsHero){
                foreach(Character character in Party.Instance.characters){
                    if(character.archetype == unit){
                        CurHP = character.hpCur;
                        break;
                    }
                }
            }else{
                CurHP = unit.hpMax;
            }
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

        public void UpdateStatusEffects(){
            foreach (StatusEffect effect in effects.ToArray()){
                effect.Update();
            }
        }

        public void TakeDamage(int atk, float multiplier, DamageType type, Element element) {
            animator.SetTrigger("TakeDamage");
            float atkDifference = atk - ( (type == DamageType.Physical)? curDef : ((type == DamageType.Magical)? curMagicDef : 0));
            atkDifference = Mathf.Max(atkDifference, 1);
            int damage = Mathf.FloorToInt(atkDifference * multiplier * 1/*elementmultiplier*/ * (Mathf.Clamp(1.0f - damageResistance, 0.0f, 1.0f)) * 10);
            CurHP -= damage;

            if(CurHP <= 0){
                BattleManager.instance.Kill(this);
                animator.SetTrigger("IsDead");
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
            animator.SetTrigger("UseSkill");
            BattleManager.instance.UpdateQueue(Mathf.FloorToInt(BattleSkillManager.currentSkill.cost * (1.0f - ActionCostReduction) ));
        }

        public void UseSkill(){
            BattleSkillManager.UseSkill();
        }

        public void ShowThisAsATarget()
        {
            battleItem.ShowThisAsATarget();
        }
    }
}
