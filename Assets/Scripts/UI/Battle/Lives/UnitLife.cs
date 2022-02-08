using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Battle.LifeMenu {

    /// <summary>
    /// Classe responsável por mostrar a vida máxima e atual de um heroi.
    /// </summary>
    public class UnitLife : MonoBehaviour {
        /// <summary>
        /// Referência para o gerenciador do menu de vidas.
        /// </summary>
        public UnitsLives manager;

        /// <summary>
        /// Heroi que será mostrado as informações de vida.
        /// </summary>
        public BattleUnit thisUnit;

        /// <summary>
        /// Campo de texto onde serão mostradas as informações de vida do heroi.
        /// </summary>
        public Text lifeText;

        /// <summary>
        /// Adiciona a atualização da vida desse heroi no evento do gerenciador.
        /// </summary>
        private void Awake() {
            if (manager != null) {
                AddUpdateToEvent();
            }
        }

        public void AddUpdateToEvent() {
            manager.OnUpdate += UpdateUnitLife;
        }

        /// <summary>
        /// Atualiza o campo de texto com as informações de vida do heroi e também com sua cor.
        /// </summary>
        public virtual void UpdateUnitLife() {
            lifeText.text = thisUnit.Unit.name + " - " + thisUnit.CurHP + "/" + thisUnit.MaxHP;
            lifeText.color = thisUnit.Unit.color;
        }
    }

}