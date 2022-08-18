using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FinalInferno.EventSystem;

namespace FinalInferno.UI.Battle.QueueMenu {
    public class BattleQueueUI : MonoBehaviour {
        private const float previewItemSize = 75f;
        [SerializeField] private GameObject QueueObject;
        [SerializeField] private RectTransform PreviewObject;
        [SerializeField] private Image previewObjectImage;
        [SerializeField] private Image currentTurnBattleImage;
        [SerializeField] private Transform content;
        [SerializeField] private Transform currentTurnContent;
        [SerializeField] private HorizontalLayoutGroup layout;
        [SerializeField] private GenericEventListenerFI<int> startPreviewEventListener;
        [SerializeField] private VoidEventListenerFI stopPreviewEventListener;
        private List<Image> BattleImages;
        private BattleQueue BattleQueue => BattleManager.instance.queue;

        private void Awake() {
            BattleImages = new List<Image>();
            PreviewObject.gameObject.SetActive(false);
        }

        private void Start() {
            BattleQueue.OnUpdateQueue.AddListener(UpdateQueue);
        }

        private void OnEnable() {
            startPreviewEventListener.StartListeningEvent();
            stopPreviewEventListener.StartListeningEvent();
        }

        private void OnDisable() {
            startPreviewEventListener.StopListeningEvent();
            stopPreviewEventListener.StopListeningEvent();
        }

        public void UpdateQueue(BattleUnit currentUnit) {
            InstantiateOrDestroyImages();
            ShowCurrentUnit(currentUnit);
            ShowRemainingUnitsInQueue();
        }

        private void InstantiateOrDestroyImages() {
            while (BattleImages.Count < BattleQueue.Count) {
                Image newImage = Instantiate(QueueObject, content).GetComponentsInChildren<Image>()[1];
                BattleImages.Add(newImage);
            }
            while (BattleImages.Count > BattleQueue.Count) {
                Destroy(BattleImages[BattleImages.Count - 1].transform.parent.gameObject);
                BattleImages.RemoveAt(BattleImages.Count - 1);
            }
        }

        private void ShowCurrentUnit(BattleUnit currentUnit) {
            currentTurnBattleImage.sprite = currentUnit ? currentUnit.QueueSprite : null;
            currentTurnBattleImage.color = currentTurnBattleImage.sprite != null ? Color.white : Color.clear;
        }

        private void ShowRemainingUnitsInQueue() {
            for (int i = 0; i < BattleQueue.Count; i++) {
                BattleImages[i].sprite = BattleQueue.Peek(i).QueueSprite;
                BattleImages[i].color = BattleImages[i].sprite != null ? Color.white : Color.clear;
            }
        }

        public void StartPreview(int actionPoints) {
            int newPosition = BattleQueue.CalculateNewPosition(actionPoints);
            PreviewObject.gameObject.SetActive(true);
            if (previewObjectImage)
                previewObjectImage.sprite = currentTurnBattleImage.sprite;
            SetPreviewPosition(newPosition);
        }

        public void SetPreviewPosition(int newPosition) {
            PreviewObject.anchoredPosition = new Vector2(CalculatePositionX(newPosition), PreviewObject.anchoredPosition.y);
        }

        private float CalculatePositionX(int newPosition) {
            float maxSizeX = layout.GetComponent<RectTransform>().rect.width;
            float positionX = layout.padding.left - layout.spacing / 2 + newPosition * (75 + layout.spacing);
            return Mathf.Min(maxSizeX, positionX);
        }

        public void StopPreview() {
            PreviewObject.gameObject.SetActive(false);
            if (previewObjectImage)
                previewObjectImage.sprite = null;
        }
    }
}