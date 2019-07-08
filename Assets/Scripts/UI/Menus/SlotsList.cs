using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FinalInferno.UI.AII;
using FinalInferno.UI.FSM;

namespace FinalInferno.UI.Saves
{
    public class SlotsList : MonoBehaviour
    {
        [Header("Prefab")]
        /// <summary>
        /// Objeto template para um item de slot que será mostrado no menu.
        /// </summary>
        [SerializeField] private GameObject slotObject;

        [Header("Content reference")]
        /// <summary>
        /// Referência para o local onde todas os slots serão mostrados para o jogador.
        /// </summary>
        [SerializeField] private RectTransform slotsContent;

        [Header("Manager")]
        /// <summary>
        /// Controlador dos itens de slot.
        /// </summary>
        [SerializeField] private AIIManager manager;

        [Header("Click decision")]
        /// <summary>
        /// Decisão que será chamada quando a tecla de ativação for pressionada.
        /// </summary>
        [SerializeField] private ButtonClickDecision clickDecision;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            //UpdateSlotsContent(SaveLoader.PreviewAllSlots());
        }

        public void UpdateSlotsContent(List<SavePreviewInfo> slots)
        {
            // Deleta todos os itens previamente alocados no content
            foreach (Slot element in slotsContent.GetComponentsInChildren<Slot>())
            {
                Destroy(element.gameObject);
            }

            // Variável auxiliar para a ordenação dos itens
            AxisInteractableItem lastItem = null;

            // Passa por todas as skills da lista, adicionando as ativas no menu e as ordenando
            foreach (SavePreviewInfo slot in slots)
            {
                // Instancia um novo item e o coloca no content
                GameObject newSlot = Instantiate(slotObject);
                newSlot.GetComponent<Slot>().LoadSlot(slot);
                newSlot.transform.SetParent(slotsContent);

                // Adiciona a decisão de clique no item criado
                ClickableItem newClickableItem = newSlot.GetComponent<ClickableItem>();
                newClickableItem.BCD = clickDecision;

                // Ordena o item na lista
                AxisInteractableItem newItem = newSlot.GetComponent<AxisInteractableItem>();
                if (lastItem != null)
                {
                    newItem.positiveItem = lastItem;
                    lastItem.negativeItem = newItem;
                }
                else
                {
                    manager.firstItem = newItem;
                }
                lastItem = newItem;
            }
        }
    }
}