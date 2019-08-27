using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "NewBattleDialogue", menuName = "ScriptableObject/DialogueSystem/DialogueBeforeBattle")]
    public class DialogueBeforeBattle : DialogueEventTrigger
    {
        [SerializeField] private Sprite battleBG;
        [SerializeField] private AudioClip battleBGM;
        [SerializeField] private Enemy[] battleEnemies;
        
        public override void AfterDialogue(){
            base.AfterDialogue();
            SceneLoader.LoadBattleScene(battleEnemies, battleBG, battleBGM);
        }
    }
}
