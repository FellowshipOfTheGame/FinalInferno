using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.AII
{
    /// <summary>
	/// Referência aos eixos disponíveis.
	/// </summary>
    public enum AxisEnum
    {
        Horizontal,
        Vertical
    }

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
        /// Orientação do eixo (horizontal/vertical), define as teclas que precisam ser pressionadas 
        /// para que haja mudança de item selecionado.
        /// </summary>
        public AxisEnum orientation;

        /// <summary>
        /// Eixo que precisa ser ativado para executar a ação do item ativado.
        /// </summary>
        [SerializeField] private string activatorAxis;

        /// <summary>
        /// Tempo que passou desde o último clique.
        /// </summary>
        private float _time;

        /// <summary>
        /// Tempo mínimo entre dois cliques.
        /// </summary>
        public float timeBetweenClicks;

        /// <summary>
        /// Estado do gerenciador.
        /// </summary>
        public bool active;

        [SerializeField] private bool interactable;

        void Start()
        {
            _time = 0f;
            active = false;
        }

        void Update()
        {
            if (active)
            {
                // Apenas verifica interação de movimento após o tempo mínimo.
                _time += Time.deltaTime;
                if (_time >= timeBetweenClicks)
                {
                    // Valida e altera o item ativado se necessário.
                    float direction = Input.GetAxis(orientation.ToString());
                    if (direction > .1f)
                    {
                        ChangeItem(currentItem.positiveItem);
                        _time = 0f;
                    }
                    else if (direction < -.1f)
                    {
                        ChangeItem(currentItem.negativeItem);
                        _time = 0f;
                    }
                }

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
        public void Desactive()
        {
            if (currentItem != null)
            {
                currentItem.Exit();
            }
            active = false;
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
