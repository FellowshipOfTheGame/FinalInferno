﻿using System.Collections;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "EndChapterDialogue", menuName = "ScriptableObject/DialogueSystem/DialogueEndChapter")]
    public class DialogueEndChapter : Fog.Dialogue.Dialogue
    {
        [SerializeField] private FinalInferno.UI.FSM.ButtonClickDecision decision;
        
        public override void AfterDialogue(){
            base.AfterDialogue();
            Fog.Dialogue.DialogueHandler.instance.StartCoroutine(WaitToGoBack());
        }

        public IEnumerator WaitToGoBack(){
            yield return new WaitForSecondsRealtime(1.5f);
            BackToMainMenu();
        }

        public void BackToMainMenu(){
            SceneLoader.LoadMainMenu();
        }
    }

}