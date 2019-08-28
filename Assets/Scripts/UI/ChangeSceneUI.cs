using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI
{

    public class ChangeSceneUI : MonoBehaviour
    {
        private void MainMenu()
        {
            //SceneLoader.LoadMainMenu();
            SceneLoader.LoadOWScene("DemoStart");
        }

        private void ReturnCheckpoint()
        {
            // TO DO: Carrega o jogo salvo no slot atual
            // SaveLoader.LoadGame();
            SceneLoader.LoadOWScene("DemoStart");
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