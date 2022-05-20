using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    public enum StatusEffectVisuals {
        Null = 0,// Esses valores serão usados para acesso numa array, então precisam começar em 0 e incrementar de 1 em 1
        Bribed,
        Confused,
        DamageDown,
        DamageDownExponential,
        DamageDrained,
        DamageUp,
        DamageUpExponential,
        DefenseUp,
        DefenseDown,
        Burn,
        Hypothermia,
        Quicksand,
        Suffocation,
        DamageOverTime,
        DelayedSkill,
        DrainingDamage,
        HealingOverTime,
        Hiding,
        LosingHPOverTime,
        MarketCrash,
        Paralyzed,
        Regenerating,
        ResistanceDown,
        ResistanceUp,
        SpeedDown,
        SpeedUp,
        StatusResistUp,
        Taunting,
        VengefulGhost
    }

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

        public void Setup(BattleUnit bUnit, int sortingOrder) {
            // Inicia todos os valores necessarios
            string[] names = System.Enum.GetNames(typeof(StatusEffectVisuals));
            nHandlers = names.Length;
            handlers = new IndividualStatusVFXHandler[nHandlers];
            // Cria handlers apenas para os efeitos que existem como filhos
            for (int i = 0; i < nHandlers; i++) {
                Transform child = transform.Find(names[i]);
                if (child != null) {
                    Debug.Log($"Creating vfx handler for status effect {names[i]}");
                    handlers[i] = new IndividualStatusVFXHandler(child);
                    foreach (Transform t in child) {
                        SpriteRenderer sr = t.GetComponent<SpriteRenderer>();
                        if (sr != null) {
                            sr.sortingOrder = sortingOrder;
                        }
                    }
                    foreach (Transform t in child) {
                        StatusEffectVFX vfx = t.GetComponent<StatusEffectVFX>();
                        if (vfx != null && vfx.Position != SkillVFX.TargetPosition.Default) {
                            Debug.Log($"object {t.name} being positioned at {bUnit.name}'s {vfx.Position}");
                            vfx.UpdatePosition(bUnit);
                        }
                    }
                } else {
                    Debug.Log($"vfx handler for status effect {names[i]} is not setup");
                    handlers[i] = null;
                }
            }

            // Os icones de mudança de status precisam ser ordenados também
            damageChanges.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
            defenseChanges.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
            resistanceChanges.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
            speedChanges.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;

            // Pega a situação inicial da unidade e armazena os valores de status
            if (bUnit != null) {
                unit = bUnit;
                // TO DO: colocar um status base no battleunit e fazer ele ser alterado quando
                // o skill effect tenta alterar ele antes do setup terminar (OnSpawn)
                // baseDamage = bUnit.curDmg;
                // baseDefense = bUnit.curDef;
                // baseResistance = bUnit.curMagicDef;
                // baseSpeed = bUnit.curSpeed;
                baseDamage = bUnit.Unit.baseDmg;
                baseDefense = bUnit.Unit.baseDef;
                baseResistance = bUnit.Unit.baseMagicDef;
                baseSpeed = bUnit.Unit.baseSpeed;
            }
        }

        private void CheckStats() {
            if (unit != null) {
                if (unit.CurHP <= 0) {
                    damageChanges.Hide();
                    defenseChanges.Hide();
                    resistanceChanges.Hide();
                    speedChanges.Hide();
                } else {
                    damageChanges.Show();
                    defenseChanges.Show();
                    resistanceChanges.Show();
                    speedChanges.Show();
                }
                // O parametro "turnsLeft" é utilizado para indicar a variação de status
                damageChanges.TurnsLeft = (unit.curDmg - baseDamage);
                defenseChanges.TurnsLeft = (unit.curDef - baseDefense);
                resistanceChanges.TurnsLeft = (unit.curMagicDef - baseResistance);
                speedChanges.TurnsLeft = (unit.curSpeed - baseSpeed);
            }
        }

        public void AddEffect(StatusEffect effect) {
            CheckStats();

            int index = (int)effect.VFXID;
            if (handlers[index] == null || handlers[index].effects.Contains(effect)) {
                // Se o handler não existir é porque o efeito visual não foi implementado
                // Se o efeito em questão ja está sendo mostrado ele não deve ser adicionado de novo
                if (handlers[index] != null) {
                    Debug.LogError("Tentou adicionar um efeito que ja estava no handler");
                }
                return;
            }

            // Setar esse trigger deve ser feito de maneira independente de ApplyChanges
            handlers[index].Add(effect);
        }

        public void UpdateEffect(StatusEffect effect) {
            int index = (int)effect.VFXID;
            if (handlers[index] == null || !handlers[index].effects.Contains(effect)) {
                // Se o handler não existir é porque o efeito visual não foi implementado
                // Se o efeito em questão não esta sendo mostrado aqui, ele não deveria ser atualizado aqui
                if (handlers[index] != null) {
                    Debug.LogError($"Tentou atualizar o efeito {effect} que não estava no handler");
                }
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
                // Se o handler não existir é porque o efeito visual não foi implementado
                // Se o efeito em questão não esta sendo mostrado aqui, ele não deveria ser removido aqui
                if (handlers[index] != null) {
                    Debug.LogError("Tentou remover um efeito que não estava no handler");
                }
                return;
            }

            // Setar esse trigger deve ser feito de maneira independente de ApplyChanges
            handlers[index].Remove(effect);
        }

        public void ApplyChanges() {
            CheckStats();

            // Aplica as alterações em cada tipo de status effect
            for (int i = 0; i < nHandlers; i++) {
                if (handlers[i] != null) {
                    handlers[i].ApplyChanges();
                }
            }
        }
    }
}