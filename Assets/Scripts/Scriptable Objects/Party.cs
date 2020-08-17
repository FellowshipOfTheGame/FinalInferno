using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System.Data;


namespace FinalInferno{
    //representa a equipe inteira do jogador
    [CreateAssetMenu(fileName = "Party", menuName = "ScriptableObject/Party")]
    public class Party : ScriptableObject, IDatabaseItem{
        private static Party instance = null;
        public static Party Instance{
            get{
                if(instance == null){
                    instance = AssetManager.LoadAsset<Party>("Party");
                }
                
                return instance;
            }
        }
        
        public const int Capacity = 4;

        public string currentMap = StaticReferences.FirstScene;
        private int level;
        public int Level { get => level; } //nivel da equipe(todos os personagens tem sempre o mesmo nivel)
        public int ScaledLevel{
            // Nível ajustado de acordo com o progresso na historia
            get{
                int questParam = 0;
                if(AssetManager.LoadAsset<Quest>("MainQuest").events["CerberusDead"]) questParam++;
                int levelRange = questParam * 10;

                return Mathf.Clamp(level, levelRange, levelRange+10);
            }
        }
        // Multiplicador para aplicar penalidades ou bonus de exp
        private float xpMultiplier = 1f;
        public float XpMultiplier{
            get{
                return xpMultiplier;
            }
            set{
                xpMultiplier = value;
            }
        }
        public long xp; //experiencia da equipe(todos os personagens tem sempre a mesma experiencia)
        public long xpNext; //experiencia necessaria para avancar de nivel
        public long XpCumulative{ get{ return ( (table == null)? 0 : (xp +  ((level <= 1)? 0 : (table.Rows[level-2].Field<long>("XPAccumulated"))) ) ); } }
        public List<Character> characters = new List<Character>(); //lista dos personagens que compoe a equipe 
        // Precisaria disso pra dar suporte a salvar o jogo em situações com menos personagems que o desejado mas
        // como talvez de mais trabalho vamo deixar comentado mesmo
        // public int Count{
        //     get{
        //         int i = 0;
        //         foreach(Character character in characters){
        //             if(character.isPresent)
        //                 i++;
        //         }
        //         return i;
        //     }
        // }
        public List<Quest> activeQuests = new List<Quest>(); // Lista das quests ativas
        private Dictionary<Enemy, int> bestiary = new Dictionary<Enemy, int>();
        public ReadOnlyDictionary<Enemy, int> Bestiary { get => (new ReadOnlyDictionary<Enemy, int>(bestiary)); }

        [SerializeField] private TextAsset partyXP;
        [SerializeField] private DynamicTable table = null;
        private DynamicTable Table {
            get {
                if(table == null)
                    table = DynamicTable.Create(partyXP);
                return table;
            }
        }

        public void LoadTables(){
            table = DynamicTable.Create(partyXP);
            level = 0;
            xp = 0;
            xpNext = 0;
            currentMap = StaticReferences.FirstScene;
        }

        public void Preload(){
            level = 0;
            xp = 0;
            xpNext = 0;
            currentMap = StaticReferences.FirstScene;
        }

        public void RegisterKill(Enemy enemy){
            if(bestiary.ContainsKey(enemy)){
                bestiary[enemy]++;
            }else{
                bestiary.Add(enemy, 1);
            }
        }

        public void ReloadBestiary(BestiaryEntry[] entries){
            bestiary.Clear();
            if(entries != null){
                foreach(BestiaryEntry entry in entries){
                    if(entry.monsterName != ""){
                        bestiary.Add(AssetManager.LoadAsset<Enemy>(entry.monsterName), entry.numberKills);
                    }
                }
            }
        }

        //faz todos os persoangens subirem de nivel
        public void LevelUp(){
            foreach (Character character in characters){
                character.LevelUp(level);
            }
        }

        // Função auxiliar para preview de level baseado na informação do save file
        public int GetLevel(long cumulativeExp){
            if(cumulativeExp <= 0)
                return 0;

            int _level = 1;
            while(cumulativeExp > Table.Rows[_level-1].Field<long>("XPAccumulated")){
                _level++;
            }
            return _level;
        }

        //Adiciona os pontos de experiência conquistado pelo jogador
        public bool GiveExp(long value){
            bool up = false;
            
            xp += value;

            //testa se os persoanagens subiram de nivel
            while(xp >= xpNext && level < Table.Rows.Count){
                // TO DO: Revisão de tabelas (level tem que ser user friendly)
                xp -= xpNext;
                level++;
                xpNext = Table.Rows[level-1].Field<long>("XPNextLevel");
                
                up = true;
            }
                

            if(up) LevelUp();

            return up;
        }

        public void ResetParty(){
            level = 0;
            xp = 0;
            xpNext = 0;
            Debug.Log("Party resetada");
            characters.Clear();
            bestiary.Clear();
            activeQuests.Clear();
            currentMap = StaticReferences.FirstScene;
            // Gambiarra mas provavelmente tem que ser hardcoded mesmo(?)
            // Talvez seja desnecessário ter que limpar a lista de character e achar de novo,
            // pode ser melhor só deixar as 4 referencias fixas e definir que elas não vão ser alteradas
            // Para os arquétipos pode ter uma configuração base e ela ser usada para construir
            characters.Add(AssetManager.LoadAsset<Character>("Character 1"));
            characters[characters.Count - 1].archetype = AssetManager.LoadAsset<Hero>("Amidi");
            characters.Add(AssetManager.LoadAsset<Character>("Character 2"));
            characters[characters.Count - 1].archetype = AssetManager.LoadAsset<Hero>("Gregorim");
            characters.Add(AssetManager.LoadAsset<Character>("Character 3"));
            characters[characters.Count - 1].archetype = AssetManager.LoadAsset<Hero>("Herman");
            characters.Add(AssetManager.LoadAsset<Character>("Character 4"));
            characters[characters.Count - 1].archetype = AssetManager.LoadAsset<Hero>("Xander");
            foreach(Character character in characters){
                character.ResetCharacter();
            }
            GiveExp(0);
        }
    }
}
