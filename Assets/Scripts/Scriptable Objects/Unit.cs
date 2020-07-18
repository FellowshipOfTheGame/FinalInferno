using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno{
    //engloba todas as unidades que entraram em batalha
    public abstract class Unit : Fog.Dialogue.DialogueEntity {
        public const int maxStatValue = 999;
        [Space(10)]
        [Header("Unit Stats")]
        public new string name;
        // TO DO: Testar se isso esta funcionando, especificamente para inimigos que tiveram o nome alterado
        public string AssetName { get => base.name; }
        public int level; //nivel dessa unidade
        public int hpMax; //vida maxima dessa unidade
        public int baseDmg; //dano base dessa unidade, sem status de buff/debuff
        public int baseDef; //defesa base dessa unidade, sem status de buff/debuff
        public int baseMagicDef; //defesa magica base dessa unidade, sem status de buff/debuff
        public int baseSpeed; //velocidade base dessa unidade, sem status de buff/debuff
        public float[] elementalResistance = new float[(int)Element.Neutral];
        [Space(10)]
        [Header("Skill Info")]
        public List<Skill> skills; //lista de "skills" da unidade
        public Skill attackSkill; //habilidade basica de ataque
        public Skill defenseSkill; //habilidade basica de defesa
        public virtual bool IsHero{ get{ return false; } }
        public abstract long SkillExp {get;}
        [Space(10)]
        [Header("Art")]
        public Color color; //cor dessa unidade, utilizado para inimigos que tem o mesmo sprite mas nivel de poder diferente 
        [SerializeField]
        protected RuntimeAnimatorController animator; //"animator" dessa unidade 
        public virtual RuntimeAnimatorController Animator { get => animator; }
        [SerializeField]
        protected Sprite portrait; //
        public virtual Sprite Portrait { get => portrait; }
        [SerializeField]
        protected Sprite dialoguePortrait = null; //
        public override Sprite DialoguePortrait { get => dialoguePortrait; }
        [SerializeField]
        protected Sprite battleSprite;
        public virtual Sprite BattleSprite { get => battleSprite; }
        public virtual float BoundsSizeX { get => BattleSprite.bounds.size.x; }
        public virtual float BoundsSizeY { get => BattleSprite.bounds.size.y; }
        [SerializeField]
        protected Sprite queueSprite;
        public virtual Sprite QueueSprite { get => queueSprite; }
    }
}
