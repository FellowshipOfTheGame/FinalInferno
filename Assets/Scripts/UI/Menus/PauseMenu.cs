using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno{
    public class PauseMenu : MonoBehaviour
    {
        public static PauseMenu Instance { get; private set; } = null;
        public static bool IsPaused { get; private set; } = false;

        [SerializeField] FinalInferno.UI.BestiaryMenu bestiaryMenu;

        private void Awake()
        {
            if(Instance == null){
                Instance = this;
            }else{
                Destroy(this);
            }

            IsPaused = false;
        }

        public void ToggleBestiary(){
            bestiaryMenu?.ToggleBestiary();
        }

        public void ChangePauseState()
        {
            IsPaused = !IsPaused;
            CharacterOW.PartyCanMove = !IsPaused;
        }

        public void OnDestroy(){
            if(Instance == this){
                Instance = null;
            }
        }
    }
}