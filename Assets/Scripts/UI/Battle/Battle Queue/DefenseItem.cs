using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.Battle.QueueMenu
{
    /// <summary>
	/// Item que ativa a skill de defesa.
	/// </summary>
    public class DefenseItem : SkillItem
    {
        [SerializeField] private UI.Battle.SkillMenu.SkillList skillListManager;
        [SerializeField] private Animator consoleAnim;
        new void Awake(){
            item.OnEnter += GetSkill;
            base.Awake();
        }
        void GetSkill()
        {
            skill = (BattleManager.instance.currentUnit != null)? BattleManager.instance.currentUnit.Unit.defenseSkill : null;
            if(skill){
                skillListManager.UpdateSkillDescription(skill);
                // Mostra o console e pede preview de skill
                if(consoleAnim){
                    consoleAnim.SetTrigger("ShowConsole");
                    consoleAnim.SetTrigger("ShowSkillDetails");
                }
            }
        }
    }

}
