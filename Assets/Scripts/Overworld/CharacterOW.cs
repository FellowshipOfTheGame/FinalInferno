using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;

namespace FinalInferno{
    [RequireComponent(typeof(Movable))]
    public class CharacterOW : MonoBehaviour, IOverworldSkillListener
    {
        [SerializeField] private Character characterSO;
        [SerializeField] private OverworldSkill sprintSkill;
        public Character CharacterSO { get{ return characterSO; } }
        [SerializeField] private bool isMain;

        private SpriteRenderer spriteRenderer = null;
        private MoveTo moveTo = null;
        private Movable movable = null;
        public bool CanMove{
            get => movable? movable.CanMove : false;
            set{
                if(movable != null){
                    movable.CanMove = value;
                }
            }
        }

        public static CharacterOW MainOWCharacter{ get{ return Party.Instance.characters?[0]?.OverworldInstance; } }

        public static bool PartyCanMove{
            get => Party.Instance.characters?[0]?.OverworldInstance?.CanMove ?? false;
            set{
                foreach(Character character in Party.Instance.characters){
                    if(character != null && character.OverworldInstance){
                        character.OverworldInstance.CanMove = value;
                    }
                }
            }
        }

        public static void ReloadParty(){
            foreach(Character character in Party.Instance.characters){
                if(character && character.archetype){
                    Hero hero = character.archetype;
                    CharacterOW characterOW = character?.OverworldInstance;
                    if(hero.spriteOW && characterOW){
                        characterOW.spriteRenderer.sprite = hero.spriteOW;
                        Animator anim = characterOW.GetComponent<Animator>();
                        anim.runtimeAnimatorController = hero.animatorOW;
                    }
                }
            }
        }

        public void Awake(){
            characterSO.OverworldInstance = this;
            if(characterSO.OverworldInstance != this){
                Debug.LogError($"Failed to set {name} as overworld instance of {characterSO.name}");
                return;
            }
            movable = GetComponent<Movable>();

            BoxCollider2D col = GetComponent<BoxCollider2D>();
            if(!col)
                col = gameObject.AddComponent<BoxCollider2D>();
            if(characterSO && characterSO.archetype){
                Hero hero = characterSO.archetype;
                if(hero.spriteOW){
                    spriteRenderer = GetComponent<SpriteRenderer>();
                    if(!spriteRenderer)
                        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = hero.spriteOW;
                    spriteRenderer.spriteSortPoint = SpriteSortPoint.Pivot;
                    Animator anim = GetComponent<Animator>();
                    if(!anim)
                        anim = gameObject.AddComponent<Animator>();
                    anim.runtimeAnimatorController = hero.animatorOW;
                    col.offset = new Vector2(0f, 0.16f);
                    col.size = new Vector2(0.4f, 0.32f);
                }
            }
            if(isMain && !MainOWCharacter){
                gameObject.GetComponent<Movable>().Reset();
                gameObject.GetComponent<Rigidbody2D>().useFullKinematicContacts = false;
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }else{
                moveTo = gameObject.AddComponent<MoveToTarget>();
                movable.Reset();
                moveTo.Reset();
                gameObject.GetComponent<Rigidbody2D>().useFullKinematicContacts = false;
                gameObject.GetComponent<Collider2D>().isTrigger = true;
            }
            movable.CanMove = false;
        }

        public void Start(){
            if(!isMain){
                int characterIndex = Party.Instance.characters.IndexOf(characterSO);
                (moveTo as MoveToTarget).target = Party.Instance.characters[characterIndex-1].OverworldInstance?.transform;
                spriteRenderer.sortingOrder = MainOWCharacter.spriteRenderer.sortingOrder;
            }
        }

        public void OnDestroy(){
            characterSO.OverworldInstance = (characterSO.OverworldInstance == this)? null : characterSO.OverworldInstance;
        }

        #region SprintCallbacks
        public void OnEnable(){
            sprintSkill?.AddActivationListener(this);
        }

        public void OnDisable(){
            sprintSkill?.RemoveActivationListener(this);
        }

		public void ActivatedSkill(OverworldSkill skill)
		{
            if(skill == sprintSkill){
                float moveSpeedChange = sprintSkill?.effects[0].value1 ?? 0;
                movable.MoveSpeed += moveSpeedChange;
            }
		}

		public void DeactivatedSkill(OverworldSkill skill)
		{
            if(skill == sprintSkill){
                float moveSpeedChange = sprintSkill?.effects[0].value1 ?? 0;
                movable.MoveSpeed -= moveSpeedChange;
            }
		}
        #endregion
	}
}
