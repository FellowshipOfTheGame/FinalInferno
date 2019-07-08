using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public static class SaveLoader{
        private const string fileName = "SaveFile";
        private static DataSaver<SaveFile> dataSaver = new DataSaver<SaveFile>(fileName, false);
        private static SaveFile saveFile = dataSaver.LoadData();
        public static int SaveSlot { get{ return saveFile.Slot; } set{ saveFile.Slot = value; }}
        public static bool AutoSave {
            get{
                if(saveFile != null){
                    return saveFile.autoSave;
                }
                return false;
            }
            set{
                if(saveFile != null && CanSaveGame){
                    saveFile.autoSave = value;
                }
            }
        }
        public static bool CanSaveGame{
            get{
                // Verifica se o numero de personagens no overwold é igual à capacidade da party
                // Consequentemente só vai permitir salvar o jogo no overworld
                int charCount = 0;
                for(int i = 0; i < CharacterOW.CharacterList.Count; i++){
                    if(CharacterOW.CharacterList[i] != null)
                        charCount++;
                }
                return (charCount == Party.Capacity);
            }
        }

        public static SavePreviewInfo[] PreviewAllSlots(){
            return saveFile.Preview();
        }

        public static SavePreviewInfo PreviewSlot(int slotNumber){
            return saveFile.Preview(slotNumber);
        }

        public static void SaveGame(){
            // Falha em salvar o jogo caso não esteja numa situação na qual isso é permitido
            if(!CanSaveGame)
                return;
            // Avalia a situação atual do jogo e salva todas as informações necessarias
            saveFile.Save();
            // Escreve as informações no arquivo de save
            dataSaver.SaveData(saveFile);
        }

        public static void LoadGame(){
            ResetGame();
            // Teoricamente não é necessario reler o arquivo, mas faremos isso como medida de segurança,
            // assim evitamos que a variavel saveFile tenha sido alterada de alguma maneira em runtime
            saveFile = dataSaver.LoadData();
            // Aplica a situação do save slot atual nos arquivos do jogo
            saveFile.Load();
            // Carrega a nova cena
            SceneLoader.LoadOWScene(Party.Instance.currentMap, true);
        }

        public static void NewGame(){
            // Reseta todas as informações do jogo para um estado inicial
            ResetGame();
            foreach(Character character in Party.Instance.characters){
                character.ResetCharacter();
                character.archetype.ResetHero();
            }

            Party.Instance.GiveExp(0);

            List<Quest> quests = AssetManager.LoadBundleAssets<Quest>();
            foreach(Quest quest in quests){
                // Desativa todas as flags de quest
                quest.active = false;
                foreach(string key in quest.events.Keys){
                    quest.events[key] = false;
                }
                // Checa se é a quest principal, e ativa a flag padrão
                if(quest.name == "MainQuest"){
                    quest.events["Default"] = true;
                }
            }
            // Carrega a cena inicial
            SceneLoader.LoadOWScene(Party.StartingMap);
        }

        public static void ResetGame(){
            Debug.Log("Vamo reseta entao!!!");
            Party.Instance.ResetParty();
        }
    }
}