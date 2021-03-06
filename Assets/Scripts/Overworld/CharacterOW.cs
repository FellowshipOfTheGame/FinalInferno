﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;

namespace FinalInferno{
    [RequireComponent(typeof(Movable))]
    public class CharacterOW : MonoBehaviour
    {
        [SerializeField] private Character characterSO;
        public Character CharacterSO { get{ return characterSO; } }
        [SerializeField] private bool isMain;
        // Tamanho desse vetor deve ser igual a Party.Capacity
        // To Do: Fazer esse vetor usar scriptable objects ao inves de ser uma referencia estatica
        private static CharacterOW[] characterList = new CharacterOW[]{null, null, null, null};
        public static ReadOnlyCollection<CharacterOW> CharacterList {get {return System.Array.AsReadOnly(characterList); } }
        public static CharacterOW MainOWCharacter{ get{ return characterList[0]; } }

        public static bool PartyCanMove{
            set{
                foreach(CharacterOW character in characterList){
                    if(character != null){
                        character.GetComponent<Movable>().CanMove = value;
                    }
                }
            }
            get{
                if(characterList[0] != null)
                    return characterList[0].GetComponent<Movable>().CanMove;
                    
                return false;
            }
        }

        public static void ReloadParty(){
            CharacterOW[] oldList = new CharacterOW[characterList.Length];
            characterList.CopyTo(oldList, 0);
            foreach(CharacterOW character in oldList){
                if(character.CharacterSO && character.CharacterSO.archetype){
                    Hero hero = character.CharacterSO.archetype;
                    if(hero.spriteOW){
                        SpriteRenderer sr = character.GetComponent<SpriteRenderer>();
                        sr.sprite = hero.spriteOW;
                        Animator anim = character.GetComponent<Animator>();
                        anim.runtimeAnimatorController = hero.animatorOW;
                    }
                }
                characterList[int.Parse(character.characterSO.name.Split(' ')[1]) - 1] = character;
            }
        }

        public void Awake(){
            BoxCollider2D col = GetComponent<BoxCollider2D>();
            if(!col)
                col = gameObject.AddComponent<BoxCollider2D>();
            if(characterSO && characterSO.archetype){
                Hero hero = characterSO.archetype;
                if(hero.spriteOW){
                    SpriteRenderer sr = GetComponent<SpriteRenderer>();
                    if(!sr)
                        sr = gameObject.AddComponent<SpriteRenderer>();
                    sr.sprite = hero.spriteOW;
                    sr.spriteSortPoint = SpriteSortPoint.Pivot;
                    Animator anim = GetComponent<Animator>();
                    if(!anim)
                        anim = gameObject.AddComponent<Animator>();
                    anim.runtimeAnimatorController = hero.animatorOW;
                    col.offset = new Vector2(0f, 0.16f);
                    col.size = new Vector2(0.4f, 0.32f);
                }
            }
            if(isMain && !MainOWCharacter){
                gameObject.AddComponent<MoveToInput>();
                gameObject.GetComponent<Movable>().Reset();
                gameObject.GetComponent<MoveToInput>().Reset();
                gameObject.AddComponent<Fog.Dialogue.Agent>();
                gameObject.GetComponent<Rigidbody2D>().useFullKinematicContacts = false;
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }else{
                isMain = false;
                gameObject.AddComponent<MoveToTarget>();
                gameObject.GetComponent<Movable>().Reset();
                gameObject.GetComponent<MoveToTarget>().Reset();
                gameObject.GetComponent<Rigidbody2D>().useFullKinematicContacts = false;
                gameObject.GetComponent<Collider2D>().isTrigger = true;
            }
            characterList[int.Parse(characterSO.name.Split(' ')[1]) - 1] = this;
            GetComponent<Movable>().CanMove = false;
        }

        public void Start(){
            if(!isMain){
                GetComponent<MoveToTarget>().target = characterList[System.Array.IndexOf(characterList, this) - 1].gameObject.transform;
                GetComponent<SpriteRenderer>().sortingOrder = MainOWCharacter.GetComponent<SpriteRenderer>().sortingOrder;
            }
        }

        public void OnDestroy(){
            characterList[System.Array.IndexOf(characterList, this)] = null;
        }
    }
}
