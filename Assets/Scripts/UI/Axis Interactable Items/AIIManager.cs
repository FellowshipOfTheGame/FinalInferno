using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.AII
{
    /// <summary>
    /// Componente que representa um grupo de itens que podem ser selecionados por atalhos do teclado.
    /// </summary>
    public class AIIManager : MonoBehaviour
    {
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

        /// <summary>
        /// Estado do gerenciador.
        /// </summary>
        public bool active;

        private bool enableInput = true;

        [SerializeField] private bool interactable;


        void Awake(){
            currentItem = null;
        }

        void Start()
        {
            active = false;
        }

        void Update()
        {
            if (active)
            {
                // Valida e altera o item ativado se necessário.
                Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                if (direction == Vector2.up)
                {
                    if(currentItem!= null && enableInput)
                        ChangeItem(currentItem.upItem);
                    enableInput = false;
                }
                else if (direction == Vector2.down)
                {
                    if(currentItem!= null && enableInput)
                        ChangeItem(currentItem.downItem);
                    enableInput = false;
                }
                else if (direction == Vector2.left)
                {
                    if(currentItem!= null && enableInput)
                        ChangeItem(currentItem.leftItem);
                    enableInput = false;
                }
                else if (direction == Vector2.right)
                {
                    if(currentItem!= null && enableInput)
                        ChangeItem(currentItem.rightItem);
                    enableInput = false;
                }
                else
                    enableInput = true;

                // Executa a ação do item se o eixo for ativado.
                if (interactable && Input.GetAxisRaw(activatorAxis) != 0)
                {
                    currentItem.Act();
                }
            }
        }

        /// <summary>
        /// Ativa o gerenciador e o item atual.
        /// </summary>
        public void Active()
        {
            active = true;
            currentItem = firstItem;
            if (currentItem != null)
            {
                currentItem.Enter();
            }
        }

        /// <summary>
        /// Desativa o item atual e o gerenciador.
        /// </summary>
        public void Deactive()
        {
            if (currentItem != null)
            {
                currentItem.Exit();
            }
            active = false;
        }

        public void ClearItems(){
            if(currentItem != null){
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
        private void ChangeItem(AxisInteractableItem nextItem)
        {
            if (currentItem != null && nextItem != null)
            {
                currentItem.Exit();
                currentItem = nextItem;
                currentItem.Enter();
            }
        }
    }

}
