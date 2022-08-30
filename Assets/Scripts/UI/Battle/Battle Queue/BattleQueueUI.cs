using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Battle.QueueMenu {
    /// <summary>
    /// Classe responsável por controlar a UI da fila de batalha.
    /// </summary>
    public class BattleQueueUI : MonoBehaviour {
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

        private void Awake() {
            BattleImages = new List<Image>();
            PreviewObject.gameObject.SetActive(false);
            currentTurnBattleImage = Instantiate(QueueObject, currentTurnContent).GetComponentsInChildren<Image>()[1];
        }

        /// <summary>
        /// Atualiza a fila de batalha
        /// </summary>
        public void UpdateQueue(BattleUnit currentUnit) {
            while (BattleImages.Count < BattleManager.instance.queue.Count) {
                Image newImage = Instantiate(QueueObject, content).GetComponentsInChildren<Image>()[1];
                BattleImages.Add(newImage);
            }
            while (BattleImages.Count > BattleManager.instance.queue.Count) {
                Destroy(BattleImages[BattleImages.Count - 1].transform.parent.gameObject);
                BattleImages.RemoveAt(BattleImages.Count - 1);
            }

            // Coloca o personagem que se encontra em seu turno atual no local específico.
            currentTurnBattleImage.sprite = currentUnit ? currentUnit.QueueSprite : null;
            if (currentTurnBattleImage.sprite != null) {
                currentTurnBattleImage.color = Color.white;
            } else {
                currentTurnBattleImage.color = Color.clear;
            }

            // Coloca o restante dos personagens na fila.
            for (int i = 0; i < BattleManager.instance.queue.Count; i++) {
                // BattleImages[i].transform.parent.gameObject.SetActive(true);
                BattleImages[i].sprite = BattleManager.instance.queue.Peek(i).QueueSprite;
                if (BattleImages[i].sprite != null) {
                    BattleImages[i].color = Color.white;
                } else {
                    BattleImages[i].color = Color.clear;
                }
            }
        }

        /// <summary>
        /// Coloca um marcador na posição da lista onde o personagem ficará quando utilizar a referente skill.
        /// </summary>
        /// <param name="newPosition"> Posição do personagem se utilizar a skill. </param>
        public void StartPreview(int newPosition = 0) {
            PreviewObject.gameObject.SetActive(true);
            Transform image = PreviewObject.transform.Find("Image");
            if (image) {
                image.GetComponent<Image>().sprite = currentTurnBattleImage.sprite;
            }
            SetPreviewPosition(newPosition);
        }

        /// <summary>
        /// Posiciona o marcador na posição da lista onde o personagem ficará quando utilizar a referente skill.
        /// </summary>
        /// <param name="newPosition"> Posição do personagem se utilizar a skill. </param>
        public void SetPreviewPosition(int newPosition) {
            PreviewObject.anchoredPosition = new Vector2(Mathf.Min(layout.GetComponent<RectTransform>().rect.width,
                                    layout.padding.left - layout.spacing / 2 + newPosition * (75 + layout.spacing))
                                                                    , PreviewObject.anchoredPosition.y);
        }


        /// <summary>
        /// Retira o marcador.
        /// </summary>
        public void StopPreview() {
            PreviewObject.gameObject.SetActive(false);
            Transform image = PreviewObject.transform.Find("Image");
            if (image) {
                image.GetComponent<Image>().sprite = null;
            }
        }
    }

}