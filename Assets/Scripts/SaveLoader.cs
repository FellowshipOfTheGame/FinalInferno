using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public static class SaveLoader{
        private const string fileName = "SaveFile";
        private static DataSaver<SaveFile> dataSaver = new DataSaver<SaveFile>(fileName, false);
        private static SaveFile saveFile = dataSaver.LoadData();
        public static SaveFile SaveFile { get{ return saveFile; } }

        public static void SaveGame(){
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
            //SceneLoader.LoadOWScene(Party.Instance.currentMap, true);
        }

        public static void NewGame(){
            // Reseta todas as informações do jogo para um estado inicial
            ResetGame();
            Party.Instance.GiveExp(0);
            // Carrega a cena inicial
            //SceneLoader.LoadOWScene(Party.Instance.currentMap, true);
        }

        public static void ResetGame(){
            Debug.Log("Vamo reseta entao!!!");
            Party.Instance.ResetParty();
           
            foreach(Character character in Party.Instance.characters){
                character.ResetCharacter();
                character.archetype.ResetHero();
            }
        }
    }
}