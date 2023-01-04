using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Battle.LifeMenu {
    public class UnitLifeImage : MonoBehaviour {
        public UnitsLives manager;
        public BattleUnit thisUnit;
        public Text lifeText;
        public Image healthFillImage;
        public Image unitImage;

        private void Awake() {
            if (manager != null)
                AddUpdateToEvent();
        }

        public void AddUpdateToEvent() {
            manager.OnUpdate += UpdateUnitLife;
        }

        public void UpdateUnitLife() {
            unitImage.sprite = thisUnit.Portrait;
            lifeText.text = $"{thisUnit.CurHP}/{thisUnit.MaxHP}";
            lifeText.color = thisUnit.Unit.color;
            if (healthFillImage == null)
                return;
            healthFillImage.fillAmount = Mathf.Clamp(thisUnit.CurHP / (float)thisUnit.MaxHP, 0f, 1f);
        }
    }

}