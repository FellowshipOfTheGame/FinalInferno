using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno {
    public abstract class Unit : Fog.Dialogue.DialogueEntity {
        private const string xOffsetTooltip = "X axis indicator for the relative position of where status effects show on top of the unit battle sprite. Left margin is 0, right margin is 1, sprites will have the X axis inverted when actually in battle but this value should ignore that";
        private const string yOffsetTooltip = "Y axis indicator for the relative position of where status effects show on top of the unit battle sprite. Bottom is 0, top is 1, the Y axis behaves normally in battle";
		protected const string HPColumnName = "HP";
		protected const string DamageColumnName = "Damage";
		protected const string DefenseColumnName = "Defense";
		protected const string ResistanceColumnName = "Resistance";
		protected const string SpeedColumnName = "Speed";
        public enum BodyPosition {
            Feet = 0,
            Torso = 1,
            Head = 2,
            Overhead = 3
        }

        public const int maxStatValue = 999;
        [Space(10)]
        [Header("Unit Stats")]
        public new string name;
        public string AssetName => base.name;
        public int level;
        public int hpMax;
        public int baseDmg;
        public int baseDef;
        public int baseMagicDef;
        public int baseSpeed;
        [SerializeField] protected ElementResistanceDictionary elementalResistances = new ElementResistanceDictionary();
        public ReadOnlyDictionary<Element, float> ElementalResistances => new ReadOnlyDictionary<Element, float>(elementalResistances);
        [Space(10)]
        [Header("Skill Info")]
        public List<Skill> skills;
        public Skill attackSkill;
        public Skill defenseSkill;
        public virtual bool IsHero => false;
        public abstract long SkillExp { get; }
        [Space(10)]
        [Header("Art")]
        public Color color;
        [SerializeField] protected RuntimeAnimatorController animator;
        public virtual RuntimeAnimatorController Animator => animator;
        [SerializeField] protected Sprite portrait;
        public virtual Sprite Portrait => portrait;
        [SerializeField] protected Sprite dialoguePortrait = null;
        public override Sprite DialoguePortrait => dialoguePortrait;
        [SerializeField] protected Sprite battleSprite;
        public virtual Sprite BattleSprite => battleSprite;
        public virtual float BoundsSizeX => BattleSprite.bounds.size.x;
        public virtual float BoundsSizeY => BattleSprite.bounds.size.y;
        [SerializeField] private BodyPosition defaultTargetPosition = BodyPosition.Feet;
        public BodyPosition DefaultTargetPosition => defaultTargetPosition;
        [Header("    Status Effect Position (read the tooltips)")]
        [Space(-10)]
        [SerializeField, Range(0, 1f), Tooltip(xOffsetTooltip)] protected float xOffset = 0;
        [SerializeField, Range(0, 1f), Tooltip(yOffsetTooltip)] protected float yOffset = 0;
        public virtual Vector2 EffectsRelativePosition => new Vector2(xOffset, yOffset);
        [SerializeField] protected Sprite queueSprite;
        public virtual Sprite QueueSprite => queueSprite;
        public virtual Sprite GetSubUnitPortrait(int index) { return portrait; }
    }
}
