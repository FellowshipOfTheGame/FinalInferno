using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System.Data;


namespace FinalInferno{
    //representa a equipe inteira do jogador
    [CreateAssetMenu(fileName = "Party", menuName = "ScriptableObject/Party", order = 0)]
    public class Party : ScriptableObject{
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
        public int level; //nivel da equipe(todos os personagens tem sempre o mesmo nivel)
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

        [SerializeField] private TextAsset PartyXP;
        [SerializeField] private DynamicTable table;
        private DynamicTable Table {
            get {
                if(table == null)
                    table = DynamicTable.Create(PartyXP);
                return table;
            }
        }

        public void Awake(){
            table = null;
            table = DynamicTable.Create(PartyXP);
            level = 0;
            xp = 0;
            xpNext = 0;
            currentMap = StaticReferences.FirstScene;
            //Debug.Log("Iniciou");
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
            //table = DynamicTable.Create(PartyXP);
            bool up = false;
            
            xp += value;
            //Debug.Log("Deu xp");

            //testa se os persoangens subiram de nivel
            //Debug.Log(xp + ">=" + xpNext + "?");
            while(xp >= xpNext && level < Table.Rows.Count){
                //Debug.Log("claro que upo");
                // TO DO: Revisão de tabelas (level tem que ser user friendly)
                xp -= xpNext;
                level++;
                xpNext = Table.Rows[level-1].Field<long>("XPNextLevel");
                //Debug.Log("agora xp pro proximo level eh: " + xpNext);
                
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
            // Gambiarra
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
