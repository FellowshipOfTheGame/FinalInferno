using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using FinalInferno.UI.Battle;
using FinalInferno.UI.AII;

namespace FinalInferno{
    public delegate void SkillDelegate(BattleUnit user, List<BattleUnit> targets, bool shouldOverride1 = false, float value1 = 0f, bool shouldOverride2 = false, float value2 = 0f);
    
    //representa todos os buffs/debuffs, dano etc que essa unidade recebe
    [RequireComponent(typeof(Animator)),RequireComponent(typeof(SpriteRenderer))]
    public class BattleUnit : MonoBehaviour{
        // Status e listas necessarios para a batalha
        public Unit Unit { get; private set; } = null; //referencia para os atributos base dessa unidade
        public int MaxHP { private set; get; }
        [Header("Battle Info")]
        private int curHP; //vida atual dessa unidade, descontando dano da vida maxima
        public int CurHP { get{ return curHP; } private set{ curHP = Mathf.Clamp(value, 0, MaxHP); } }
        public int curDmg; //dano atual dessa unidade, contando status de buff/debuff
        public int curDef; //defesa atual dessa unidade, contando status de buff/debuff
        public int curMagicDef; //defesa magica atual dessa unidade, contando status de buff/debuff
        public int curSpeed; //velocidade atual dessa unidade, contando status de buff/debuff
        public float ActionCostReduction{ // Redução porcentual do cust de ações dessa unidade
            get{
                float maxReduction = 0.75f;
                return Mathf.Clamp(maxReduction * (curSpeed / (Unit.maxStatValue * 1.0f)), 0.0f, maxReduction);
            }
        }
        public int actionPoints; //define a posicao em que essa unidade agira no combate
        private int hpOnHold = 0;
        public int stuns = 0;
        public bool CanAct{ get{ return (CurHP > 0 && stuns <= 0); } }
        public float aggro = 0;
        public float statusResistance = 0; // resistencia a debuffs em geral
        public float damageResistance = 0.0f; // resistencia a danos em geral
        public float healResistance = 0.0f; // resistencias a curas
        private Dictionary<Element, float> elementalResistances = new Dictionary<Element, float>();
        public List<StatusEffect> effects; //lista de status fazendo efeito nessa unidade
        private List<Skill> activeSkills; // lista de skills ativas que essa unidade pode usar
        // Callbacks da unidade
        public ReadOnlyCollection<Skill> ActiveSkills { get => activeSkills.AsReadOnly(); }
        public SkillDelegate OnEndBattle = null;
        public SkillDelegate OnStartBattle = null;
        //public SkillDelegate OnGiveBuff = null;
        public SkillDelegate OnReceiveBuff = null;
        //public SkillDelegate OnGiveDebuff = null;
        public SkillDelegate OnReceiveDebuff = null;
        public SkillDelegate OnTakeDamage = null;
        public SkillDelegate OnHeal = null;
        public SkillDelegate OnDeath = null;
        public SkillDelegate OnSkillUsed = null;

        // Referencias para objetos na cena
        [Header("References")]
        public UnitItem battleItem;
        [SerializeField] private UI.Battle.DamageIndicator damageIndicator;
        [SerializeField] private StatusVFXHandler statusEffectHandler;
        public StatusVFXHandler StatusVFXHandler { get => statusEffectHandler; }
        private Animator animator;
        private Transform canvasTransform;

        // Propriedades auxiliares
        private bool hasGhostAnim = false;
        public bool Ghost{
            set{
                if(value && CurHP < 0 && hasGhostAnim){
                    animator.SetBool("Ghost", true);
                }else if (!value && hasGhostAnim){
                    animator.SetBool("Ghost", false);
                }
            }
        }
        public Sprite Portrait { get; private set; }
        public Sprite QueueSprite { get; private set; }
        public Vector2 DefaultSkillPosition { get; private set; } = Vector2.zero;
        public Vector2 HeadPosition { get; private set; } = Vector2.zero;
        public Vector2 TorsoPosition { get; private set; } = Vector2.zero;
        public Vector2 FeetPosition { get; private set; } = Vector2.zero;
        public Vector2 OverheadPosition {get; private set; } = Vector2.zero;

