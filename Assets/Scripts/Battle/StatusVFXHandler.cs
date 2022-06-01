using UnityEngine;

namespace FinalInferno {
    public class StatusVFXHandler : MonoBehaviour {
        [SerializeField] private StatusEffectVFX damageChanges;
        [SerializeField] private StatusEffectVFX defenseChanges;
        [SerializeField] private StatusEffectVFX resistanceChanges;
        [SerializeField] private StatusEffectVFX speedChanges;
        private BattleUnit unit = null;
        private int baseDamage = 0;
        private int baseDefense = 0;
        private int baseResistance = 0;
        private int baseSpeed = 0;

        private IndividualStatusVFXHandler[] handlers;
        private int nHandlers = 0;

        public void Setup(BattleUnit battleUnit, int sortingOrder) {
            CreateIndividualHandlers(battleUnit, sortingOrder);
            SetupStatusChangeIcons(sortingOrder);
            SaveBaseStatValues(battleUnit);
        }

        private void CreateIndividualHandlers(BattleUnit battleUnit, int sortingOrder) {
            string[] names = System.Enum.GetNames(typeof(StatusEffectVisuals));
            nHandlers = names.Length;
            handlers = new IndividualStatusVFXHandler[nHandlers];
            for (int i = 0; i < nHandlers; i++) {
                Transform statusVFXTransform = transform.Find(names[i]);
                handlers[i] = CreateIndividualHandler(statusVFXTransform, battleUnit, sortingOrder);
            }
        }

        private static IndividualStatusVFXHandler CreateIndividualHandler(Transform statusVFXTransform, BattleUnit battleUnit, int sortingOrder) {
            if (statusVFXTransform == null)
                return null;

            foreach (Transform child in statusVFXTransform) {
                if (child.TryGetComponent(out SpriteRenderer spriteRenderer))
                    spriteRenderer.sortingOrder = sortingOrder;
                if (child.TryGetComponent(out StatusEffectVFX vfx))
                    vfx.UpdatePosition(battleUnit);
            }
            return new IndividualStatusVFXHandler(statusVFXTransform);
        }

        private void SetupStatusChangeIcons(int sortingOrder) {
            damageChanges.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
            defenseChanges.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
            resistanceChanges.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
            speedChanges.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
        }

        private void SaveBaseStatValues(BattleUnit battleUnit) {
            if (battleUnit == null)
                return;
            unit = battleUnit;
            baseDamage = battleUnit.Unit.baseDmg;
            baseDefense = battleUnit.Unit.baseDef;
            baseResistance = battleUnit.Unit.baseMagicDef;
            baseSpeed = battleUnit.Unit.baseSpeed;
        }

        public void AddEffect(StatusEffect effect) {
            CheckStats();
            int index = (int)effect.VFXID;
            if (handlers[index] == null || handlers[index].effects.Contains(effect)) {
                if (handlers[index] != null)
                    Debug.LogError($"Tentou adicionar o efeito {effect} que ja estava no handler", this);
                return;
            }
            handlers[index].Add(effect);
        }

        private void CheckStats() {
            if (unit == null)
                return;
            if (unit.CurHP <= 0) {
                HideStatChangeIcons();
            } else {
                ShowStatChangeIcons();
            }
            UpdateStatChanges();
        }

        private void HideStatChangeIcons() {
            damageChanges.Hide();
            defenseChanges.Hide();
            resistanceChanges.Hide();
            speedChanges.Hide();
        }

        private void ShowStatChangeIcons() {
            damageChanges.Show();
            defenseChanges.Show();
            resistanceChanges.Show();
            speedChanges.Show();
        }

        private void UpdateStatChanges() {
            // O parametro "TurnsLeft" é utilizado para indicar a variação de status
            damageChanges.TurnsLeft = unit.curDmg - baseDamage;
            defenseChanges.TurnsLeft = unit.curDef - baseDefense;
            resistanceChanges.TurnsLeft = unit.curMagicDef - baseResistance;
            speedChanges.TurnsLeft = unit.curSpeed - baseSpeed;
        }

        public void UpdateEffect(StatusEffect effect) {
            int index = (int)effect.VFXID;
            if (handlers[index] == null || !handlers[index].effects.Contains(effect)) {
                if (handlers[index] != null)
                    Debug.LogError($"Tentou atualizar o efeito {effect} que não estava no handler", this);
                return;
            }
            handlers[index].turnsLeftMax = Mathf.Max(handlers[index].turnsLeftMax, effect.TurnsLeft);
            handlers[index].turnsLeftMin = Mathf.Min(handlers[index].turnsLeftMin, effect.TurnsLeft);
            handlers[index].triggeredUpdate = true;
        }

        public void RemoveEffect(StatusEffect effect) {
            CheckStats();
            int index = (int)effect.VFXID;
            if (handlers[index] == null || !handlers[index].effects.Contains(effect)) {
                if (handlers[index] != null)
                    Debug.LogError($"Tentou remover o efeito {effect} que não estava no handler", this);
                return;
            }
            handlers[index].Remove(effect);
        }

        public void ApplyChanges() {
            CheckStats();
            for (int i = 0; i < nHandlers; i++) {
                if (handlers[i] != null)
                    handlers[i].ApplyChanges();
            }
        }
    }
}