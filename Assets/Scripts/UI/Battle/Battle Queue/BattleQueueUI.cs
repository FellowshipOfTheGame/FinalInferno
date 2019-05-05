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

        /// <summary>
        /// Configurações de layout.
        /// </summary>
        [SerializeField] private HorizontalLayoutGroup layout;

        void Start()
        {
            LoadQueue();
            PreviewObject.gameObject.SetActive(false);
        }

        /// <summary>
        /// Carrega a fila de batalha.
        /// </summary>
        public void LoadQueue()
        {
            // Apaga os itens previamente alocados na fila
            foreach (Image img in content.GetComponentsInChildren<Image>())
            {
                Destroy(img.gameObject);
            }
            
            // Coloca o personagem que se encontra em seu turno atual no local específico.
            GameObject newItem = Instantiate(QueueObject, currentTurnContent);
            newItem.GetComponent<Image>().color = BattleManager.instance.currentUnit.unit.color;

            // Coloca o restante dos personagens na fila.
            foreach (BattleUnit unit in BattleManager.instance.queue.list)
            {
                newItem = Instantiate(QueueObject, content);
                newItem.GetComponent<Image>().color = unit.unit.color;
            }
        }

        /// <summary>
        /// Coloca um marcador na posição da lista onde o personagem ficará quando utilizar a referente skill.
        /// </summary>
        /// <param name="newPosition"> Posição do personagem se utilizar a skill. </param>
        public void StartPreview(int newPosition = 0)
        {
            PreviewObject.gameObject.SetActive(true);
            SetPreviewPosition(newPosition);
        }

        /// <summary>
        /// Posiciona o marcador na posição da lista onde o personagem ficará quando utilizar a referente skill.
        /// </summary>
        /// <param name="newPosition"> Posição do personagem se utilizar a skill. </param>
        public void SetPreviewPosition(int newPosition)
        {
            PreviewObject.anchoredPosition = new Vector3(layout.padding.left +
                                                newPosition * (75 + layout.spacing / 2), 0f);
        }


        /// <summary>
        /// Retira o marcador.
        /// </summary>
        public void StopPreview()
        {
            PreviewObject.gameObject.SetActive(false);
        }
    }

}