        public void Awake(){
            animator = GetComponent<Animator>();
            hasGhostAnim = System.Array.Find(animator.parameters, parameter => parameter.name == "Ghost") != null;
            activeSkills = new List<Skill>();
            canvasTransform = FindObjectOfType<Canvas>().transform;
        }

        public void Configure(Unit unit){
            if(Unit != null){
                Debug.LogError("Tentou configurar uma unidade já configurada");
                return;
            }

            Unit = unit;
            this.name = unit.name;

            // Seta configuracoes de renderizacao
            // Essa configuração inicial serve para definir a altura dos objetos que dependem dela
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.sprite = unit.BattleSprite;
            damageIndicator.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, sr.sprite.bounds.size.y);
            // FeetPosition = Vector2.zero; // Ja esta inicializado com esse valor
            HeadPosition = new Vector2(-((sr.sprite.bounds.size.x * unit.effectsRelativePosition.x) - (sr.sprite.pivot.x / sr.sprite.pixelsPerUnit)),
                                        ((sr.sprite.bounds.size.y * unit.effectsRelativePosition.y) - (sr.sprite.pivot.y / sr.sprite.pixelsPerUnit)) );
            OverheadPosition = new Vector2(FeetPosition.x, sr.sprite.bounds.size.y);
            Vector2 aux = (HeadPosition - FeetPosition) / 3;
            TorsoPosition = FeetPosition + new Vector2(2 * aux.x, aux.y);
            switch(unit.DefaultTargetPosition){
                case Unit.BodyPosition.Feet:
                    DefaultSkillPosition = FeetPosition;
                    break;
                case Unit.BodyPosition.Torso:
                    DefaultSkillPosition = TorsoPosition;
                    break;
                case Unit.BodyPosition.Head:
                    DefaultSkillPosition = HeadPosition;
                    break;
                case Unit.BodyPosition.Overhead:
                    DefaultSkillPosition = OverheadPosition;
                    break;
            }
            // move o objeto de status effects para HeadPosition
            statusEffectHandler.transform.localPosition = new Vector3(HeadPosition.x, HeadPosition.y);
            animator.runtimeAnimatorController = unit.Animator;
            hasGhostAnim = System.Array.Find(animator.parameters, parameter => parameter.name == "Ghost") != null;
            QueueSprite = unit.QueueSprite;
            Portrait = unit.Portrait;


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

            ReadOnlyDictionary<Element, float> baseResistances = unit.ElementalResistances;
            foreach(Element element in System.Enum.GetValues(typeof(Element))){
                if(baseResistances.ContainsKey(element)){
                    elementalResistances.Add(element, baseResistances[element]);
                }else{
                    elementalResistances.Add(element, 1.0f);
                }
            }
            actionPoints = 0;
            hpOnHold = 0; 

            effects = new List<StatusEffect>();

            // Manda as configurações base para o handler de status effects
            statusEffectHandler.Setup(this, sr.sortingOrder+1);

            // Percorre a lista de skills da unidade
            foreach(Skill skill in unit.skills){
                // Ignora skills passivas e inativas
                if(skill.Type != SkillType.Active && !skill.active)
                    continue;

                switch(skill.Type){
                    case SkillType.Active:
                        activeSkills.Add(skill);
                        break;
                    case SkillType.PassiveOnSpawn:
                        // Aplica o efeito das skills relevantes na unidade
                        skill.Use(this, this);
                        // Da exp para a skill
                        if(unit.IsHero){
                            List<Unit> enemies = new List<Unit>();
                            foreach(Unit u in BattleManager.instance.units){
                                if(!u.IsHero)
                                    enemies.Add(u);
                            }
                            (skill as PlayerSkill).GiveExp(enemies);
                        }
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
                    case SkillType.PassiveOnSkillUsed:
                        // Adiciona a skill no callback de Skill utilizada
                        OnSkillUsed += skill.Use;
                        break;
                }
            }
        }

