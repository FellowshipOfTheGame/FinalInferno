using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class GiveExp : MonoBehaviour
    {
        public PlayerSkill skill;

        void Update(){
            #if UNITY_EDITOR
            if(Input.GetKeyDown(KeyCode.X)){
                Party.Instance.GiveExp(5000);
            }

            if(Input.GetKeyDown(KeyCode.C)){
                Party.Instance.GiveExp(500);
            }

            if(Input.GetKeyDown(KeyCode.E)){
                skill.GiveExp(500);
            }

            if(Input.GetKeyDown(KeyCode.R)){
                skill.GiveExp(50);
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
            #endif
        }
    }
}
