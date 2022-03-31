﻿using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "CerberusHead", menuName = "ScriptableObject/Enemy/CerberusHead")]
    public class CerberusHead : Enemy {
        // OBS.: A IA parte do pressuposto que as 3 cabeças do cérbero são as unicas unidades no time inimigo
        // O posicionamento de unidades também será feito levando isso em conta
        // O nível do cérbero deve ser incrementado de 1 em 1 para versões mais fortes
        // O nível das habilidades do cérbero devem ser incrementadas de 3 em 3 para cada versão
        private const int maxHeads = 3;
        public static int heads = 0;
        private static int hellFireCD = 0;
        private static bool summonedGhosts;
        private static BattleUnit backHead = null;
        private static BattleUnit middleHead = null;
        private static BattleUnit frontHead = null;
        public override string DialogueName => "Cerberus";
        private int SkillLevel => ((level - 1) * 3) + 4 - heads;
        private Skill HellfireSkill => skills[0];
        private Skill TremendousRoalSkill => skills[1];
        private Skill GhostLimbsSkill => skills[2];

        [Space(10)]
        [Header("It has 3 heads")]
        [SerializeField] private Sprite bodySprite;
        [SerializeField] private Sprite battleSpriteBackHead;
        [SerializeField] private RuntimeAnimatorController animatorMiddleHead;
        [SerializeField] private RuntimeAnimatorController animatorFrontHead;
        public override RuntimeAnimatorController Animator {
            get {
                return heads switch {
                    1 => animator,
                    2 => animatorMiddleHead,
                    3 => animatorFrontHead,
                    _ => null
                };
            }
        }
        [SerializeField] private Sprite portraitMiddleHead;
        [SerializeField] private Sprite portraitFrontHead;
        public override Sprite Portrait {
            get {
                return heads switch {
                    1 => portrait,
                    2 => portraitMiddleHead,
                    3 => portraitFrontHead,
                    _ => null
                };
            }
        }
        [Space(10)]
        [SerializeField] private Sprite battleSpriteMiddleHead;
        [Header("    Middle Head Status Effect Position")]
        [Space(-10)]
        [SerializeField, Range(0, 1f)] private float xOffsetMiddle = 0;
        [SerializeField, Range(0, 1f)] private float yOffsetMiddle = 0;
        [Space(7)]
        [SerializeField] private Sprite battleSpriteFrontHead;
        [Header("    Front Head Status Effect Position")]
        [Space(-10)]
        [SerializeField, Range(0, 1f)] private float xOffsetFront = 0;
        [SerializeField, Range(0, 1f)] private float yOffsetFront = 0;
        public override Vector2 EffectsRelativePosition {
            get {
                return heads switch {
                    1 => new Vector2(xOffset, yOffset),
                    2 => new Vector2(xOffsetMiddle, yOffsetMiddle),
                    3 => new Vector2(xOffsetFront, yOffsetFront),
                    _ => new Vector2(0.5f, 1f)
                };
            }
        }
        public override Sprite BattleSprite {
            get {
                if (BattleManager.instance == null) {
                    return battleSprite;
                }

                switch (heads) {
                    default:
                    case 0:
                        heads = 1;
                        SaveBackHeadBattleUnitReference();
                        return battleSpriteBackHead;
                    case 1:
                        heads++;
                        SaveMiddleHeadBattleUnitReference();
                        return battleSpriteMiddleHead;
                    case 2:
                        heads++;
                        SaveFrontHeadBattleUnitReference();
                        AdjustUnitPositions();
                        CreateCerberusBodyObject();
                        return battleSpriteFrontHead;
                }
            }
        }

        private void SaveBackHeadBattleUnitReference() {
            foreach (BattleUnit bUnit in FindObjectsOfType<BattleUnit>()) {
                if (bUnit.Unit != this || bUnit.name != name) {
                    continue;
                }
                bUnit.name += $" {heads}";
                backHead = bUnit;
                break;
            }
        }

        private void SaveMiddleHeadBattleUnitReference() {
            foreach (BattleUnit bUnit in FindObjectsOfType<BattleUnit>()) {
                if (bUnit.Unit != this || bUnit.name != name) {
                    continue;
                }
                bUnit.name += $" {heads}";
                middleHead = bUnit;
                break;
            }
        }

        private void SaveFrontHeadBattleUnitReference() {
            foreach (BattleUnit bUnit in FindObjectsOfType<BattleUnit>()) {
                if (bUnit.Unit != this || bUnit.name != name) {
                    continue;
                }
                bUnit.name += $" {heads}";
                frontHead = bUnit;
                break;
            }
        }

        private static void AdjustUnitPositions() {
            CompositeBattleUnit composite = middleHead.gameObject.AddComponent<CompositeBattleUnit>();
            composite?.AddApendage(backHead);
            composite?.AddApendage(frontHead);
        }

        private void CreateCerberusBodyObject() {
            GameObject bodyObj = new GameObject();
            bodyObj.name = "Cerberus's body";
            bodyObj.transform.SetParent(middleHead.transform);
            bodyObj.transform.position = Vector3.zero;
            bodyObj.transform.rotation = Quaternion.identity;
            SpriteRenderer renderer = bodyObj.gameObject.AddComponent<SpriteRenderer>();
            if (!renderer) {
                return;
            }
            renderer.sprite = bodySprite;
            renderer.sortingOrder = 0;
            renderer.flipX = true;
            renderer.color = middleHead.GetComponent<SpriteRenderer>().color;
        }

        public override float BoundsSizeX => (battleSprite.bounds.size.x);
        public override float BoundsSizeY => (battleSprite.bounds.size.y / 8f);
        [Space(10)]
        [SerializeField] private Sprite queueSpriteMiddleHead;
        [SerializeField] private Sprite queueSpriteFrontHead;
        public override Sprite QueueSprite {
            get {
                return heads switch {
                    1 => queueSprite,
                    2 => queueSpriteMiddleHead,
                    3 => queueSpriteFrontHead,
                    _ => null
                };
            }
        }

        public override Sprite GetSubUnitPortrait(int index) {
            return index switch {
                0 => queueSprite,
                1 => queueSpriteMiddleHead,
                2 => queueSpriteFrontHead,
                _ => null
            };
        }

        public override void ResetParameters() {
            hellFireCD = 0;
            summonedGhosts = false;
        }

        protected override Skill SkillDecision(float percentageNotDefense) {
            heads = BattleManager.instance.GetTeam(UnitType.Enemy).Count;
            if (heads <= 1 && !summonedGhosts) {
                summonedGhosts = true;
                return GhostLimbsSkill;
            }

            float roll = Random.Range(0.0f, 1.0f);
            float percentageDebuff = Mathf.Min(1f, percentageNotDefense) / 3f;

            if (!AllHeroesAreParalised() && roll < percentageDebuff) {
                TremendousRoalSkill.Level = SkillLevel;
                return TremendousRoalSkill;
            }
            if (roll < percentageNotDefense) {
                return AttackDecision();
            }
            return defenseSkill;
        }

        public override Skill AttackDecision() {
            if (hellFireCD < 1) {
                return RollHellfireAttack();
            }
            hellFireCD--;
            return attackSkill;
        }

        private Skill RollHellfireAttack() {
            float roll = Random.Range(0.0f, 1.0f);
            if (roll < 0.9f / heads) {
                hellFireCD = (heads - 1);
                HellfireSkill.Level = SkillLevel;
                return HellfireSkill;
            }
            return attackSkill;
        }

        protected override List<BattleUnit> GetTargets(TargetType type) {
            return type switch {
                TargetType.Self => new List<BattleUnit>() { BattleManager.instance.currentUnit },
                TargetType.AllLiveAllies => BattleManager.instance.GetTeam(UnitType.Enemy),
                TargetType.AllLiveEnemies => GetTargetsPerHead(),
                TargetType.SingleLiveAlly => new List<BattleUnit>() { GetRandomLiveAlly() },
                TargetType.SingleLiveEnemy => new List<BattleUnit>() { TargetDecision(GetHeroesTeam()) },
                TargetType.AllDeadAllies => BattleManager.instance.GetTeam(UnitType.Enemy, true, true),
                _ => throw new System.NotImplementedException("[CerberusHead.cs]: Target type not implemented for enemy targeting")
            };
        }

        private List<BattleUnit> GetTargetsPerHead() {
            List<BattleUnit> targets = new List<BattleUnit>();
            List<BattleUnit> heroesTeam = GetHeroesTeam();
            for (int i = 0; i < heads && heroesTeam.Count > 0; i++) {
                BattleUnit target = TargetDecision(heroesTeam);
                targets.Add(target);
                heroesTeam.Remove(target);
            }
            return targets;
        }
    }
}
