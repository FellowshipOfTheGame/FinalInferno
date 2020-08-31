using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public static class SaveLoader{
        private const string fileName = "SaveFile";
        private static DataSaver<SaveFile> dataSaver = new DataSaver<SaveFile>(fileName, true);
        private static SaveFile saveFile = dataSaver.LoadData();
        public static int SaveSlot { get{ return saveFile.Slot; } set{ saveFile.Slot = value; } }
        public static bool AutoSave {
            get{
                if(saveFile != null){
                    return saveFile.autoSave;
                }
                return false;
            }
            set{
                if(saveFile != null){
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
            // // Failsafe para garantir que saveFile não seja null
            // if(saveFile == null)
            //     saveFile = new SaveFile();
            // Falha em salvar o jogo caso não esteja numa situação na qual isso é permitido
            if(!CanSaveGame){
                Debug.Log("Attempted to save the game when it shouldn't be possible");
                return;
            }
            // // Teoricamente não é necessario reler o arquivo, mas faremos isso como medida de segurança,
            // // assim evitamos que a variavel saveFile tenha sido alterada de alguma maneira em runtime
            // if(!CheckIntegrity()){
            //     // BUG: Crashando o jogo no primeiro save, quando o arquivo não existe
            //     Debug.LogError("Save File has been altered during gameplay");
            //     Application.Quit();
            //     return;
            // }
            // Avalia a situação atual do jogo e salva todas as informações necessarias
            Debug.Log("Saving game...");
            saveFile.Save();
            // Escreve as informações no arquivo de save
            dataSaver.SaveData(saveFile);
            Debug.Log("Game saved (...probably)");
        }

        public static void LoadGame(){
            if(!saveFile.HasCheckPoint){
                NewGame();
            }else{
                ResetGame();
                // // Teoricamente não é necessario reler o arquivo, mas faremos isso como medida de segurança,
                // // assim evitamos que a variavel saveFile tenha sido alterada de alguma maneira em runtime
                // if(!CheckIntegrity()){
                //     Debug.LogError("Save File has been altered during gameplay");
                //     Application.Quit();
                //     return;
                // }
                // Aplica a situação do save slot atual nos arquivos do jogo
                saveFile.Load();
                // Carrega a nova cena
                SceneLoader.LoadOWScene(Party.Instance.currentMap, true);
            }
        }

        public static void NewGame(){
            // // Failsafe para garantir que saveFile não seja null
            // if(saveFile == null)
            //     saveFile = new SaveFile();
            // Reseta todas as informações do jogo para um estado inicial
            ResetGame();

            Quest mainQuest = AssetManager.LoadAsset<Quest>("MainQuest");
            mainQuest.StartQuest(true);
            
            //Debug.Log("Default flag = " + mainQuest.events["Default"]);
            // Carrega a cena inicial como cutscene
            SceneLoader.LoadCustscene(StaticReferences.FirstScene, StaticReferences.FirstDialogue);
        }

        public static void StartDemo(){
            // // Failsafe para garantir que saveFile não seja null
            // if(saveFile == null)
            //     saveFile = new SaveFile();
            // Reseta todas as informações do jogo para um estado inicial
            ResetGame();

            Quest mainQuest = AssetManager.LoadAsset<Quest>("AdventurerQuest");
            mainQuest.StartQuest(true);
            mainQuest = AssetManager.LoadAsset<Quest>("MainQuest");
            mainQuest.StartQuest(true);
            
            //Debug.Log("Default flag = " + mainQuest.events["Default"]);
            // Carrega a cena inicial como cutscene
            SceneLoader.LoadOWScene("Demo");
        }

        private static bool CheckIntegrity(){
            SaveFile currentSaved = dataSaver.LoadData();
            return saveFile.Equals(currentSaved);
        }

        public static void ResetGame(){
            // Debug.Log("Vamo reseta entao!!!");
            Party.Instance.ResetParty();
        }
    }
}