        public void UpdateStatusEffects(){
            // Atualiza os valores de aggro
            aggro *= 0.75f;
            bool deadUnit = CurHP <= 0;
            // Atualiza os status effects depois, pois alguns deles afetam o aggro
            foreach (StatusEffect effect in effects.ToArray()){
                if(!effect.Update()){
                    statusEffectHandler.UpdateEffect(effect);
                    // A remoção acontece automaticamente, então a condição do if foi invertida
                    // effects.Remove(effect);
                }
            }
            statusEffectHandler.ApplyChanges();

            // Se uma unidade já morta não tem mais status effects, mata ela de novo
            // Por padrão unidades mortas tem o callback OnDeath setado para null, então nada acontece
            // Isso pode ser usado por um status effect em casos mais específicos que o DelayedSkill não cubra
            if(deadUnit && CurHP <= 0 && effects.Count <= 0){
                BattleManager.instance.Kill(this);
            }
        }

        public StatusEffect AddEffect(StatusEffect statusEffect, bool ignoreCallback = false){
            if(statusEffect.Failed)
                return null;

            if(BattleManager.instance.currentUnit != this) 
                BattleManager.instance.Revive(this); // Se certifica que unidades com status effects aparecem na fila, mesmo mortas
                
            effects.Add(statusEffect);
            statusEffectHandler.AddEffect(statusEffect);
            statusEffect.Source.aggro += statusEffect.AggroOnApply;
            
            List<BattleUnit> targets = new List<BattleUnit>();
            targets.Add(statusEffect.Target);
            targets.Add(statusEffect.Source);
            foreach(BattleUnit bUnit in BattleManager.instance.battleUnits){
                if(!targets.Contains(bUnit))
                    targets.Add(bUnit);
            }

            switch(statusEffect.Type){
                case StatusType.Buff:
                    // chama o callback de receber buff com o status effect atual (value1 = index do status effect novo)
                    if(OnReceiveBuff != null && !ignoreCallback){
                        OnReceiveBuff(statusEffect.Target, targets, true, effects.IndexOf(statusEffect));
                    }
                    break;
                case StatusType.Debuff:
                    // chama o callback de receber debuff com o status effect atual (value1 = index do status effect novo)
                    if(OnReceiveDebuff != null && !ignoreCallback){
                        OnReceiveDebuff(statusEffect.Target, targets, true, effects.IndexOf(statusEffect));
                    }
                    break;
            }

            return statusEffect;
        }

        public void RemoveEffect(StatusEffect effect){
            if(effects.Contains(effect)){
                effects.Remove(effect);
                statusEffectHandler.RemoveEffect(effect);
            }
        }

        public int Heal(int atk, float multiplier, BattleUnit healer = null){
            // healResistance pode ser usado para amplificar cura
            int damage = Mathf.FloorToInt(atk * -multiplier *  (Mathf.Clamp(1.0f - healResistance, -1.0f, 1.0f)));
            if(CurHP <= 0)
                return 0;
            CurHP -= damage;
            if(damage > 0){
                // Só triggera a animação de dano tomado se o dano for maior que zero
                animator.SetTrigger("TakeDamage");
            }
            damageIndicator.ShowDamage(Mathf.Abs(damage), (damage <= 0));

            // Aplica o aggro pra cura
            if(healer != null && (Unit.IsHero == healer.Unit.IsHero)){
                healer.aggro += 0.7f * 100f * Mathf.Max(-damage, 0f) / (1.0f * MaxHP);
            }

            if(CurHP <= 0){
                Kill();
            }else if(OnHeal != null && damage < 0){
                // Chama a funcao de callback de cura
                List<BattleUnit> aux = new List<BattleUnit>();
                aux.Add(this);
                OnHeal(healer, aux, true, -damage);
            }

            BattleManager.instance.UpdateLives();

            return damage;
        }

