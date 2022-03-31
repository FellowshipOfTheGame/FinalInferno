using System.Collections.Generic;

namespace FinalInferno {
    // Struct a ser usada para visualizar os saveSlots
    public struct SavePreviewInfo {
        public int level;
        public string mapName;
        public List<Hero> heroes;
        public SavePreviewInfo(SaveInfo save) {
            // Listas são null por default, portanto um save com uma lista nula não foi inicializado
            if (save.xpParty <= 0) {
                level = 0;
                mapName = "";
                heroes = null;
            } else {
                level = Party.Instance.CalculateLevel(save.xpParty);
                mapName = save.mapName;
                heroes = new List<Hero>();
                foreach (string heroName in save.archetype) {
                    heroes.Add(AssetManager.LoadAsset<Hero>(heroName));
                }
            }
        }
    }

}