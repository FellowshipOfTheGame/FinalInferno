﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class GiveExp : MonoBehaviour
    {
        public PlayerSkill skill;

        #if UNITY_EDITOR
        void Update(){
            if(Input.GetKeyDown(KeyCode.X)){
                Party.Instance.GiveExp(5000);
            }

            if(Input.GetKeyDown(KeyCode.C)){
                Party.Instance.GiveExp(500);
            }

            if(Input.GetKeyDown(KeyCode.E)){
                if(skill != null)
                skill.GiveExp(500);
            }

            if(Input.GetKeyDown(KeyCode.R)){
                if(skill != null)
                skill.GiveExp(50);
            }

            if(Input.GetKeyDown(KeyCode.T)){
                foreach(Character character in Party.Instance.characters){
                    foreach(PlayerSkill skill in character.archetype.skills.ToArray()){
                        if(skill.active){
                            skill.GiveExp(5000);
                        }
                    }
                }
            }

            if(Input.GetKeyDown(KeyCode.Y)){
                foreach(Character character in Party.Instance.characters){
                    foreach(PlayerSkill skill in character.archetype.skills.ToArray()){
                        if(skill.active){
                            skill.GiveExp(500);
                        }
                    }
                }
            }

            if(Input.GetKeyDown(KeyCode.N)){
                SaveLoader.NewGame();
            }

            if(Input.GetKeyDown(KeyCode.S)){
                SaveLoader.SaveGame();
            }

            if(Input.GetKeyDown(KeyCode.L)){
                SaveLoader.LoadGame();
            }
        }
        #endif
    }
}
