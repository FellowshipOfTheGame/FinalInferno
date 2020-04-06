using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
        [SerializeField] private GameObject[] slotObjects;

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

        [SerializeField] private LoadPreviews loadPreviewsActions;

        [Header("Autosave info")]
        [SerializeField] private TextMeshProUGUI textMesh;

        void Update(){
            if(textMesh != null){
                string autoSaveStatus = (SaveLoader.AutoSave)? "on" : "off";
                string currentSlot = "" + (SaveLoader.SaveSlot + 1);
                textMesh.text = "";
                textMesh.text += "Autosave is " + autoSaveStatus;
                if(SaveLoader.AutoSave){
                    textMesh.text += ", curent slot is " + currentSlot;
                }
            }
        }

        void Awake()
        {
            loadPreviewsActions.list = this;
        }

        public void UpdateSlotsContent(SavePreviewInfo[] slots)
        {
            // Passa por todas as skills da lista, adicionando as ativas no menu e as ordenando
            for (int i = 0; i < SaveFile.NSaveSlots && i < slotObjects.Length && i < slots.Length; i++)
            {
                SavePreviewInfo slot = slots[i];
                GameObject slotGO = slotObjects[i];
                // Instancia um novo item e o coloca no content
                slotGO.GetComponent<Slot>().LoadSlot(slot, i);
                slotGO.transform.SetParent(slotsContent);
            }
        }
    }
}