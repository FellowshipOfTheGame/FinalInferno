using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    [RequireComponent(typeof(Collider2D)), RequireComponent(typeof(Animator))]
    public class Changeable : MonoBehaviour {
        [SerializeField,HideInInspector] private Animator anim = null;
        [SerializeField] private List<ChangeRule> changeRules = new List<ChangeRule>();
        private List<Collider2D> colliders = new List<Collider2D>();
        private List<Collider2D> triggers = new List<Collider2D>();

        public void Awake(){
            if(anim == null)
                anim = GetComponent<Animator>();
            foreach(Collider2D col in GetComponents<Collider2D>()){
                if(col.isTrigger)
                    triggers.Add(col);
                else
                    colliders.Add(col);
            }
        }

        public void Reset(){
            if(anim == null)
                anim = GetComponent<Animator>();
        }

        public void Update(){
            foreach(ChangeRule rule in changeRules){
                if(rule.quest != null && rule.quest.events[rule.eventFlag]){
                    anim.SetBool(rule.animationFlag, rule.newValue);
                }
            }
        }

        public void DisableColliders(){
            foreach(Collider2D col in colliders)
                col.enabled = false;
        }

        public void ReenableColliders(){
            foreach(Collider2D col in colliders)
                col.enabled = true;
        }

        public void DisableInteractions(){
            foreach(Collider2D col in triggers)
                col.enabled = false;
        }

        public void ReenableInteractions(){
            foreach(Collider2D col in triggers)
                col.enabled = true;
        }
    }
}