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
        private RectTransform rectTransform;

        [SerializeField] private float stepSize = 0.5f;
        public Vector2 CurrentOffset {get; private set;}
        [HideInInspector] public Vector2 defaultOffset = Vector2.zero;

        private bool showingTarget = false;

        void Awake(){
            rectTransform = GetComponent<RectTransform>();
        }

        public void Setup()
        {
            // TO DO: No lugar disso aqui talvez colocar um outline ou indicador na lista de unidades correspondente
            item.OnAct += SetTarget;
        }

        void Update(){
            // Posiciona o objeto com os sprites da unidade na sua posição desejada
            CurrentOffset = Vector2.zero;
            if(unit.gameObject != gameObject && rectTransform != null){
                Vector3 newPosition = rectTransform.TransformPoint(rectTransform.rect.center.x, 0f, 0f);
                newPosition += new Vector3(defaultOffset.x, defaultOffset.y, 2);
                if(BattleManager.instance.currentUnit == unit){
                    float xOffset = (unit.unit.IsHero)? stepSize : -stepSize;
                    newPosition.x += xOffset;
                    CurrentOffset = new Vector2(xOffset, 0f);
                }
                unit.transform.position = newPosition;
            }
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
