using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.AII
{
    /// <summary>
	/// Componente que implementa um sistema de itens que podem ser selecionados por atalhos do teclado, 
    /// como se fosse um eixo.
	/// </summary>
    public class AxisInteractableItem : MonoBehaviour
    {
        /// <summary>
        /// Referência que se mostra visível quando o item está ativo.
        /// </summary>
        [SerializeField] private GameObject activeReference;

        /// <summary>
        /// Base para as ações do item.
        /// </summary>
        public delegate void ItemAction();

        /// <summary>
        /// Evento chamado toda vez que o item é ativado.
        /// </summary>
        public event ItemAction OnEnter;

        /// <summary>
        /// Evento chamado toda vez que o item é desativado.
        /// </summary>
        public event ItemAction OnExit;

        /// <summary>
        /// Evento que representa as ações do item, quando chamado.
        /// </summary>
        public event ItemAction OnAct;

        /// <summary>
        /// Referência para o item da posição negativa (esquerda/baixo).
        /// </summary>
        public AxisInteractableItem negativeItem;

        /// <summary>
        /// Referência para o item da posição positiva (direita/cima).
        /// </summary>
        public AxisInteractableItem positiveItem;

        void Awake()
        {
            OnEnter += EnableGO;
            OnExit += DisableGO;
        }

        /// <summary>
        /// Ativa o item.
        /// </summary>
        public void Enter()
        {
            if (OnEnter != null) OnEnter();
        }

        /// <summary>
        /// Desativa o item.
        /// </summary>
        public void Exit()
        {
            if (OnExit != null) OnExit();
        }

        /// <summary>
        /// Executa a ação do item.
        /// </summary>
        public void Act()
        {
            if (OnAct != null)
            {
                Debug.Log(OnAct.GetInvocationList());
                foreach(System.Delegate del in OnAct.GetInvocationList())
                {
                    Debug.Log(del.Method.Name);
                }
                OnAct();
            }
        }

        /// <summary>
        /// Ativa a referência do item.
        /// </summary>
        protected void EnableGO()
        {
            activeReference.SetActive(true);
        }

        /// <summary>
        /// Desativa a referência do item.
        /// </summary>
        protected void DisableGO()
        {
            activeReference.SetActive(false);
        }
    }

}
