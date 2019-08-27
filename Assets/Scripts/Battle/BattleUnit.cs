using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using FinalInferno.UI.Battle;
using FinalInferno.UI.AII;

namespace FinalInferno{
    //representa todos os buffs/debuffs, dano etc que essa unidade recebe
    [RequireComponent(typeof(Animator)),RequireComponent(typeof(SpriteRenderer)),RequireComponent(typeof(UnityEngine.UI.Image)),RequireComponent(typeof(FinalInferno.UI.AII.UnitItem))]
    public class BattleUnit : MonoBehaviour{
        public delegate void SkillDelegate(BattleUnit user, List<BattleUnit> targets, bool shouldOverride1 = false, float value1 = 0f, bool shouldOverride2 = false, float value2 = 0f);
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
        private int hpOnHold;
        public int stuns;
        public bool CanAct{ get{ return (stuns <= 0); } }
        public float aggro;
        public float statusResistance; // resistencia a debuffs em geral
        private float damageResistance = 0.0f; // resistencia a danos em geral
        public float[] elementalResistance = new float[(int)Element.Neutral];
        public List<StatusEffect> effects; //lista de status fazendo efeito nessa unidade
        private List<Skill> activeSkills; // lista de skills ativas que essa unidade pode usar
        public ReadOnlyCollection<Skill> ActiveSkills { get{ return activeSkills.AsReadOnly(); } }
        public SkillDelegate OnEndBattle = null;
        public SkillDelegate OnStartBattle = null;
        //public SkillDelegate OnGiveBuff = null;
        public SkillDelegate OnReceiveBuff = null;
        //public SkillDelegate OnGiveDebuff = null;
        public SkillDelegate OnReceiveDebuff = null;
        public SkillDelegate OnTakeDamage = null;
        public SkillDelegate OnDeath = null;
        public UnitItem battleItem;

        private Animator animator;
        private Transform canvasTransform;
        private Sprite portrait;
        public Sprite Portrait { get => portrait; }
        private Sprite queueSprite;
        public Sprite QueueSprite { get => queueSprite; }

        public void Awake(){
            animator = GetComponent<Animator>();
            activeSkills = new List<Skill>();
            canvasTransform = FindObjectOfType<Canvas>().transform;
        }

        public void Update(){
            if(Mathf.Abs((transform.localScale.x * canvasTransform.localScale.x) - 1.0f) > float.Epsilon){
                transform.localScale = new Vector3(-1.0f/canvasTransform.localScale.x,1.0f/canvasTransform.localScale.y,1.0f/canvasTransform.localScale.z);
            }
        }

        public void Configure(Unit unit){
            this.unit = unit;
            this.name = unit.name;

            // Seta configuracoes de renderizacao
            GetComponent<SpriteRenderer>().sprite = unit.BattleSprite;
            GetComponent<UnityEngine.UI.Image>().sprite = GetComponent<SpriteRenderer>().sprite;
            animator.runtimeAnimatorController = unit.Animator;
            queueSprite = unit.QueueSprite;
            portrait = unit.Portrait;
            GetComponent<FinalInferno.UI.AII.UnitItem>().unit = this;


            // Aplica os status base da unidade
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
            unit.elementalResistance.CopyTo(elementalResistance, 0);
            actionPoints = 0;
            hpOnHold = 0; 

            effects = new List<StatusEffect>();

            // Percorre a lista de skills da unidade
            foreach(Skill skill in unit.skills){
                switch(skill.Type){
                    case SkillType.Active:
                        activeSkills.Add(skill);
                        break;
                    case SkillType.PassiveOnSpawn:
                        // Aplica o efeito das skills relevantes na unidade
                        skill.Use(this, this);
                        if(unit.IsHero)
                            (skill as PlayerSkill).GiveExp(BattleManager.instance.units);
                        break;
                    case SkillType.PassiveOnStart:
                        // Adiciona a skill no callback de inicio de batalha
                        OnStartBattle += skill.Use;
                        break;
                    case SkillType.PassiveOnEnd:
                        // Adiciona a skill no callback de fim de batalha
                        OnEndBattle += skill.Use;
                        break;
                    case SkillType.PassiveOnTakeDamage:
                        // Adiciona a skill no callback de dano tomado
                        OnTakeDamage += skill.Use;
                        break;
                    case SkillType.PassiveOnReceiveBuff:
                        // Adiciona a skill no callback de buff recebido
                        OnReceiveBuff += skill.Use;
                        break;
                    case SkillType.PassiveOnReceiveDebuff:
                        // Adiciona a skill no callback de debuff recebido
                        OnReceiveDebuff += skill.Use;
                        break;
                    case SkillType.PassiveOnDeath:
                        // Adiciona a skill no callback de morte
                        OnDeath += skill.Use;
                        break;
                }
            }
        }

