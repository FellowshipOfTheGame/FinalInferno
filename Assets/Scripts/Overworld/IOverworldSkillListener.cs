namespace FinalInferno{
    public interface IOverworldSkillListener
    {
        void ActivatedSkill(OverworldSkill skill);
        void DeactivatedSkill(OverworldSkill skill);
    }
}
