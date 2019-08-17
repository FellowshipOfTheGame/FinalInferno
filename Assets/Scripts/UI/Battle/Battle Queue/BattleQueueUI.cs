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

        private Image currentTurnBattleImage;
        private List<Image> BattleImages;

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
            // Coloca o personagem que se encontra em seu turno atual no local específico.
            currentTurnBattleImage = Instantiate(QueueObject, currentTurnContent).GetComponentsInChildren<Image>()[1];
            currentTurnBattleImage.sprite = BattleManager.instance.currentUnit.unit.queueSprite;

            // Coloca o restante dos personagens na fila.
            BattleImages = new List<Image>();
            foreach (BattleUnit unit in BattleManager.instance.queue.list)
            {
                Image newImage = Instantiate(QueueObject, content).GetComponentsInChildren<Image>()[1];
                newImage.sprite = unit.unit.queueSprite;
                BattleImages.Add(newImage);
            }
        }

        /// <summary>
        /// Atualiza a fila de batalha
        /// </summary>
        public void UpdateQueue()
        {
            // Coloca o personagem que se encontra em seu turno atual no local específico.
            currentTurnBattleImage.sprite = BattleManager.instance.currentUnit.unit.queueSprite;

            // Coloca o restante dos personagens na fila.
            int count = 0;
            foreach (BattleUnit unit in BattleManager.instance.queue.list)
                BattleImages[count++].sprite = unit.unit.queueSprite;

            // Destroi os objetos que não estão mais sendo utilizados (personagens que morreram)
            for (int i = BattleImages.Count-1; i >= count ; i--)
            {
                Destroy(BattleImages[i].transform.parent.gameObject);
                BattleImages.RemoveAt(i);
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
            PreviewObject.anchoredPosition = new Vector3(layout.padding.left - layout.spacing/2 +
                                                newPosition * (75 + layout.spacing), 0f);
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