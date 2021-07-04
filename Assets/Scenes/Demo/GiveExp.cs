using UnityEngine;
using UnityEngine.InputSystem;

namespace FinalInferno{
    public class GiveExp : MonoBehaviour
    {
        public PlayerSkill skill;

        #if UNITY_EDITOR
        void Update(){
            if(Keyboard.current[Key.X].wasPressedThisFrame){
                Party.Instance.GiveExp(5000);
            }

            if(Keyboard.current[Key.C].wasPressedThisFrame){
                Party.Instance.GiveExp(500);
            }

            if(Keyboard.current[Key.E].wasPressedThisFrame){
                if(skill != null)
                skill.GiveExp(500);
            }

            if(Keyboard.current[Key.R].wasPressedThisFrame){
                if(skill != null)
                skill.GiveExp(50);
            }

            if(Keyboard.current[Key.T].wasPressedThisFrame){
                foreach(Character character in Party.Instance.characters){
                    foreach(PlayerSkill skill in character.archetype.skills.ToArray()){
                        if(skill.active){
                            skill.GiveExp(5000);
                        }
                    }
                }
            }

            if(Keyboard.current[Key.Y].wasPressedThisFrame){
                foreach(Character character in Party.Instance.characters){
                    foreach(PlayerSkill skill in character.archetype.skills.ToArray()){
                        if(skill.active){
                            skill.GiveExp(500);
                        }
                    }
                }
            }

            if(Keyboard.current[Key.N].wasPressedThisFrame){
                SaveLoader.NewGame();
            }

            if(Keyboard.current[Key.S].wasPressedThisFrame){
                SaveLoader.SaveGame();
            }

            if(Keyboard.current[Key.L].wasPressedThisFrame){
                SaveLoader.LoadGame();
            }
        }
        #endif
    }
}
