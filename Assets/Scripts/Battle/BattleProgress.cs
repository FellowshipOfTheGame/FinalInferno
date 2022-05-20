using System.Collections.Generic;

namespace FinalInferno {
    public static class BattleProgress {
        public static int startingLevel;
        public static long startingExp;
        public static long startingCumulativeExp;
        public static long startingExpToNextLevel;
        public static Hero[] heroes = new Hero[Party.Capacity];
        public static List<SkillInfo>[] startingHeroesSkillInfo = new List<SkillInfo>[Party.Capacity];
        public static List<PlayerSkill>[] skillReferences = new List<PlayerSkill>[Party.Capacity];
        private static int currentIndex = 0;

        public static void ResetInfo(Party party) {
            currentIndex = 0;
            startingLevel = party.Level;
            startingExp = party.xp;
            startingExpToNextLevel = party.xpNextLevel;
            startingCumulativeExp = party.XpCumulative;
            for (int index = 0; index < Party.Capacity; index++) {
                heroes[index] = null;
                startingHeroesSkillInfo[index] = new List<SkillInfo>();
                skillReferences[index] = new List<PlayerSkill>();
            }
        }

        public static void AddHeroSkills(Hero hero) {
            heroes[currentIndex] = hero;
            foreach (PlayerSkill skill in hero.skills) {
                AddSkill(skill);
            }
            currentIndex++;
        }

        private static void AddSkill(PlayerSkill skill) {
            if (!skill.active && skill.Type != SkillType.Active)
                return;
            startingHeroesSkillInfo[currentIndex].Add(new SkillInfo(skill));
            skillReferences[currentIndex].Add(skill);
        }
    }
}