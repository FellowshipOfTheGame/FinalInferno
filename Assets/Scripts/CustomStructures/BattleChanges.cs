using System.Collections.Generic;

namespace FinalInferno {
    public struct BattleChanges {
        public int levelChanges;
        public long xpGained;
        public Hero[] heroes;
        // Ao contrario das listas de BattleProgress, essas listas contem apenas as habilidades que ganharam exp durante a batalha
        public List<SkillInfo>[] heroSkills;
        public List<PlayerSkill>[] skillReferences;
        public List<PlayerSkill>[] newSkills;

        public BattleChanges(Party party) { // Ex de uso no final da batalha: BattleChanges changes = new BattleChanges(Party.Instance)
            levelChanges = party.Level - BattleProgress.startingLevel;
            xpGained = party.XpCumulative - BattleProgress.startingCumulativeExp;
            heroes = new Hero[Party.Capacity];
            heroSkills = new List<SkillInfo>[Party.Capacity];
            skillReferences = new List<PlayerSkill>[Party.Capacity];
            newSkills = new List<PlayerSkill>[Party.Capacity];
            for (int i = 0; i < Party.Capacity; i++) {
                heroes[i] = BattleProgress.heroes[i];
                heroSkills[i] = new List<SkillInfo>();
                skillReferences[i] = new List<PlayerSkill>();
                newSkills[i] = new List<PlayerSkill>();

                for (int j = 0; j < BattleProgress.skillReferences[i].Count; j++) {
                    if (BattleProgress.skillReferences[i][j].XpCumulative != BattleProgress.startingHeroesSkillInfo[i][j].xpCumulative) {
                        heroSkills[i].Add(BattleProgress.startingHeroesSkillInfo[i][j]);
                        skillReferences[i].Add(BattleProgress.skillReferences[i][j]);
                    }
                }

                for (int j = 0; j < BattleProgress.startingHeroesSkillInfo[i].Count; j++) {
                    if (BattleProgress.skillReferences[i][j].active && !BattleProgress.startingHeroesSkillInfo[i][j].active) {
                        newSkills[i].Add(BattleProgress.skillReferences[i][j]);
                    }
                }
            }
        }
    }
}
