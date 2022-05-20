using System.Collections.Generic;
using System.Collections.ObjectModel;
using FinalInferno.UI.AII;
using FinalInferno.UI.Battle;
using UnityEngine;
using UnityEngine.Events;

namespace FinalInferno {
    #region Custom Delegates/Events
    public delegate void SkillDelegate(BattleUnit user, List<BattleUnit> targets, bool shouldOverride1 = false, float value1 = 0f, bool shouldOverride2 = false, float value2 = 0f);
    public class BattleUnitEvent : UnityEvent<BattleUnit> { }
    #endregion

    [RequireComponent(typeof(Animator)), RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(AudioSource))]
    public class BattleUnit : MonoBehaviour {
        private const string GhostAnimString = "Ghost";
        private const string TakeDamageAnimString = "TakeDamage";
        private const string IsDeadAnimString = "IsDead";
        private const string UseSkillAnimString = "UseSkill";
        public Unit Unit { get; private set; } = null;
        public int MaxHP { private set; get; }
        [Header("Battle Info")]
        private int curHP;
        public int CurHP { get => curHP; private set => curHP = Mathf.Clamp(value, 0, MaxHP); }
        public int curDmg;
        public int curDef;
        public int curMagicDef;
        public int curSpeed;
        public float ActionCostReduction {
            get {
                float maxReduction = 0.75f;
                return Mathf.Clamp(maxReduction * (curSpeed / (Unit.maxStatValue * 1.0f)), 0.0f, maxReduction);
            }
        }
        public int actionPoints;
        public int HpOnHold { get; private set; } = 0;
        public int stuns = 0;
        public bool CanAct => CurHP > 0 && stuns <= 0;
        public float aggro = 0f;
        public float statusResistance = 0f;
        public float damageResistance = 0f;
        public float healResistance = 0f;
        private Dictionary<Element, float> elementalResistances = new Dictionary<Element, float>();
        public List<StatusEffect> effects;
        private List<Skill> activeSkills;
        public ReadOnlyCollection<Skill> ActiveSkills => activeSkills.AsReadOnly();

        #region Unit Callbacks
        public SkillDelegate OnEndBattle = null;
        public SkillDelegate OnStartBattle = null;
        public SkillDelegate OnReceiveBuff = null;
        public SkillDelegate OnReceiveDebuff = null;
        public SkillDelegate OnTakeDamage = null;
        public SkillDelegate OnHeal = null;
        public SkillDelegate OnDeath = null;
        public SkillDelegate OnSkillUsed = null;
        [HideInInspector] public BattleUnitEvent OnTurnStart = new BattleUnitEvent();
        [HideInInspector] public BattleUnitEvent OnTurnEnd = new BattleUnitEvent();
        #endregion

        [Header("References")]
        public UnitItem battleItem;
        [SerializeField] private DamageIndicator damageIndicator;
        [SerializeField] private RectTransform reference;
        public RectTransform Reference => reference;
        [SerializeField] private StatusVFXHandler statusEffectHandler;
        public StatusVFXHandler StatusVFXHandler => statusEffectHandler;
        private Animator animator;
        private AudioSource audioSource;

        private bool hasGhostAnim = false;
        public bool Ghost {
            set {
                if (value && CurHP <= 0 && hasGhostAnim) {
                    animator.SetBool(GhostAnimString, true);
                } else if (!value && hasGhostAnim) {
                    animator.SetBool(GhostAnimString, false);
                }
            }
        }
        public Sprite Portrait { get; private set; }
        public Sprite QueueSprite { get; private set; }
        public Vector2 DefaultSkillPosition { get; private set; } = Vector2.zero;
        public Vector2 HeadPosition { get; private set; } = Vector2.zero;
        public Vector2 TorsoPosition { get; private set; } = Vector2.zero;
        public Vector2 FeetPosition { get; private set; } = Vector2.zero;
        public Vector2 OverheadPosition { get; private set; } = Vector2.zero;

        public void Awake() {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            hasGhostAnim = System.Array.Find(animator.parameters, parameter => parameter.name == GhostAnimString) != null;
            activeSkills = new List<Skill>();
        }

        public void Configure(Unit unit) {
            if (Unit != null) {
                Debug.LogError("Tried to configure already configured BattleUnit", this);
                return;
            }

            Unit = unit;
            name = unit.name;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            SetupUnitRelativePositions(unit, spriteRenderer);
            SaveAnimationAndSpriteInfo(unit);
            ApplyBaseStats(unit);
            SetupElementalResistances(unit);
            actionPoints = 0;
            HpOnHold = 0;
            effects = new List<StatusEffect>();
            statusEffectHandler.Setup(this, spriteRenderer.sortingOrder + 1);
            foreach (Skill skill in unit.skills) {
                if (IsSkillNotBattleRelated(skill))
                    continue;
                SaveOrUseSkill(unit, skill);
            }
        }

        private void SetupUnitRelativePositions(Unit unit, SpriteRenderer spriteRenderer) {
            spriteRenderer.sprite = unit.BattleSprite;
            HeadPosition = CalculateHeadPosition(unit, spriteRenderer);
            OverheadPosition = new Vector2(FeetPosition.x, spriteRenderer.sprite.bounds.size.y);
            Vector2 aux = (HeadPosition - FeetPosition) / 3;
            TorsoPosition = FeetPosition + new Vector2(2 * aux.x, 2 * aux.y);
            DefaultSkillPosition = GetDefaultSkillPosition(unit);
            damageIndicator.GetComponent<RectTransform>().anchoredPosition += new Vector2(HeadPosition.x, HeadPosition.y + 0.35f);
            statusEffectHandler.transform.localPosition = new Vector3(HeadPosition.x, HeadPosition.y + 0.35f);
        }

        private static Vector2 CalculateHeadPosition(Unit unit, SpriteRenderer spriteRenderer) {
            float xHeadPosition = -((spriteRenderer.sprite.bounds.size.x * unit.EffectsRelativePosition.x) - (spriteRenderer.sprite.pivot.x / spriteRenderer.sprite.pixelsPerUnit));
            float yHeadPosition = (spriteRenderer.sprite.bounds.size.y * unit.EffectsRelativePosition.y) - (spriteRenderer.sprite.pivot.y / spriteRenderer.sprite.pixelsPerUnit);
            return new Vector2(xHeadPosition, yHeadPosition);
        }

        private Vector2 GetDefaultSkillPosition(Unit unit) {
            return unit.DefaultTargetPosition switch {
                Unit.BodyPosition.Feet => FeetPosition,
                Unit.BodyPosition.Torso => TorsoPosition,
                Unit.BodyPosition.Head => HeadPosition,
                Unit.BodyPosition.Overhead => OverheadPosition,
                _ => throw new System.NotImplementedException()
            };
        }

        private void SaveAnimationAndSpriteInfo(Unit unit) {
            animator.runtimeAnimatorController = unit.Animator;
            hasGhostAnim = System.Array.Find(animator.parameters, parameter => parameter.name == "Ghost") != null;
            QueueSprite = unit.QueueSprite;
            Portrait = unit.Portrait;
        }

        private void ApplyBaseStats(Unit unit) {
            MaxHP = unit.hpMax;
            if (unit is Hero)
                LoadHeroCurHP(unit);
            curDmg = unit.baseDmg;
            curDef = unit.baseDef;
            curMagicDef = unit.baseMagicDef;
            curSpeed = unit.baseSpeed;
        }

        private void LoadHeroCurHP(Unit unit) {
            foreach (Character character in Party.Instance.characters) {
                if (character.archetype != unit)
                    continue;
                CurHP = character.hpCur;
                return;
            }
        }

        private void SetupElementalResistances(Unit unit) {
            elementalResistances.Clear();
            ReadOnlyDictionary<Element, float> baseResistances = unit.ElementalResistances;
            foreach (Element element in System.Enum.GetValues(typeof(Element))) {
                float resistanceValue = baseResistances.ContainsKey(element) ? baseResistances[element] : 1.0f;
                elementalResistances.Add(element, resistanceValue);
            }
        }

        private static bool IsSkillNotBattleRelated(Skill skill) {
            return (skill.Type != SkillType.Active && !skill.active) || skill is OverworldSkill;
        }

        private void SaveOrUseSkill(Unit unit, Skill skill) {
            switch (skill.Type) {
                case SkillType.Active:
                    activeSkills.Add(skill);
                    break;
                case SkillType.PassiveOnSpawn:
                    skill.Use(this, this);
                    GiveExpToOnSpawnSkill(unit, skill);
                    break;
                case SkillType.PassiveOnStart:
                    OnStartBattle += skill.UseCallbackOrDelayed;
                    break;
                case SkillType.PassiveOnEnd:
                    OnEndBattle += skill.UseCallbackOrDelayed;
                    break;
                case SkillType.PassiveOnTakeDamage:
                    OnTakeDamage += skill.UseCallbackOrDelayed;
                    break;
                case SkillType.PassiveOnReceiveBuff:
                    OnReceiveBuff += skill.UseCallbackOrDelayed;
                    break;
                case SkillType.PassiveOnReceiveDebuff:
                    OnReceiveDebuff += skill.UseCallbackOrDelayed;
                    break;
                case SkillType.PassiveOnDeath:
                    OnDeath += skill.UseCallbackOrDelayed;
                    break;
                case SkillType.PassiveOnSkillUsed:
                    OnSkillUsed += skill.UseCallbackOrDelayed;
                    break;
            }
        }

        private static void GiveExpToOnSpawnSkill(Unit unit, Skill skill) {
            PlayerSkill playerSkill = skill as PlayerSkill;
            if (!(unit is Hero) || playerSkill == null)
                return;

            List<Unit> enemies = new List<Unit>();
            foreach (Unit otherUnit in BattleManager.instance.units) {
                if (otherUnit is Enemy)
                    enemies.Add(otherUnit);
            }
            playerSkill.GiveExp(enemies);
        }

        public void ConfigureMorph(Unit unit, float curHPMultiplier) {
            Unit = unit;
            name = unit.name;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            SetupUnitRelativePositions(unit, spriteRenderer);
            SaveAnimationAndSpriteInfo(unit);
            ApplyBaseStatsMorph(unit, curHPMultiplier);
            SetupElementalResistances(unit);
            activeSkills.Clear();
            statusEffectHandler.Setup(this, spriteRenderer.sortingOrder + 1);
            foreach (Skill skill in unit.skills) {
                if (IsSkillNotBattleRelated(skill))
                    continue;
                SaveOrUseSkillMorph(skill);
            }
        }

        private void ApplyBaseStatsMorph(Unit unit, float curHPMultiplier) {
            MaxHP = unit.hpMax;
            curHPMultiplier = Mathf.Clamp(curHPMultiplier, 0, 1f);
            CurHP = Mathf.Max(1, Mathf.FloorToInt(unit.hpMax * curHPMultiplier));
            curDmg = unit.baseDmg;
            curDef = unit.baseDef;
            curMagicDef = unit.baseMagicDef;
            curSpeed = unit.baseSpeed;
        }

        private void SaveOrUseSkillMorph(Skill skill) {
            switch (skill.Type) {
                case SkillType.Active:
                    activeSkills.Add(skill);
                    break;
                case SkillType.PassiveOnSpawn:
                    skill.Use(this, this);
                    break;
                case SkillType.PassiveOnStart:
                    OnStartBattle += skill.UseCallbackOrDelayed;
                    break;
                case SkillType.PassiveOnEnd:
                    OnEndBattle += skill.UseCallbackOrDelayed;
                    break;
                case SkillType.PassiveOnTakeDamage:
                    OnTakeDamage += skill.UseCallbackOrDelayed;
                    break;
                case SkillType.PassiveOnReceiveBuff:
                    OnReceiveBuff += skill.UseCallbackOrDelayed;
                    break;
                case SkillType.PassiveOnReceiveDebuff:
                    OnReceiveDebuff += skill.UseCallbackOrDelayed;
                    break;
                case SkillType.PassiveOnDeath:
                    OnDeath += skill.UseCallbackOrDelayed;
                    break;
                case SkillType.PassiveOnSkillUsed:
                    OnSkillUsed += skill.UseCallbackOrDelayed;
                    break;
            }
        }

        public void UpdateAggro() {
            aggro *= 0.75f;
        }

        public void UpdateStatusEffects() {
            bool deadUnit = CurHP <= 0;
            foreach (StatusEffect effect in effects.ToArray()) {
                if (!effect.Update())
                    statusEffectHandler.UpdateEffect(effect);
            }
            statusEffectHandler.ApplyChanges();

            // Se uma unidade já morta não tem mais status effects, mata ela de novo
            // Por padrão unidades mortas tem o callback OnDeath setado para null, então nada acontece
            // Isso pode ser usado por um status effect em casos mais específicos que o DelayedSkill não cubra
            if (deadUnit && CurHP <= 0 && effects.Count <= 0)
                BattleManager.instance.Kill(this);
        }

        public void ShowDamage(int value, bool isHeal, float multiplier) {
            if (damageIndicator)
                damageIndicator.ShowDamage(value, isHeal, multiplier);
        }

        public StatusEffect AddEffect(StatusEffect statusEffect, bool ignoreCallback = false) {
            if (statusEffect.Failed) {
                damageIndicator.ShowMiss();
                return null;
            }
            if (BattleManager.instance.CurrentUnit != this)
                BattleManager.instance.Revive(this);
            AddEffectAndApplyAggro(statusEffect);
            CheckStatusEffectCallbacks(statusEffect, ignoreCallback);
            return statusEffect;
        }

        private void AddEffectAndApplyAggro(StatusEffect statusEffect) {
            effects.Add(statusEffect);
            statusEffectHandler.AddEffect(statusEffect);
            statusEffect.Source.aggro += statusEffect.AggroOnApply;
        }

        private void CheckStatusEffectCallbacks(StatusEffect statusEffect, bool ignoreCallback) {
            if (ignoreCallback)
                return;

            List<BattleUnit> targets = new List<BattleUnit> { statusEffect.Target, statusEffect.Source };
            foreach (BattleUnit bUnit in BattleManager.instance.battleUnits) {
                if (!targets.Contains(bUnit))
                    targets.Add(bUnit);
            }
            switch (statusEffect.Type) {
                case StatusType.Buff:
                    OnReceiveBuff?.Invoke(statusEffect.Target, targets, true, effects.IndexOf(statusEffect));
                    break;
                case StatusType.Debuff:
                    OnReceiveDebuff?.Invoke(statusEffect.Target, targets, true, effects.IndexOf(statusEffect));
                    break;
            }
        }

        public void RemoveEffect(StatusEffect effect) {
            if (!effects.Contains(effect))
                return;
            effects.Remove(effect);
            statusEffectHandler.RemoveEffect(effect);
        }

        public int Heal(int atk, float multiplier, BattleUnit healer = null) {
            if (CurHP <= 0)
                return 0;

            int damage = CalculateHealDamage(atk, multiplier);
            CurHP -= damage;
            ShowHealEffects(damage);
            ApplyHealAggro(healer, damage);
            CheckHealDeathAndCallbacks(healer, damage);
            BattleManager.instance.UpdateLives();
            return damage;
        }

        private int CalculateHealDamage(int atk, float multiplier) {
            // healResistance pode ser usado para amplificar cura
            return Mathf.FloorToInt(atk * -multiplier * Mathf.Clamp(1.0f - healResistance, -1.0f, 1.0f));
        }

        private void ShowHealEffects(int damage) {
            if (damage > 0)
                animator.SetTrigger(TakeDamageAnimString);
            damageIndicator.ShowDamage(Mathf.Abs(damage), damage <= 0, 1.0f - healResistance);
        }

        private void ApplyHealAggro(BattleUnit healer, int damage) {
            if (healer != null && (Unit.IsHero == healer.Unit.IsHero))
                healer.aggro += 0.7f * 100f * Mathf.Max(-damage, 0f) / (1.0f * MaxHP);
        }

        private void CheckHealDeathAndCallbacks(BattleUnit healer, int damage) {
            if (CurHP <= 0) {
                Kill();
            } else if (OnHeal != null && damage < 0) {
                List<BattleUnit> aux = new List<BattleUnit> { this };
                OnHeal(healer, aux, true, -damage);
            }
        }

        public int TakeDamage(int atk, float multiplier, DamageType type, Element element, BattleUnit attacker = null) {
            if (CurHP <= 0)
                return 0;

            int damage = CalculateDamage(atk, multiplier, type, element);
            CurHP -= damage;
            ShowDamageEffects(element, damage);
            ApplyDamageAggro(attacker, damage);
            CheckDamageDeathAndCallbacks(attacker, damage, element);
            BattleManager.instance.UpdateLives();
            return damage;
        }

        private int CalculateDamage(int atk, float multiplier, DamageType type, Element element) {
            float atkDifference = atk - ((type == DamageType.Physical) ? curDef : ((type == DamageType.Magical) ? curMagicDef : 0));
            atkDifference = Mathf.Max(atkDifference, 1);
            // damageResistance nao pode amplificar o dano ainda por conta da maneira que iria interagir com a resistencia elemental
            int damage = Mathf.FloorToInt(atkDifference * multiplier * elementalResistances[element] * Mathf.Clamp(1.0f - damageResistance, 0.0f, 1.0f));
            return damage;
        }

        private void ShowDamageEffects(Element element, int damage) {
            if (damage > 0)
                animator.SetTrigger(TakeDamageAnimString);
            damageIndicator.ShowDamage(Mathf.Abs(damage), damage < 0, elementalResistances[element]);
        }

        private void ApplyDamageAggro(BattleUnit attacker, int damage) {
            if (attacker != null && (Unit.IsHero != attacker.Unit.IsHero))
                attacker.aggro += 0.5f * 100f * Mathf.Max(damage, 0f) / (1.0f * MaxHP);
        }

        private void CheckDamageDeathAndCallbacks(BattleUnit attacker, int damage, Element element) {
            if (CurHP <= 0) {
                Kill();
            } else if (OnTakeDamage != null && damage > 0) {
                List<BattleUnit> aux = new List<BattleUnit> { this };
                OnTakeDamage(attacker, aux, true, damage, true, (int)element);
            }
        }

        public void Kill() {
            animator.SetBool(IsDeadAnimString, true);
            RemoveStatusEffects();
            aggro = 0;
            stuns = 0;
            PlayCryIfEnemy();
            BattleManager.instance.Kill(this);
            // Se houver algum callback de morte que, por exemplo, ressucita a unidade ele já vai ter sido chamado aqui
        }

        private void RemoveStatusEffects() {
            foreach (StatusEffect effect in effects.ToArray()) {
                // TO DO: Considerar se vale a pena retirar a checagem de status type
                if (effect.Duration >= 0 && effect.Type != StatusType.None)
                    effect.Remove();
            }
        }

        private void PlayCryIfEnemy() {
            Enemy enemy = Unit as Enemy;
            if (enemy)
                audioSource.PlayOneShot(enemy.EnemyCry);
        }

        public void Revive() {
            if (CurHP > 0)
                return;
            curHP = 1;
            animator.SetBool(IsDeadAnimString, false);
            BattleManager.instance.Revive(this);
        }

        public int DecreaseHP(float lostHPPercent) {
            int lostHPAbsoluteValue = Mathf.Min(MaxHP - 1, Mathf.FloorToInt(lostHPPercent * MaxHP));
            MaxHP = Mathf.Max(Mathf.FloorToInt((1.0f - lostHPPercent) * MaxHP), 1);
            if (lostHPPercent < 0) {
                CurHP += lostHPAbsoluteValue;
            } else {
                HpOnHold += CurHP - MaxHP;
                CurHP = CurHP;
            }
            BattleManager.instance.UpdateLives();
            return lostHPAbsoluteValue;
        }

        public void ResetMaxHP() {
            MaxHP = Unit.hpMax;
            if (curHP > 0)
                CurHP += HpOnHold;
            HpOnHold = 0;
            BattleManager.instance.UpdateLives();
        }

        public void SkillSelected() {
            if (BattleSkillManager.currentSkill != Unit.defenseSkill) {
                animator.SetTrigger(UseSkillAnimString);
            } else {
                UseSkill();
            }
        }

        public void UseSkill() {
            BattleSkillManager.UseSkill();
        }

        public void ShowThisAsATarget() {
            battleItem.ShowThisAsATarget();
        }

        public void ChangeColor() {
            GetComponent<SpriteRenderer>().color = Unit.color;
            foreach (SpriteRenderer sr in gameObject.GetComponentsInChildren<SpriteRenderer>()) {
                sr.color = Unit.color;
            }
        }
    }
}
