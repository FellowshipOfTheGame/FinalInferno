using UnityEngine;
using UnityEngine.InputSystem;

namespace FinalInferno.UI.AII {
    /// <summary>
    /// Componente que representa um grupo de itens que podem ser selecionados por atalhos do teclado.
    /// </summary>
    public class AIIManager : MonoBehaviour {
        /// <summary>
        /// Item atualmente ativado.
        /// </summary>
        public AxisInteractableItem currentItem;

        /// <summary>
        /// Primeiro item da lista.
        /// </summary>
        public AxisInteractableItem firstItem;

        /// <summary>
        /// Último item da lista.
        /// </summary>
        public AxisInteractableItem lastItem;

        /// <summary>
        /// Eixo que precisa ser ativado para executar a ação do item ativado.
        /// </summary>
        [SerializeField] private string activatorAxis;
        [SerializeField] private InputActionReference movementAction;
        [SerializeField] private InputActionReference activationAction;

        /// <summary>
        /// Estado do gerenciador.
        /// </summary>
        protected bool active;
        public bool IsActive => active;

        private bool enableInput = true;

        [SerializeField] private bool interactable;
        public bool Interactable => interactable;

        [SerializeField] protected AudioSource AS;


        public void Awake() {
            currentItem = null;
        }

        public void Start() {
            active = false;
        }

        public void Update() {
            if (active) {
                // Valida e altera o item ativado se necessário.
                // Vector2 direction = new Vector2(UnityEngine.Input.GetAxisRaw("Horizontal"), UnityEngine.Input.GetAxisRaw("Vertical"));
                Vector2 direction = movementAction.action.ReadValue<Vector2>();
                if (direction == Vector2.up) {
                    if (currentItem != null && enableInput) {
                        ChangeItem(currentItem.upItem);
                    }

                    enableInput = false;
                } else if (direction == Vector2.down) {
                    if (currentItem != null && enableInput) {
                        ChangeItem(currentItem.downItem);
                    }

                    enableInput = false;
                } else if (direction == Vector2.left) {
                    if (currentItem != null && enableInput) {
                        ChangeItem(currentItem.leftItem);
                    }

                    enableInput = false;
                } else if (direction == Vector2.right) {
                    if (currentItem != null && enableInput) {
                        ChangeItem(currentItem.rightItem);
                    }

                    enableInput = false;
                } else {
                    enableInput = true;
                }

                // Executa a ação do item se o eixo for ativado.
                if (interactable && activationAction && activationAction.action.triggered) {
                    currentItem.Act();
                }
            }
        }

        /// <summary>
        /// Ativa o gerenciador e o item atual.
        /// </summary>
        public virtual void Active() {
            active = true;
            currentItem = firstItem;
            if (currentItem != null) {
                currentItem.Enter();
            }
        }

        /// <summary>
        /// Desativa o item atual e o gerenciador.
        /// </summary>
        public virtual void Deactive() {
            if (currentItem != null) {
                currentItem.Exit();
            }
            active = false;
        }

        public void SetFocus(bool isActive) {
            active = isActive;
        }

        public void SetInteractable(bool value) {
            interactable = value;
        }

        public void ClearItems() {
            if (currentItem != null) {
                currentItem.Exit();
            }
            currentItem = null;
            firstItem = null;
            lastItem = null;
        }

        /// <summary>
        /// Muda o item atualmente ativado para o próximo.
        /// </summary>
        /// <param name="nextItem"> Próximo item a ser ativado, se existir. </param>
        protected void ChangeItem(AxisInteractableItem nextItem) {
            if (currentItem != null && nextItem != null) {
                currentItem.Exit();
                currentItem = nextItem;
                currentItem.Enter();
                if (AS) {
                    AS.Play();
                }
            }
        }
    }

}
