using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class CharacterOW : MonoBehaviour
    {
        [SerializeField] private Character characterSO;
        [SerializeField] private bool isMain;
        private static CharacterOW mainOWCharacter;
        public static CharacterOW MainOWCharacter{ get{ return mainOWCharacter; } }
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
                    Animator anim = GetComponent<Animator>();
                    if(!anim)
                        anim = gameObject.AddComponent<Animator>();
                    anim.runtimeAnimatorController = hero.animatorOW;
                    col.size = sr.bounds.size;
                }
            }
            if(isMain){
                gameObject.AddComponent<MoveToInput>();
                gameObject.GetComponent<Movable>().Reset();
                gameObject.GetComponent<MoveToInput>().Reset();
                gameObject.AddComponent<Fog.Dialogue.Agent>();
                mainOWCharacter = this;
            }else{
                gameObject.AddComponent<MoveToTarget>();
                gameObject.GetComponent<Movable>().Reset();
                gameObject.GetComponent<MoveToTarget>().Reset();
            }
        }
        public void Start(){
            if(!isMain){
                GetComponent<MoveToTarget>().target = mainOWCharacter.transform;
            }
        }
    }
}
