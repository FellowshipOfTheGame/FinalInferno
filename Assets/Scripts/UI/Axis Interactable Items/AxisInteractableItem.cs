using UnityEngine;
using UnityEngine.EventSystems;

namespace FinalInferno.UI.AII {
    /// <summary>
	/// Componente que implementa um sistema de itens que podem ser selecionados por atalhos do teclado, 
    /// como se fosse um eixo.
	/// </summary>
    public class AxisInteractableItem : MonoBehaviour {
        /// <summary>
        /// Referência que se mostra visível quando o item está ativo.
        /// </summary>
        [SerializeField] private UIBehaviour activeReference = null;
        public UIBehaviour ActiveReference {
            get => activeReference;
            set {
                if (activeReference == null) {
                    activeReference = value;
                }
            }
        }

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
        /// Referência para o item da esquerda.
        /// </summary>
        public AxisInteractableItem leftItem;

        /// <summary>
        /// Referência para o item da direita.
        /// </summary>
        public AxisInteractableItem rightItem;

        /// <summary>
        /// Referência para o item de baixo.
        /// </summary>
        public AxisInteractableItem downItem;

        /// <summary>
        /// Referência para o item de cima.
        /// </summary>
        public AxisInteractableItem upItem;

        public virtual void Awake() {
            OnEnter += EnableReference;
            OnExit += DisableReference;
        }

        /// <summary>
        /// Ativa o item.
        /// </summary>
        public void Enter() {
            if (OnEnter != null) {
                OnEnter();
            }
        }

        /// <summary>
        /// Desativa o item.
        /// </summary>
        public void Exit() {
            if (OnExit != null) {
                OnExit();
            }
        }

        /// <summary>
        /// Executa a ação do item.
        /// </summary>
        public void Act() {
            if (OnAct != null) {
                OnAct();
            }
        }

        /// <summary>
        /// Ativa a referência do item.
        /// </summary>
        public void EnableReference() {
            if (activeReference) {
                activeReference.enabled = true;
            }
        }

        /// <summary>
        /// Desativa a referência do item.
        /// </summary>
        public void DisableReference() {
            if (activeReference) {
                activeReference.enabled = false;
            }
        }
    }

}
