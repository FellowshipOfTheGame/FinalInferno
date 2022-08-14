using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace FinalInferno.UI {
    public class BestiaryMenu : MonoBehaviour {
        private const string resistanceColorString = "<color=#006400>";
        private const string waknessColorString = "<color=#840000>";
        [SerializeField, Range(0f, 2f)] private float inputCooldown = 0.25f;
        [Header("Active References")]
        [SerializeField] private TextMeshProUGUI monsterName;
        [SerializeField] private GameObject detailsObject;
        [SerializeField] private Image portrait;
        [SerializeField] private TextMeshProUGUI bio;
        [Space(5)]
        [SerializeField] private TextMeshProUGUI rank;
        [SerializeField] private Image damageType;
        [SerializeField] private Image element;
        [SerializeField] private TextMeshProUGUI HP;
        [SerializeField] private TextMeshProUGUI damage;
        [SerializeField] private TextMeshProUGUI speed;
        [SerializeField] private TextMeshProUGUI defense;
        [SerializeField] private TextMeshProUGUI resistance;
        [SerializeField] private TextMeshProUGUI exp;
        [SerializeField] private TextMeshProUGUI killCount;
        [SerializeField] private TextMeshProUGUI elementalResistances;
        [SerializeField] private InputActionReference movementAction;
        [Space(10)]
        [SerializeField] private GameObject rightArrow;
        [SerializeField] private GameObject leftArrow;

        private AudioSource source;
        private List<Enemy> enemies = new List<Enemy>();
        private ReadOnlyDictionary<Enemy, int> bestiary;
        private int currentIndex = 0;
        private bool isOpen = false;
        private bool activatedInput = false;
        private float cooldown = 0;

        private void Awake() {
            CloseBestiary();
            source = GetComponent<AudioSource>();
        }

        public void CloseBestiary() {
            detailsObject.SetActive(false);
            monsterName.text = "";
            bestiary = null;
            isOpen = false;
            cooldown = 0f;
            currentIndex = 0;
            activatedInput = false;
        }

        public void ToggleBestiary() {
            if (!isOpen) {
                OpenBestiary();
            } else {
                CloseBestiary();
            }
        }

        public void OpenBestiary() {
            bestiary = Party.Instance.Bestiary;
            enemies.Clear();
            int enemiesLevel = Enemy.CalculateEnemyLevel();
            foreach (Enemy enemy in bestiary.Keys) {
                if (enemy.BestiaryPortrait == null)
                    continue;
                enemies.Add(enemy);
                enemy.LevelEnemy(enemiesLevel);
            }
            Enemy firstEntry = (enemies.Count > 0) ? enemies[0] : null;
            currentIndex = 0;
            isOpen = true;
            ShowEnemy(firstEntry);
            cooldown = 0f;
        }

        private void ShowEnemy(Enemy enemy) {
            if (!isOpen)
                return;

            if (enemy != null) {
                detailsObject.SetActive(true);
                ShowEnemyDetails(enemy);
            } else {
                detailsObject.SetActive(false);
                monsterName.text = "Empty";
            }
            HideInputIndicators();
        }

        private void ShowEnemyDetails(Enemy enemy) {
            monsterName.text = enemy.DialogueName;
            portrait.sprite = enemy.BestiaryPortrait;
            bio.text = enemy.Bio;
            rank.text = $"Rank: <color=#{ColorUtility.ToHtmlStringRGB(enemy.color)}>{enemy.name}</color>";
            damageType.sprite = Icons.instance.damageSprites[(int)enemy.DamageFocus - 1];
            element.sprite = Icons.instance.elementSprites[(int)enemy.Element - 1];
            HP.text = $"HP: {enemy.hpMax}";
            damage.text = $"{enemy.baseDmg}";
            speed.text = $"{enemy.baseSpeed}";
            defense.text = $"{enemy.baseDef}";
            resistance.text = $"{enemy.baseMagicDef}";
            exp.text = $"Exp: {enemy.BaseExp}";
            killCount.text = $"Kills: {bestiary[enemy]}";
            elementalResistances.text = GetResistanceString(enemy);
            if (source)
                source.PlayOneShot(enemy.EnemyCry);
        }

        private string GetResistanceString(Enemy enemy) {
            ReadOnlyDictionary<Element, float> enemyResistances = enemy.ElementalResistances;
            if (enemyResistances.Count <= 0)
                return "None";
            StringBuilder stringBuilder = new StringBuilder(string.Empty);
            int elementMaxLength = CalculateElementMaxLength();
            bool hasResistance = false;
            foreach (Element element in enemyResistances.Keys) {
                stringBuilder.Append(hasResistance ? "\n" : string.Empty);
                hasResistance = true;
                stringBuilder.Append($"{System.Enum.GetName(typeof(Element), element).PadRight(elementMaxLength)}  ");
                float value = (1.0f - enemyResistances[element]) * 100f;
                stringBuilder.Append(value < 0 ? waknessColorString : resistanceColorString);
                stringBuilder.Append($"{value,6:0.###}%</color>");
            }
            return stringBuilder.ToString();
        }

        private static int CalculateElementMaxLength() {
            string[] elementNames = System.Enum.GetNames(typeof(Element));
            int maxLength = int.MinValue;
            foreach (string name in elementNames) {
                if (name.Length > maxLength)
                    maxLength = name.Length;
            }
            return maxLength;
        }

        private void HideInputIndicators() {
            activatedInput = false;
            rightArrow.SetActive(false);
            leftArrow.SetActive(false);
        }

        private void Update() {
            if (!isOpen)
                return;
            if (cooldown > inputCooldown) {
                if (!activatedInput)
                    ShowInputIndicators();
                ReadInputForPageFlip();
            } else {
                cooldown += Time.deltaTime;
            }
        }

        private void ReadInputForPageFlip() {
            float input = movementAction.action.ReadValue<Vector2>().x;
            if (input > 0 && currentIndex < enemies.Count - 1) {
                currentIndex++;
                ShowEnemy(enemies[currentIndex]);
                cooldown = 0f;
            } else if (input < 0 && currentIndex > 0) {
                currentIndex--;
                ShowEnemy(enemies[currentIndex]);
                cooldown = 0f;
            }
        }

        private void ShowInputIndicators() {
            activatedInput = true;
            rightArrow.SetActive(currentIndex < enemies.Count - 1);
            leftArrow.SetActive(currentIndex > 0);
        }
    }
}