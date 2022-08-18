using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Battle {
    [RequireComponent(typeof(Canvas))]
    public class DamageIndicator : MonoBehaviour, IDamageIndicator {
        private const float yOffset = 0.35f;
        private const string missString = "miss";
        [SerializeField] private GameObject numberPrefab;
        [SerializeField] private BattleUnit battleUnit;
        [SerializeField, Range(1f, 5f)] private float critFontSize = 2f;
        [SerializeField, Range(0.1f, 1f)] private float weakFontSize = 0.5f;
        [SerializeField] private Color critDamageColor;
        [SerializeField] private Color damageColor;
        [SerializeField] private Color weakDamageColor;
        [SerializeField] private Color critHealColor;
        [SerializeField] private Color healColor;
        [SerializeField] private Color weakHealColor;

        [Space(10)]
        [SerializeField, Range(0, 5)] private float intervalBetweenNumbers = 0.1f;
        private float cooldown = 0;

        private Queue<DamageEntry?> queue = new Queue<DamageEntry?>();

        private void Awake() {
            GetComponent<Canvas>().worldCamera = Camera.main;
            battleUnit.DamageIndicator = this;
        }

        public void Setup() {
            GetComponent<RectTransform>().anchoredPosition += battleUnit.HeadPosition + new Vector2(0, yOffset);
        }

        private void InstantiateNewNumber(DamageEntry entry) {
            GameObject newObj = Instantiate(numberPrefab, transform);
            Text txt = newObj.GetComponent<Text>();
            ApplyTextSizeAndColor(entry, txt);
            txt.text = $"{entry.value}";
        }

        private void ApplyTextSizeAndColor(DamageEntry entry, Text txt) {
            if (entry.strength == DamageStrength.Strong) {
                txt.color = entry.isHeal ? critHealColor : critDamageColor;
                txt.fontSize = Mathf.CeilToInt(txt.fontSize * critFontSize);
            } else if (entry.strength == DamageStrength.Weak) {
                txt.color = entry.isHeal ? weakHealColor : weakDamageColor;
                txt.fontSize = Mathf.CeilToInt(txt.fontSize * weakFontSize);
            } else {
                txt.color = entry.isHeal ? healColor : damageColor;
            }
        }

        private void InstantiateMissWord() {
            GameObject newObj = Instantiate(numberPrefab, transform);
            Text txt = newObj.GetComponent<Text>();
            txt.color = Color.white;
            txt.text = missString;
        }

        public void ShowDamage(int value, bool isHeal, float multiplier) {
            DamageStrength strength = GetDamageStrength(multiplier);
            queue.Enqueue(new DamageEntry(value, isHeal, strength));
        }

        private static DamageStrength GetDamageStrength(float multiplier) {
            float difference = multiplier - 1.0f;
            if (difference < -float.Epsilon) {
                return DamageStrength.Weak;
            } else if (difference > float.Epsilon) {
                return DamageStrength.Strong;
            } else {
                return DamageStrength.Regular;
            }
        }

        public void ShowMiss() {
            queue.Enqueue(null);
        }

        private void Update() {
            if (cooldown > float.Epsilon || queue.Count <= 0) {
                if (cooldown > 0)
                    cooldown -= Time.deltaTime;
                return;
            }
            cooldown = intervalBetweenNumbers;
            ShowNextDamageEntry();
        }

        private void ShowNextDamageEntry() {
            DamageEntry? popped = queue.Dequeue();
            if (popped.HasValue) {
                InstantiateNewNumber(popped.Value);
            } else {
                InstantiateMissWord();
            }
        }
    }
}