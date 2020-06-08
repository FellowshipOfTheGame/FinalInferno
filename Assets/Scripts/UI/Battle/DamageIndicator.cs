using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.Battle{
    [RequireComponent(typeof(Canvas))]
    public class DamageIndicator : MonoBehaviour
    {
        [SerializeField] private GameObject numberPrefab;
        [SerializeField] private Color damageColor;
        [SerializeField] private Color healColor;

        [Space(10)]
        [SerializeField, Range(0, 5)] private float intervalBetweenNumbers = 0.1f;
        private float cooldown = 0;

        private struct DamageEntry{
            public int value;
            public bool isHeal;
            public DamageEntry(int val, bool heal){
                value = val;
                isHeal = heal;
            }
        }
        private List<DamageEntry> queue = new List<DamageEntry>();

        private void InstantiateNewNumber(DamageEntry entry){
            GameObject newObj = Instantiate(numberPrefab, transform);
            newObj.GetComponent<UnityEngine.UI.Text>().color = (entry.isHeal)? healColor : damageColor;
            newObj.GetComponent<UnityEngine.UI.Text>().text = "" + entry.value;
        }

        public void ShowDamage(int value, bool isHeal){
            queue.Add(new DamageEntry(value, isHeal));
        }

        void Awake(){
            GetComponent<Canvas>().worldCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            if(cooldown <= float.Epsilon && queue.Count > 0){
                cooldown = intervalBetweenNumbers;

                DamageEntry popped = queue[0];
                queue.RemoveAt(0);
                InstantiateNewNumber(popped);
            }else if(cooldown > 0){
                cooldown -= Time.deltaTime;
            }
        }
    }
}