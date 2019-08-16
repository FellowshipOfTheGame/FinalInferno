using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public static class BattleProgress {
        public static int startingLevel; // Nivel da party do inicio da batalha
        public static long startingExp; // Exp nao cumulativa da party no inicio da batalha
        public static long xpCumulative; // Exp cumulativa da party no inicio da batalha
        public static long xpToNextLevel; // Valor que a exp nao cumulativa precisa atingir para a party subir de nivel (calculada no inicio da batalha)
        public static Hero[] heroes = new Hero[Party.Capacity]; // Referencias pros heroes
        public static List<SkillInfo>[] heroSkills = new List<SkillInfo>[Party.Capacity]; // Info das skills de cada hero no inicio da batalha
        private static List<PlayerSkill>[] skillReferences = new List<PlayerSkill>[Party.Capacity]; // Referencia para cada skill do hero (contem info atualizada)

        public struct BattleChanges{
            public int levelChanges; // Quantos levels a party ganhou
            public long xpGained; // Total de exp que a party ganhou
            public Hero[] heroes; // Referencias pros heroes
            // Ao contrario das listas de BattleProgress, essas listas contem apenas as habilidades que ganharam exp durante a batalha
            public List<SkillInfo>[] heroSkills; // Info das skills de cada hero no inicio da batalha
            public List<PlayerSkill>[] skillReferences; // Referencia para cada skill do hero (contem info atualizada)
            public List<PlayerSkill>[] newSkills; // Skills que cada hero desbloqueou durante a batalha

            public BattleChanges(Party party){ // Ex de uso no final da batalha: BattleChanges changes = new BattleChanges(Party.Instance)
                levelChanges = party.level - BattleProgress.startingLevel;
                xpGained = party.XpCumulative - BattleProgress.xpCumulative;
                heroes = new Hero[Party.Capacity];
                heroSkills = new List<SkillInfo>[Party.Capacity];
                skillReferences = new List<PlayerSkill>[Party.Capacity];
                newSkills = new List<PlayerSkill>[Party.Capacity];
                for(int i = 0; i < Party.Capacity; i++){
                    heroes[i] = BattleProgress.heroes[i];
                    Debug.Log(heroes[i]);
                    heroSkills[i] = new List<SkillInfo>();
                    skillReferences[i] = new List<PlayerSkill>();
                    newSkills[i] = new List<PlayerSkill>();

                    for(int j = 0; j < BattleProgress.skillReferences[i].Count; j++){
                        if(BattleProgress.skillReferences[i][j].XpCumulative != BattleProgress.heroSkills[i][j].xpCumulative){
                            heroSkills[i].Add(BattleProgress.heroSkills[i][j]);
                            skillReferences[i].Add(BattleProgress.skillReferences[i][j]);
                        }
                    }

                    foreach(PlayerSkill skill in heroes[i].skills){
                        if(skill.active && !BattleProgress.skillReferences[i].Contains(skill)){
                            newSkills[i].Add(skill);
                        }
                    }
                }
            }
        }

        private static int currentIndex = 0;

        public static void ResetInfo(Party party){
            startingLevel = party.level;
            startingExp = party.xp;
            xpToNextLevel = party.xpNext;
            xpCumulative = party.XpCumulative;
            for(int i = 0; i < Party.Capacity; i++){
                heroes[i] = null;
                heroSkills[i] = new List<SkillInfo>();
                skillReferences[i] = new List<PlayerSkill>();
            }
        }

        public static void addHeroSkills(Hero hero){
            heroes[currentIndex] = hero;
            foreach(PlayerSkill skill in hero.skills){
                addSkill(skill);
            }
            currentIndex++;
        }

        private static void addSkill(PlayerSkill skill){
            if(skill.active){
                heroSkills[currentIndex].Add(new SkillInfo(skill));
                skillReferences[currentIndex].Add(skill);
            }
        }
    }
}