        public int TakeDamage(int atk, float multiplier, DamageType type, Element element, BattleUnit attacker = null) {
            float atkDifference = atk - ( (type == DamageType.Physical)? curDef : ((type == DamageType.Magical)? curMagicDef : 0));
            atkDifference = Mathf.Max(atkDifference, 1);
            // damageResistance nao pode amplificar o dano ainda por conta da maneira que iria interagir com a resistencia elemental
            int damage = Mathf.FloorToInt(atkDifference * multiplier * elementalResistances[element] * (Mathf.Clamp(1.0f - damageResistance, 0.0f, 1.0f)));
            if(CurHP <= 0)
                return 0;
            CurHP -= damage;
            if(damage > 0){
                // Só triggera a animação de dano tomado se o dano for maior que zero
                animator.SetTrigger("TakeDamage");
            }
            damageIndicator.ShowDamage(Mathf.Abs(damage), (damage < 0));

            // Aplica o aggro pra dano
            if(attacker != null && (Unit.IsHero != attacker.Unit.IsHero)){
                attacker.aggro += 0.5f * 100f * Mathf.Max(damage, 0f) / (1.0f * MaxHP);
            }

            if(CurHP <= 0){
                Kill();
            }else if(OnTakeDamage != null && damage > 0){
                // Chama a funcao de callback de dano tomado
                List<BattleUnit> aux = new List<BattleUnit>();
                aux.Add(this);
                OnTakeDamage(attacker, aux, true, damage, true, (int)element);
            }

            BattleManager.instance.UpdateLives();

            return damage;
        }

        public void Kill(){
            // Anima a morte
            animator.SetBool("IsDead", true);
            // Tira os buffs e debuffs
            foreach(StatusEffect effect in effects.ToArray()){
                // TO DO: Considerar se vale a pena retirar a checagem de status type
                if(effect.Duration >= 0 && effect.Type != StatusType.None){
                    effect.Remove();
                }
            }
            // Reseta o aggro
            aggro = 0;
            stuns = 0;

            BattleManager.instance.Kill(this);
            // Se houver algum callback de morte que, por exemplo, ressucita a unidade ele já vai ter sido chamado aqui
        }

        public void Revive(){
            if(CurHP <= 0){
                curHP = 1;
                //Volta a animação de morte
                animator.SetBool("IsDead", false);
                BattleManager.instance.Revive(this);
            }
        }

        public int DecreaseHP(float lostHPPercent){
            int returnValue = Mathf.Min(MaxHP - 1,  Mathf.FloorToInt(lostHPPercent * MaxHP));

            MaxHP = Mathf.Max(Mathf.FloorToInt((1.0f - lostHPPercent) * MaxHP), 1);
            if(lostHPPercent < 0){ // Para usar a mesma função para aumentar hp maximo, o aumento é adicionado como cura
                CurHP += returnValue;
            }else{
                hpOnHold += CurHP - MaxHP;
            }
            CurHP = CurHP;

            BattleManager.instance.UpdateLives();

            return returnValue;
        }

        public void ResetMaxHP(){ // Funcao que deve ser chamada no final da batalha
            MaxHP = Unit.hpMax;
            if(curHP > 0){
                CurHP += hpOnHold;
            }
            hpOnHold = 0;

            BattleManager.instance.UpdateLives();
        }

        public void SkillSelected(){
            // BattleManager.instance.UpdateQueue(Mathf.FloorToInt(BattleSkillManager.currentSkill.cost * (1.0f - ActionCostReduction) ));
            // Debug.Log("Começou a animação de ataque");
            if(BattleSkillManager.currentSkill != Unit.defenseSkill){
                animator.SetTrigger("UseSkill");
            }else{
                UseSkill();
            }
        }

        public void UseSkill(){
            // Debug.Log("Chamou o evento de animação");
            BattleSkillManager.UseSkill();
        }

        public void ShowThisAsATarget()
        {
            battleItem.ShowThisAsATarget();
        }

        public void ChangeColor()
        {
            // Change color according to rank
            GetComponent<SpriteRenderer>().color = Unit.color;
            foreach(SpriteRenderer sr in gameObject.GetComponentsInChildren<SpriteRenderer>()){
                sr.color = Unit.color;
            }
        }
    }
}
