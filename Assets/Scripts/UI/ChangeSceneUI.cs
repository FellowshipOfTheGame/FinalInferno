using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI
{

    public class ChangeSceneUI : MonoBehaviour
    {
        public static bool isBattle = false;

        public static Sprite battleBG;
        public static AudioClip battleBGM;
        public static Enemy[] battleEnemies;

        public static string sceneName;
        public static Vector2 positionOnLoad;
        public static bool isCutscene;
        public static Fog.Dialogue.Dialogue selectedDialogue;

        private void ChangeSceneOnTrigger()
        {
            if (isBattle)
            {
                isBattle = false;
                SceneLoader.LoadBattleScene(battleEnemies, battleBG, battleBGM);
            }
            else if (!isCutscene)
                SceneLoader.LoadOWScene(sceneName, true, positionOnLoad);
            else
                SceneLoader.LoadCustscene(sceneName, selectedDialogue);
        }

        private void MainMenu()
        {
            SceneLoader.LoadMainMenu();
            //SceneLoader.LoadOWScene("DemoStart");
        }

        private void ReturnCheckpoint()
        {
            // TO DO: Carrega o jogo salvo no slot atual
            SaveLoader.LoadGame();
            //SceneLoader.LoadOWScene("DemoStart");
        }

        private void Continue()
        {
            foreach(Character character in Party.Instance.characters){
                character.hpCur = Mathf.Max(BattleManager.instance.GetBattleUnit(character.archetype).CurHP, 1);
            }
            SceneLoader.LoadOWScene(Party.Instance.currentMap, true);
        }
    }

}