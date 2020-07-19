using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FinalInferno.UI.AII{
    public class BestiaryMenu : MonoBehaviour
    {
        [SerializeField] private float inputCooldown = 0.25f;
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
        [Space(10)]
        [SerializeField] private GameObject rightArrow;
        [SerializeField] private GameObject leftArrow;

        private List<Enemy> enemies = new List<Enemy>();
        private ReadOnlyDictionary<Enemy, int> bestiary;
        private int currentIndex = 0;
        private bool isOpen = false;
        private float cooldown = 0;

        public void OpenBestiary()
        {
            bestiary = Party.Instance.Bestiary;
            enemies.Clear();
            foreach(Enemy enemy in bestiary.Keys){
                // Inimigos que não aparecem no bestiario, como o Dummy, tem essa propriedade nula
                if(enemy.BestiaryPortrait != null){
                    enemies.Add(enemy);
                    enemy.LevelEnemy();
                }
            }
            Enemy firstEntry = (enemies.Count > 0)? enemies[0] : null;
            currentIndex = 0;
            ShowEnemy(firstEntry);
            isOpen = true;
            cooldown = 0f;
        }

        public void CloseBestiary(){
            detailsObject.SetActive(false);
            bestiary = null;
            isOpen = false;
            cooldown = 0f;
        }

        private string getResistanceString(Enemy enemy){
            string str = "";
            string[] elementNames = System.Enum.GetNames(typeof(Element));
            int maxLength = int.MinValue;
            foreach(string name in elementNames){
                if(name.Length > maxLength) maxLength = name.Length;
            }
            bool hasResistance = false;

            foreach(Element element in enemy.ElementalResistances.Keys){
                if(hasResistance){
                    str += "\n";
                }else{
                    hasResistance = true;
                }

                // Escreve o nome do elemento
                str += System.Enum.GetName(typeof(Element), element).PadRight(maxLength) + ":     ";

                // Escreve a resistencia do monstro a esse elemento, usando porcentagem e colorindo para indicar resistencia ou fraqueza
                float value = (1.0f - enemy.ElementalResistances[element]) * 100f;
                if(value > 0){
                    str += "<color=#840000>";
                }else{
                    // valores iguais a zero não devem aparecer aqui, apenas negativos
                    str += "<color=#006400>";
                }
                str += value.ToString("0.###").PadLeft(6) + "%</color>";
            }

            if(hasResistance){
                return str;
            }else{
                return "None";
            }
        }

        private void ShowEnemy(Enemy enemy){
            if(enemy != null){
                detailsObject.SetActive(true);
                monsterName.text = enemy.AssetName;
                portrait.sprite = enemy.BestiaryPortrait;
                bio.text = enemy.Bio;
                rank.text = enemy.name;
                damageType.sprite = Icons.instance.damageSprites[(int)enemy.DamageFocus - 1];
                element.sprite = Icons.instance.elementSprites[(int)enemy.Element - 1];
                HP.text = "" + enemy.hpMax;
                damage.text = "" + enemy.baseDmg;
                speed.text = "" + enemy.baseSpeed;
                defense.text = "" + enemy.baseDef;
                resistance.text = "" + enemy.baseMagicDef;
                exp.text = "" + enemy.BaseExp;
                killCount.text = "" + bestiary[enemy];
                elementalResistances.text = getResistanceString(enemy);
            }else{
                // Essa função só é chamada com null caso o bestiario esteja vazio
                detailsObject.SetActive(false);
                monsterName.text = "Empty";
            }

            if(currentIndex > 0){
                rightArrow.SetActive(true);
            }else{
                rightArrow.SetActive(false);
            }
            if(currentIndex < enemies.Count-1){
                leftArrow.SetActive(true);
            }else{
                leftArrow.SetActive(false);
            }
        }

        void Awake(){
            isOpen = false;
            cooldown = 0f;
            currentIndex = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if(isOpen){
                if(cooldown > inputCooldown){
                    float input = Input.GetAxis("Horizontal");
                    if(input > 0 && currentIndex < enemies.Count-1){
                        currentIndex++;
                        ShowEnemy(enemies[currentIndex]);
                        cooldown = 0f;
                    }else if(input < 0 && currentIndex > 0){
                        currentIndex--;
                        ShowEnemy(enemies[currentIndex]);
                        cooldown = 0f;
                    }
                }else{
                    cooldown += Time.deltaTime;
                }
            }
        }
    }
}