using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Battle
{
    public class Console : MonoBehaviour
    {
        public static Console Instance { get; private set; }
        [SerializeField] private Text ConsoleText;

        private void Awake()
        {
            if (Instance != null)
                Destroy(this.gameObject);
            
            Instance = this;
        }

        void OnDestroy(){
            if(Instance == this){
                Instance = null;
            }
        }

        public void UpdateConsole()
        {
            if(BattleSkillManager.currentUser != null){
                if (BattleSkillManager.currentSkill == null){
                    ConsoleText.text = "<color=#" + ColorUtility.ToHtmlStringRGBA(BattleSkillManager.currentUser.unit.color) + ">"
                            + BattleSkillManager.currentUser.unit.name + "</color> cannot act";
                }else{
                    ConsoleText.text = "<color=#" + ColorUtility.ToHtmlStringRGBA(BattleSkillManager.currentUser.unit.color) + ">"
                            + BattleSkillManager.currentUser.unit.name + "</color> used " + BattleSkillManager.currentSkill.name;
                }
            }
        }
    }
}