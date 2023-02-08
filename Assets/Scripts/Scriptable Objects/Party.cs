using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;


namespace FinalInferno {
    [CreateAssetMenu(fileName = "Party", menuName = "ScriptableObject/Party")]
    public class Party : ScriptableObject, IDatabaseItem {
        public const string mainQuestAssetName = "MainQuest";
        private const string partyAssetName = "Party";
        private const string mainQuestChapter1Flag = "CerberusDead";
        private const string accumulatedXpColumnName = "XPAccumulated";
        private const string xpNextLevelColumnName = "XPNextLevel";
        private static Party instance = null;
        public static Party Instance {
            get {
                instance = instance != null ? instance : AssetManager.LoadAsset<Party>(partyAssetName);
                return instance;
            }
        }

        public const int Capacity = 4;

        public string currentMap = StaticReferences.FirstScene;
        [SerializeField] private int level;
        public int Level => level;
        public int ScaledLevel {
            get {
                int questParam = CalculateMainQuestProgress();
                int levelRange = questParam * 10;
                return Mathf.Clamp(level, levelRange, levelRange + 10);
            }
        }

        public int ScaledLevelTier => ((ScaledLevel - 1) % 5) + 1;

        private static int CalculateMainQuestProgress() {
            int questParam = 0;
            Quest mainQuest = AssetManager.LoadAsset<Quest>(mainQuestAssetName);
            if (mainQuest.GetFlag(mainQuestChapter1Flag))
                questParam++;
            return questParam;
        }

        [SerializeField, HideInInspector] private float xpMultiplier = 1f;
        public float XpMultiplier {
            get => xpMultiplier;
            set => xpMultiplier = value;
        }
        public long xp;
        public long xpNextLevel;
        public long XpCumulative => ((table == null) ? 0 : (xp + CurrentLevelCumulativeXp));
        private long CurrentLevelCumulativeXp => ((level <= 1) ? 0 : (table.Rows[level - 2].Field<long>(accumulatedXpColumnName)));
        private bool ShouldIncreaseLevel => xp >= xpNextLevel && level < Table.Rows.Count;

        public List<Character> characters = new List<Character>();
        public List<Quest> activeQuests = new List<Quest>();
        private Dictionary<Enemy, int> bestiary = new Dictionary<Enemy, int>();
        public ReadOnlyDictionary<Enemy, int> Bestiary => new ReadOnlyDictionary<Enemy, int>(bestiary);

        [SerializeField] private TextAsset partyXP;
        [SerializeField] private DynamicTable table;
        private DynamicTable Table {
            get {
                table ??= DynamicTable.Create(partyXP);
                return table;
            }
        }

        #region IDatabaseItem
        public void LoadTables() {
            table = DynamicTable.Create(partyXP);
        }

        public void Preload() {
            level = 0;
            xp = 0;
            xpNextLevel = 0;
            currentMap = StaticReferences.FirstScene;
        }
        #endregion

        public void RegisterKill(Enemy enemy) {
            if (bestiary.ContainsKey(enemy)) {
                bestiary[enemy]++;
            } else {
                bestiary.Add(enemy, 1);
            }
        }

        public void ReloadBestiary(BestiaryEntry[] entries) {
            bestiary.Clear();
            if (entries == null)
                return;
            foreach (BestiaryEntry entry in entries) {
                AddValidBestiaryEntry(entry);
            }
        }

        private void AddValidBestiaryEntry(BestiaryEntry entry) {
            if (string.IsNullOrEmpty(entry.monsterName))
                return;
            Enemy enemy = AssetManager.LoadAsset<Enemy>(entry.monsterName);
            bestiary.Add(enemy, entry.numberKills);
        }

        public int CalculateLevel(long cumulativeExp) {
            if (cumulativeExp <= 0)
                return 0;

            int calculatedLevel = 1;
            while (cumulativeExp > Table.Rows[calculatedLevel - 1].Field<long>(accumulatedXpColumnName)) {
                calculatedLevel++;
            }
            return calculatedLevel;
        }

        public void GiveExp(long value) {
            xp += value;
            bool leveledUp = ShouldIncreaseLevel;
            while (ShouldIncreaseLevel) {
                LevelUp();
            }
            if (leveledUp)
                ApplyLevelToCharacters();
        }

        private void LevelUp() {
            xp -= xpNextLevel;
            level++;
            xpNextLevel = Table.Rows[level - 1].Field<long>(xpNextLevelColumnName);
        }

        public void ApplyLevelToCharacters() {
            foreach (Character character in characters) {
                character.LevelUp(level);
            }
        }

        public void SaveOverworldPositions() {
            foreach (Character character in characters) {
                character.SaveOverworldPosition();
            }
        }

        public void LoadOverworldPositions() {
            foreach (Character character in characters) {
                character.LoadOverworldPosition();
            }
        }

        public void ResetParty() {
            ResetPartyProgress();
            LoadDefaultHeroAssets();
            foreach (Character character in characters) {
                character.ResetCharacter();
            }
            ResetPartyXP();
        }

        private void ResetPartyProgress() {
            bestiary.Clear();
            foreach (Quest quest in activeQuests) {
                quest.ResetQuest();
            }
            activeQuests.Clear();
            currentMap = StaticReferences.FirstScene;
        }

        private void LoadDefaultHeroAssets() {
            characters[0].archetype = AssetManager.LoadAsset<Hero>("Amidi");
            characters[1].archetype = AssetManager.LoadAsset<Hero>("Gregorim");
            characters[2].archetype = AssetManager.LoadAsset<Hero>("Herman");
            characters[3].archetype = AssetManager.LoadAsset<Hero>("Xander");
        }

        private void ResetPartyXP() {
            level = 0;
            xp = 0;
            xpNextLevel = 0;
            GiveExp(0);
        }
    }
}
