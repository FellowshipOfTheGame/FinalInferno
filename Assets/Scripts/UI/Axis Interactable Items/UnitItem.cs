using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.Battle;
using FinalInferno.UI.FSM;
using UnityEngine.UI;

namespace FinalInferno.UI.AII
{
    /// <summary>
	/// Item da lista de efeitos.
	/// </summary>
    public class UnitItem : MonoBehaviour
    {
        /// <summary>
        /// Referência ao efeito do item.
        /// </summary>
        public BattleUnit unit;

        /// <summary>
        /// Referência ao item da lista.
        /// </summary>
        [SerializeField] private AxisInteractableItem item;
        [SerializeField] private HeroInfoLoader infoLoader;

        public LayoutElement layout;

        private bool showingTarget = false;

        public void Setup()
        {
            if(unit.unit.IsHero){
                item.OnEnter += UpdateHeroContent;
                item.OnExit += ResetHeroContent;
            }else{
                item.OnEnter += UpdateEnemyContent;
                item.OnExit += ResetEnemyContent;
            }
            item.OnAct += SetTarget;
        }

        private void SetTarget()
        {
            // Debug.Log("Setting target: " + unit.unit.name);
            BattleSkillManager.currentTargets.Clear();
            BattleSkillManager.currentTargets.Add(unit);
        }

        private void UpdateEnemyContent()
        {
            BattleManager.instance.enemyContent.ShowEnemyInfo(unit);
        }

        private void UpdateHeroContent(){
            infoLoader.Info.LoadInfo(unit);
        }

        private void ResetEnemyContent()
        {
            BattleManager.instance.enemyContent.ShowAllLives();
        }

        private void ResetHeroContent(){
            // Feito atraves da maquina de estados, ação GetHeroesLivesTrigger
        }

        public void ShowThisAsATarget()
        {
            if(showingTarget)
                item.DisableReference();
            else
                item.EnableReference();
            showingTarget = !showingTarget;
        }

    }

}
