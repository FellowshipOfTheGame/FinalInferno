using FinalInferno.UI.Victory;

namespace FinalInferno.UI.AII
{
    /// <summary>
	/// Item da lista de skills.
	/// </summary>
    public class VictorySkillListItem : SkillListItem
    {
        /// <summary>
        /// Atualiza a descrição da skill no menu.
        /// </summary>
        private void UpdateSkillDescription()
        {
            if (skill == null)
                skill = GetComponent<UpdatedSkill>().thisSkill;

            // skillList.UpdateSkillDescription(skill);
        }

        /// <summary>
        /// Atualiza a posição do content das skills.
        /// </summary>
        private void ClampSkillContent()
        {
            // skillList.ClampSkillContent(rect);
        }
    }

}