        public void UpdateStatusEffects(){
            foreach (StatusEffect effect in effects.ToArray()){
                if(effect.Update())
                    effects.Remove(effect);
            }
        }

        // TO DO: Criar polimorfismo para dano porcentual de hp e cura, e revisar todos os skill effects relevantes
        public int TakeDamage(int atk, float multiplier, DamageType type, Element element, BattleUnit attacker = null) {
            float atkDifference = atk - ( (type == DamageType.Physical)? curDef : ((type == DamageType.Magical)? curMagicDef : 0));
            atkDifference = Mathf.Max(atkDifference, 1);
            int damage = Mathf.FloorToInt(atkDifference * multiplier * elementalResistance[(int)element - (int)Element.Fire] * (Mathf.Clamp(1.0f - damageResistance, 0.0f, 1.0f))/* * 10 */);
            if(damage > 0 && CurHP <= 0)
                return 0;
            CurHP -= damage;
            // Aplica o aggro pra dano e cura
            if(attacker != null){
                if(damage > 0){
                    // Só triggera a animação de dano tomado se o dano for maior que zero
                    animator.SetTrigger("TakeDamage");
                    attacker.aggro += 0.5f * 100f * damage / (1.0f * MaxHP);
                }else if(damage < 0)
                    attacker.aggro += 0.7f * 100f * damage / (1.0f * MaxHP);
            }

            if(CurHP <= 0){
                //Se a unidade estiver morta, anima a morte
                animator.SetBool("IsDead", true);
                BattleManager.instance.Kill(this);
                // Se houver algum callback de morte que, por exemplo, ressucita a unidade ele já vai ter sido chamado aqui
                // Tira os buffs e debuffs
                foreach(StatusEffect effect in effects.ToArray()){
                    if(effect.Duration >= 0 && effect.Type != StatusType.None){
                        effect.Remove();
                    }
                }
            }else if(OnTakeDamage != null && damage > 0){
            // Chama a funcao de callback de dano tomado
                List<BattleUnit> aux = new List<BattleUnit>();
                aux.Add(this);
                OnTakeDamage(attacker, aux, true, damage, true, (int)element);
            }
            return damage;
        }

        public void Revive(){
            if(CurHP <= 0){
                curHP = 1;
                stuns = 0;
                //Volta a animação de morte
                animator.SetBool("IsDead", false);
                BattleManager.instance.Revive(this);
            }
        }

        public int DecreaseHP(float lostHPPercent){
            int returnValue = Mathf.FloorToInt(lostHPPercent * MaxHP);

            MaxHP = Mathf.Max(Mathf.FloorToInt((1.0f - lostHPPercent) * MaxHP), 1);
            if(lostHPPercent < 0){ // Para usar a mesma função para aumentar hp maximo, o aumento é adicionado como cura
                CurHP += returnValue;
            }else{
                hpOnHold += CurHP - MaxHP;
            }
            CurHP = CurHP;

            return returnValue;
        }

        public void ResetMaxHP(){ // Funcao que deve ser chamada no final da batalha
            MaxHP = unit.hpMax;
            if(curHP > 0){
                CurHP += hpOnHold;
            }
            hpOnHold = 0;
        }

        public void AddEffect(StatusEffect statusEffect, bool ignoreCallback = false){
            if(statusEffect.Failed)
                return;
                
            effects.Add(statusEffect);
            statusEffect.Source.aggro += statusEffect.AggroOnApply;
            List<BattleUnit> targets = new List<BattleUnit>();
            targets.Add(statusEffect.Target);
            switch(statusEffect.Type){
                case StatusType.Buff:
                    // chama o callback de receber buff com o status effect atual (value1 = index do status effect novo)
                    if(OnReceiveBuff != null && !ignoreCallback)
                        OnReceiveBuff(statusEffect.Source, targets, true, effects.IndexOf(statusEffect));
                    break;
                case StatusType.Debuff:
                    // chama o callback de receber debuff com o status effect atual (value1 = index do status effect novo)
                    if(OnReceiveDebuff != null && !ignoreCallback)
                        OnReceiveDebuff(statusEffect.Source, targets, true, effects.IndexOf(statusEffect));
                    break;
            }
        }

        public void SkillSelected(){
            BattleManager.instance.UpdateQueue(Mathf.FloorToInt(BattleSkillManager.currentSkill.cost * (1.0f - ActionCostReduction) ));
            if(BattleSkillManager.currentSkill != unit.defenseSkill){
                animator.SetTrigger("UseSkill");
            }else{
                UseSkill();
            }
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
