using System.Collections.Generic;

namespace FinalInferno {
    public struct BattleChanges {
        public int levelChanges;
        public long xpGained;
        public Hero[] heroes;
        public List<SkillInfo>[] heroSkills;
        public List<PlayerSkill>[] skillReferences;
        public List<PlayerSkill>[] newSkills;

        public BattleChanges(Party party) {
            levelChanges = party.Level - BattleProgress.startingLevel;
            xpGained = party.XpCumulative - BattleProgress.startingCumulativeExp;
            heroes = new Hero[Party.Capacity];
            heroSkills = new List<SkillInfo>[Party.Capacity];
            skillReferences = new List<PlayerSkill>[Party.Capacity];
            newSkills = new List<PlayerSkill>[Party.Capacity];
            for (int heroIndex = 0; heroIndex < Party.Capacity; heroIndex++) {
                heroes[heroIndex] = BattleProgress.heroes[heroIndex];
                heroSkills[heroIndex] = new List<SkillInfo>();
                skillReferences[heroIndex] = new List<PlayerSkill>();
                newSkills[heroIndex] = new List<PlayerSkill>();
                SaveUsedSkills(heroIndex);
                SaveUnlockedSkills(heroIndex);
            }
        }

        private readonly void SaveUsedSkills(int heroIndex) {
            for (int skillIndex = 0; skillIndex < BattleProgress.skillReferences[heroIndex].Count; skillIndex++) {
                PlayerSkill skill = BattleProgress.skillReferences[heroIndex][skillIndex];
                SkillInfo startingSkillInfo = BattleProgress.startingHeroesSkillInfo[heroIndex][skillIndex];
                if (skill.XpCumulative == startingSkillInfo.xpCumulative)
                    continue;
                heroSkills[heroIndex].Add(startingSkillInfo);
                skillReferences[heroIndex].Add(skill);
            }
        }

        private readonly void SaveUnlockedSkills(int heroIndex) {
            for (int skillIndex = 0; skillIndex < BattleProgress.startingHeroesSkillInfo[heroIndex].Count; skillIndex++) {
                PlayerSkill skill = BattleProgress.skillReferences[heroIndex][skillIndex];
                SkillInfo startingSkillInfo = BattleProgress.startingHeroesSkillInfo[heroIndex][skillIndex];
                if (!skill.active || startingSkillInfo.active)
                    continue;
                newSkills[heroIndex].Add(skill);
            }
        }
    }
}
