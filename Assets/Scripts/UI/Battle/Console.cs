using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Battle
{
    public class Console : MonoBehaviour
    {
        public static Console Instance;
        [SerializeField] private Text ConsoleText;

        private void Awake()
        {
            if (Instance != null)
                Destroy(this.gameObject);
            
            Instance = this;
        }

        public void UpdateConsole()
        {
            if (BattleSkillManager.currentSkill == null) return;
            ConsoleText.text = "<color=#" + ColorUtility.ToHtmlStringRGBA(BattleSkillManager.currentUser.unit.color) + ">"
                     + BattleSkillManager.currentUser.unit.name + "</color> used " + BattleSkillManager.currentSkill.name;
        }
    }
}