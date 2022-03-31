using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace FinalInferno.UI {
    public class BestiaryMenu : MonoBehaviour {
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

        public void ToggleBestiary() {
            if (isOpen) {
                CloseBestiary();
            } else {
                OpenBestiary();
            }
        }

        public void OpenBestiary() {
            bestiary = Party.Instance.Bestiary;
            enemies.Clear();
            foreach (Enemy enemy in bestiary.Keys) {
                // Inimigos que não aparecem no bestiario, como o Dummy, tem essa propriedade nula
                if (enemy.BestiaryPortrait != null) {
                    enemies.Add(enemy);
                    enemy.LevelEnemy();
                }
            }
            Enemy firstEntry = (enemies.Count > 0) ? enemies[0] : null;
            currentIndex = 0;
            isOpen = true;
            ShowEnemy(firstEntry);
            cooldown = 0f;
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

        private string GetResistanceString(Enemy enemy) {
            string str = "";
            string[] elementNames = System.Enum.GetNames(typeof(Element));
            int maxLength = int.MinValue;
            foreach (string name in elementNames) {
                if (name.Length > maxLength) {
                    maxLength = name.Length;
                }
            }
            bool hasResistance = false;

			ReadOnlyDictionary<Element, float> enemyResistances = enemy.ElementalResistances;
            foreach (Element element in enemyResistances.Keys) {
                if (hasResistance) {
                    str += "\n";
                } else {
                    hasResistance = true;
                }

                // Escreve o nome do elemento
                str += System.Enum.GetName(typeof(Element), element).PadRight(maxLength) + "  ";

                // Escreve a resistencia do monstro a esse elemento, usando porcentagem e colorindo para indicar resistencia ou fraqueza
                float value = (1.0f - enemyResistances[element]) * 100f;
                if (value < 0) {
                    str += "<color=#840000>";
                } else {
                    // valores iguais a zero não devem aparecer aqui, apenas negativos
                    str += "<color=#006400>";
                }
                str += value.ToString("0.###").PadLeft(6) + "%</color>";
            }

            if (hasResistance) {
                return str;
            } else {
                return "None";
            }
        }

        private void ShowEnemy(Enemy enemy) {
            // Se o bestiario não estiver aberto, não faz nada
            if (!isOpen) {
                return;
            }

            if (enemy != null) {
                detailsObject.SetActive(true);
                monsterName.text = (enemy is CerberusHead) ? "Cerberus" : enemy.AssetName;
                portrait.sprite = enemy.BestiaryPortrait;
                bio.text = enemy.Bio;
                rank.text = "Rank: <color=#" + ColorUtility.ToHtmlStringRGB(enemy.color) + ">" + enemy.name + "</color>";
                damageType.sprite = Icons.instance.damageSprites[(int)enemy.DamageFocus - 1];
                element.sprite = Icons.instance.elementSprites[(int)enemy.Element - 1];
                HP.text = "HP: " + enemy.hpMax;
                damage.text = "" + enemy.baseDmg;
                speed.text = "" + enemy.baseSpeed;
                defense.text = "" + enemy.baseDef;
                resistance.text = "" + enemy.baseMagicDef;
                exp.text = "Exp: " + enemy.BaseExp;
                killCount.text = "Kills: " + bestiary[enemy];
                elementalResistances.text = GetResistanceString(enemy);
                if (source != null) {
                    source.PlayOneShot(enemy.EnemyCry);
                }
            } else {
                // Essa função só é chamada com null caso o bestiario esteja vazio
                detailsObject.SetActive(false);
                monsterName.text = "Empty";
            }

            activatedInput = false;
            rightArrow.SetActive(false);
            leftArrow.SetActive(false);
        }

        private void Awake() {
            CloseBestiary();
            source = GetComponent<AudioSource>();
        }

        private void Update() {
            if (isOpen) {
                if (cooldown > inputCooldown) {
                    if (!activatedInput) {
                        activatedInput = true;
                        if (currentIndex < enemies.Count - 1) {
                            rightArrow.SetActive(true);
                        } else {
                            rightArrow.SetActive(false);
                        }
                        if (currentIndex > 0) {
                            leftArrow.SetActive(true);
                        } else {
                            leftArrow.SetActive(false);
                        }
                    }

                    // float input = UnityEngine.Input.GetAxis("Horizontal");
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
                } else {
                    cooldown += Time.deltaTime;
                }
            }
        }
    }
}