using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Battle.QueueMenu
{
    /// <summary>
    /// Classe responsável por controlar a UI da fila de batalha.
    /// </summary>
    public class BattleQueueUI : MonoBehaviour
    {
        /// <summary>
        /// Objeto base para ser mostrado na fila.
        /// </summary>
        [SerializeField] private GameObject QueueObject;

        /// <summary>
        /// Objeto para indicar o preview de qual posição o personagem vai ficar 
        /// ao utilizar alguma habilidade.
        /// </summary>
        [SerializeField] private RectTransform PreviewObject;

        /// <summary>
        /// Local onde os itens da fila ficarão armazenados.
        /// </summary>
        [SerializeField] private Transform content;

        /// <summary>
        /// Local onde o primeiro item da fila ficará armazenado.
        /// </summary>
        [SerializeField] private Transform currentTurnContent;

        [SerializeField] private HorizontalLayoutGroup layout;

        void Start()
        {
            LoadQueue();
            PreviewObject.gameObject.SetActive(false);
        }

        /// <summary>
        /// Carrega a fila de batalha.
        /// </summary>
        private void LoadQueue()
        {
            GameObject newItem = Instantiate(QueueObject, currentTurnContent);
            newItem.GetComponent<Image>().color = BattleManager.instance.currentUnit.unit.color;

            foreach (BattleUnit unit in BattleManager.instance.queue.list)
            {
                newItem = Instantiate(QueueObject, content);
                newItem.GetComponent<Image>().color = unit.unit.color;
            }
        }

        public void StartPreview(int newPosition = 0)
        {
            PreviewObject.gameObject.SetActive(true);
            SetPreviewPosition(newPosition);
        }

        public void SetPreviewPosition(int newPosition)
        {
            PreviewObject.anchoredPosition = new Vector3(layout.padding.left +
                                                newPosition * (75 + layout.spacing / 2), 0f);
        }

        public void StopPreview()
        {
            PreviewObject.gameObject.SetActive(false);
        }
    }

}