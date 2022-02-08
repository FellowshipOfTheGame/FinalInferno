using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno {
    //engloba todas as unidades que entraram em batalha
    public abstract class Unit : Fog.Dialogue.DialogueEntity {
        public enum BodyPosition {
            Feet = 0,
            Torso,
            Head,
            Overhead
        }

        public const int maxStatValue = 999;
        [Space(10)]
        [Header("Unit Stats")]
        public new string name;
        // TO DO: Testar se isso esta funcionando, especificamente para inimigos que tiveram o nome alterado
        public string AssetName => base.name;
        public int level; //nivel dessa unidade
        public int hpMax; //vida maxima dessa unidade
        public int baseDmg; //dano base dessa unidade, sem status de buff/debuff
        public int baseDef; //defesa base dessa unidade, sem status de buff/debuff
        public int baseMagicDef; //defesa magica base dessa unidade, sem status de buff/debuff
        public int baseSpeed; //velocidade base dessa unidade, sem status de buff/debuff
        [SerializeField] protected ElementResistanceDictionary elementalResistances = new ElementResistanceDictionary();
        public ReadOnlyDictionary<Element, float> ElementalResistances => new ReadOnlyDictionary<Element, float>(elementalResistances);
        [Space(10)]
        [Header("Skill Info")]
        public List<Skill> skills; //lista de "skills" da unidade
        public Skill attackSkill; //habilidade basica de ataque
        public Skill defenseSkill; //habilidade basica de defesa
        public virtual bool IsHero => false;
        public abstract long SkillExp { get; }
        [Space(10)]
        [Header("Art")]
        public Color color; //cor dessa unidade, utilizado para inimigos que tem o mesmo sprite mas nivel de poder diferente 
        [SerializeField]
        protected RuntimeAnimatorController animator; //"animator" dessa unidade 
        public virtual RuntimeAnimatorController Animator => animator;
        [SerializeField]
        protected Sprite portrait; //
        public virtual Sprite Portrait => portrait;
        [SerializeField]
        protected Sprite dialoguePortrait = null; //
        public override Sprite DialoguePortrait => dialoguePortrait;
        [SerializeField]
        protected Sprite battleSprite;
        public virtual Sprite BattleSprite => battleSprite;
        public virtual float BoundsSizeX => BattleSprite.bounds.size.x;
        public virtual float BoundsSizeY => BattleSprite.bounds.size.y;
        [SerializeField] private BodyPosition defaultTargetPosition = BodyPosition.Feet;
        public BodyPosition DefaultTargetPosition => defaultTargetPosition;
        [Header("    Status Effect Position (read the tooltips)")]
        [Space(-10)]
        [SerializeField, Range(0, 1f), Tooltip("X axis indicator for the relative position of where status effects show on top of the unit battle sprite. Left margin is 0, right margin is 1, sprites will have the X axis inverted when actually in battle but this value should ignore that")]
        protected float xOffset = 0;
        [SerializeField, Range(0, 1f), Tooltip("Y axis indicator for the relative position of where status effects show on top of the unit battle sprite. Bottom is 0, top is 1, the Y axis behaves normally in battle")]
        protected float yOffset = 0;
        public virtual Vector2 EffectsRelativePosition => new Vector2(xOffset, yOffset);
        [SerializeField]
        protected Sprite queueSprite;
        public virtual Sprite QueueSprite => queueSprite;
#if UNITY_EDITOR
        public virtual Sprite GetSubUnitPortrait(int index) { return portrait; }
#endif
    }

#if UNITY_EDITOR
    [CustomPreview(typeof(Unit))]
    public class UnitPreview : ObjectPreview {
        protected Texture2D tex = null;
        protected Texture2D bg = null;

        public override GUIContent GetPreviewTitle() {
            return new GUIContent("Battle Preview");
        }

        public override bool HasPreviewGUI() {
            Unit unit = target as Unit;
            if (unit != null) {
                return (unit.BattleSprite != null);
            }
            return false;
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background) {
            Unit unit = target as Unit;
            if (unit != null) {
                if (tex == null) {
                    tex = new Texture2D(Mathf.FloorToInt(unit.BattleSprite.textureRect.width), Mathf.FloorToInt(unit.BattleSprite.textureRect.height), unit.BattleSprite.texture.format, false);
                    Color[] colors = unit.BattleSprite.texture.GetPixels(Mathf.FloorToInt(unit.BattleSprite.textureRectOffset.x), Mathf.FloorToInt(unit.BattleSprite.textureRectOffset.y), tex.width, tex.height);
                    tex.SetPixels(colors);
                    tex.Apply();

                    Color[] transparency = new Color[tex.width * tex.height];
                    for (int i = 0; i < transparency.Length; i++) {
                        transparency[i] = Color.clear;
                    }
                    bg = new Texture2D(tex.width, tex.height, tex.format, false, false);
                    bg.SetPixels(transparency);
                    bg.Apply();
                }

                Rect texRect;
                float aspectRatio = tex.height / (float)tex.width;
                float scaledHeight = aspectRatio * 0.8f * r.width;

                if (tex.width > tex.height && (r.height * 0.8f) > scaledHeight) {
                    texRect = new Rect(r.center.x - 0.4f * r.width, r.center.y - aspectRatio * 0.4f * r.width, 0.8f * r.width, aspectRatio * 0.8f * r.width);
                } else {
                    texRect = new Rect(r.center.x - 0.4f * r.height / aspectRatio, r.center.y - 0.4f * r.height, 0.8f * r.height / aspectRatio, 0.8f * r.height);
                }

                float rectSize = 0.1f * Mathf.Max(texRect.width, texRect.height);
                Rect headRect = new Rect(texRect.x + (unit.EffectsRelativePosition.x * texRect.width) - rectSize / 2, texRect.yMax - (unit.EffectsRelativePosition.y * texRect.height) - rectSize / 2, rectSize, rectSize);

                EditorGUI.DrawTextureTransparent(texRect, bg, ScaleMode.StretchToFill);
                GUI.DrawTexture(texRect, tex, ScaleMode.ScaleToFit);
                EditorGUI.DrawRect(headRect, new Color(0f, 1f, 0f, .7f));
            }
        }
    }
#endif
}
