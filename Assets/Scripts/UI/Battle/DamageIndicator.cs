using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.Battle {
    [RequireComponent(typeof(Canvas))]
    public class DamageIndicator : MonoBehaviour {
        [SerializeField] private GameObject numberPrefab;
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

        private struct DamageEntry {
            public int value;
            public bool isHeal;
            public DamageStrength strength;
            public DamageEntry(int val, bool heal, DamageStrength str) {
                value = val;
                isHeal = heal;
                strength = str;
            }
        }
        private enum DamageStrength {
            Regular,
            Weak,
            Strong
        }
        private List<DamageEntry?> queue = new List<DamageEntry?>();

        private void InstantiateNewNumber(DamageEntry entry) {
            GameObject newObj = Instantiate(numberPrefab, transform);
            UnityEngine.UI.Text txt = newObj.GetComponent<UnityEngine.UI.Text>();
            if (entry.strength == DamageStrength.Strong) {
                txt.color = (entry.isHeal) ? critHealColor : critDamageColor;
                txt.fontSize = Mathf.CeilToInt(txt.fontSize * critFontSize);
            } else if (entry.strength == DamageStrength.Weak) {
                txt.color = (entry.isHeal) ? weakHealColor : weakDamageColor;
                txt.fontSize = Mathf.CeilToInt(txt.fontSize * weakFontSize);
            } else {
                txt.color = (entry.isHeal) ? healColor : damageColor;
            }
            txt.text = "" + entry.value;
        }

        private void InstantiateMissWord() {
            GameObject newObj = Instantiate(numberPrefab, transform);
            UnityEngine.UI.Text txt = newObj.GetComponent<UnityEngine.UI.Text>();
            txt.color = Color.white;
            txt.text = "miss";
        }

        public void ShowDamage(int value, bool isHeal, float multiplier) {
            float dif = multiplier - 1.0f;
            DamageStrength strength = DamageStrength.Regular;
            if (dif < -float.Epsilon) {
                strength = DamageStrength.Weak;
            } else if (dif > float.Epsilon) {
                strength = DamageStrength.Strong;
            }
            queue.Add(new DamageEntry(value, isHeal, strength));
        }

        public void ShowMiss() {
            queue.Add(null);
        }

        private void Awake() {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }

        // Update is called once per frame
        private void Update() {
            if (cooldown <= float.Epsilon && queue.Count > 0) {
                cooldown = intervalBetweenNumbers;

                DamageEntry? popped = queue[0];
                queue.RemoveAt(0);

                if (popped.HasValue) {
                    InstantiateNewNumber(popped.Value);
                } else {
                    InstantiateMissWord();
                }
            } else if (cooldown > 0) {
                cooldown -= Time.deltaTime;
            }
        }
    }
}