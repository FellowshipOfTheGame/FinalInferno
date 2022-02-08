using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
        private static List<GameObject> battleUnits = new List<GameObject>();
        private static BattleUnit backHead = null;
        private static BattleUnit middleHead = null;
        private static BattleUnit frontHead = null;
        public override string DialogueName => "Cerberus";

        [Space(10)]
        [Header("It has 3 heads")]
        [SerializeField] private Sprite bodySprite;
        [SerializeField] private Sprite battleSpriteBackHead;
        [SerializeField] private RuntimeAnimatorController animatorMiddleHead;
        [SerializeField] private RuntimeAnimatorController animatorFrontHead;
        public override RuntimeAnimatorController Animator {
            get {
                switch (heads) {
                    case 1:
                        return animator;
                    case 2:
                        return animatorMiddleHead;
                    case 3:
                        return animatorFrontHead;
                    default:
                        return null;
                }
            }
        }
        [SerializeField] private Sprite portraitMiddleHead; //
        [SerializeField] private Sprite portraitFrontHead; //
        public override Sprite Portrait {
            get {
                switch (heads) {
                    case 1:
                        return portrait;
                    case 2:
                        return portraitMiddleHead;
                    case 3:
                        return portraitFrontHead;
                    default:
                        return null;
                }
            }
        }
        [Space(10)]
        [SerializeField] private Sprite battleSpriteMiddleHead;
        [Header("    Middle Head Status Effect Position")]
        [Space(-10)]
        [SerializeField, Range(0, 1f)]
        private float xOffsetMiddle = 0;
        [SerializeField, Range(0, 1f)]
        private float yOffsetMiddle = 0;
        [Space(7)]
        [SerializeField] private Sprite battleSpriteFrontHead;
        [Header("    Front Head Status Effect Position")]
        [Space(-10)]
        [SerializeField, Range(0, 1f)]
        private float xOffsetFront = 0;
        [SerializeField, Range(0, 1f)]
        private float yOffsetFront = 0;
        public override Vector2 EffectsRelativePosition {
            get {
                switch (heads) {
                    case 1:
                        return new Vector2(xOffset, yOffset);
                    case 2:
                        return new Vector2(xOffsetMiddle, yOffsetMiddle);
                    case 3:
                        return new Vector2(xOffsetFront, yOffsetFront);
                    default:
                        return new Vector2(0.5f, 1f);
                }
            }
        }
        public override Sprite BattleSprite {
            get {
                if (BattleManager.instance == null) {
                    return battleSprite;
                }

                //Debug.Log("Usou o getter certo");
                switch (heads) {
                    default:
                    case 0:
                        heads = 1;
                        battleUnits.Clear();
                        foreach (BattleUnit bUnit in FindObjectsOfType<BattleUnit>()) {
                            if (bUnit.Unit == this && bUnit.name == name) {
                                bUnit.name += (" " + heads);
                                battleUnits.Add(bUnit.gameObject);
                                backHead = bUnit;
                                break;
                            }
                        }
                        return battleSpriteBackHead;
                    case 1:
                        heads++;
                        foreach (BattleUnit bUnit in FindObjectsOfType<BattleUnit>()) {
                            if (bUnit.Unit == this && bUnit.name == name) {
                                bUnit.name += (" " + heads);
                                battleUnits.Add(bUnit.gameObject);
                                middleHead = bUnit;
                                break;
                            }
                        }
                        return battleSpriteMiddleHead;
                    case 2:
                        heads++;
                        foreach (BattleUnit bUnit in FindObjectsOfType<BattleUnit>()) {
                            if (bUnit.Unit == this && bUnit.name == name) {
                                bUnit.name += (" " + heads);
                                battleUnits.Add(bUnit.gameObject);
                                frontHead = bUnit;
                                break;
                            }
                        }

                        //Faz os sprites ficarem na mesma posição
                        CompositeBattleUnit composite = middleHead.gameObject.AddComponent<CompositeBattleUnit>();
                        if (composite) {
                            composite.AddApendage(backHead);
                            composite.AddApendage(frontHead);
                        }
                        // Cria um game object para ter o sprite do corpo
                        GameObject bodyObj = new GameObject();
                        bodyObj.name = "Cerberus's body";
                        bodyObj.transform.SetParent(middleHead.transform);
                        bodyObj.transform.position = Vector3.zero;
                        bodyObj.transform.rotation = Quaternion.identity;
                        SpriteRenderer sr = bodyObj.gameObject.AddComponent<SpriteRenderer>();
                        if (sr) {
                            sr.sprite = bodySprite;
                            sr.sortingOrder = 0;
                            sr.flipX = true;
                            sr.color = middleHead.GetComponent<SpriteRenderer>().color;
                        }
                        return battleSpriteFrontHead;
                }
            }
        }
        public override float BoundsSizeX => (battleSprite.bounds.size.x);
        public override float BoundsSizeY => (battleSprite.bounds.size.y / 8f);
        [Space(10)]
        [SerializeField] private Sprite queueSpriteMiddleHead;
        [SerializeField] private Sprite queueSpriteFrontHead;
        public override Sprite QueueSprite {
            get {
                switch (heads) {
                    case 1:
                        return queueSprite;
                    case 2:
                        return queueSpriteMiddleHead;
                    case 3:
                        return queueSpriteFrontHead;
                    default:
                        return null;
                }
            }
        }

#if UNITY_EDITOR
        public override Sprite GetSubUnitPortrait(int index) {
            switch (index) {
                case 0:
                    return queueSprite;
                case 1:
                    return queueSpriteMiddleHead;
                case 2:
                    return queueSpriteFrontHead;
                default:
                    return null;
            }
        }
#endif

        public override void ResetParameters() {
            hellFireCD = 0;
            summonedGhosts = false;
        }

        //funcao que escolhe o ataque a ser utilizado
        public override Skill AttackDecision() {
            if (hellFireCD < 1) {
                float rand = Random.Range(0.0f, 1.0f); //gera um numero aleatorio entre 0 e 1

                if (rand < 0.9f / heads) {
                    hellFireCD = (heads - 1);
                    skills[0].Level = ((level - 1) * 3) + 4 - heads;
                    return skills[0]; //decide usar primeira habilidade
                }
            } else {
                hellFireCD--;
            }

            return attackSkill; //decide usar ataque basico
        }

        //funcao que escolhe qual acao sera feita no proprio turno
        public override Skill SkillDecision(float percentageNotDefense) {
            float rand = Random.Range(0.0f, 1.0f); //gera um numero aleatorio entre 0 e 1
            float percentageDebuff = Mathf.Min((1f / 3f), percentageNotDefense / 3f); //porcentagem para o inimigo usar a habilidade de buff
            List<BattleUnit> team;
            bool fearCD = false;

            team = BattleManager.instance.GetTeam(UnitType.Enemy);
            heads = team.Count;

            // Invoca as cabeças fantasma
            if (heads <= 1 && !summonedGhosts) {
                summonedGhosts = true;
                return skills[2];
            }

            team = BattleManager.instance.GetTeam(UnitType.Hero);
            foreach (BattleUnit hero in team) {
                if (!hero.CanAct) {
                    fearCD = true;
                }
            }

            if (!fearCD && rand < percentageDebuff) {
                skills[1].Level = ((level - 1) * 3) + 4 - heads;
                return skills[1]; //decide usar a segunda habilidade(debuff)
            }

            if (rand < percentageNotDefense) {
                return AttackDecision(); //decide atacar
            }

            return defenseSkill; //decide defender
        }

        protected override List<BattleUnit> GetTargets(TargetType type) {
            List<BattleUnit> targets = new List<BattleUnit>();
            List<BattleUnit> team = new List<BattleUnit>();

            switch (type) {
                case TargetType.Self:
                    targets.Add(BattleManager.instance.currentUnit);
                    break;
                case TargetType.MultiAlly:
                    targets = BattleManager.instance.GetTeam(UnitType.Enemy);
                    break;
                case TargetType.MultiEnemy:
                    team = BattleManager.instance.GetTeam(UnitType.Hero);
                    for (int i = 0; i < heads && team.Count > 0; i++) {
                        int targetIdx = TargetDecision(team);
                        targets.Add(team[targetIdx]);
                        team.RemoveAt(targetIdx);
                    }
                    break;
                case TargetType.SingleAlly:
                    team = BattleManager.instance.GetTeam(UnitType.Enemy);
                    targets.Add(team[Random.Range(0, team.Count - 1)]);
                    break;
                case TargetType.SingleEnemy:
                    team = BattleManager.instance.GetTeam(UnitType.Hero);
                    targets.Add(team[TargetDecision(team)]);
                    break;
                case TargetType.DeadAllies:
                    targets = BattleManager.instance.GetTeam(UnitType.Enemy, true, true);
                    break;
            }

            heads = 0;
            return targets;
        }
    }

#if UNITY_EDITOR
    [CustomPreview(typeof(CerberusHead))]
    public class CerberusHeadPreview : UnitPreview {
        public override void OnPreviewGUI(Rect r, GUIStyle background) {
            CerberusHead unit = target as CerberusHead;
            if (unit != null) {
                if (tex == null) {
                    tex = new Texture2D(Mathf.FloorToInt(unit.BattleSprite.textureRect.width), Mathf.FloorToInt(unit.BattleSprite.textureRect.height), unit.BattleSprite.texture.format, false);
                    Color[] colors = unit.BattleSprite.texture.GetPixels(Mathf.FloorToInt(unit.BattleSprite.textureRectOffset.x), Mathf.FloorToInt(unit.BattleSprite.textureRectOffset.y), tex.width, tex.height);
                    tex.SetPixels(colors);
                    tex.Apply();

                    Color[] transparency = new Color[tex.width * tex.height];
                    for (int i = 0; i < transparency.Length; i++) {
                        transparency[i] = Color.clear;
                    }
                    bg = new Texture2D(tex.width, tex.height, tex.format, false, false);
                    bg.SetPixels(transparency);
                    bg.Apply();
                }

                Rect texRect;
                float aspectRatio = tex.height / (float)tex.width;
                float scaledHeight = aspectRatio * 0.8f * r.width;

                if (tex.width > tex.height && (r.height * 0.8f) > scaledHeight) {
                    texRect = new Rect(r.center.x - 0.4f * r.width, r.center.y - aspectRatio * 0.4f * r.width, 0.8f * r.width, aspectRatio * 0.8f * r.width);
                } else {
                    texRect = new Rect(r.center.x - 0.4f * r.height / aspectRatio, r.center.y - 0.4f * r.height, 0.8f * r.height / aspectRatio, 0.8f * r.height);
                }

                float rectSize = 0.1f * Mathf.Max(texRect.width, texRect.height);

                EditorGUI.DrawTextureTransparent(texRect, bg, ScaleMode.StretchToFill);
                GUI.DrawTexture(texRect, tex, ScaleMode.ScaleToFit);

                int previousValue = CerberusHead.heads;
                for (int i = 1; i <= 3; i++) {
                    CerberusHead.heads = i;
                    Rect headRect = new Rect(texRect.x + (unit.EffectsRelativePosition.x * texRect.width) - rectSize / 2, texRect.yMax - (unit.EffectsRelativePosition.y * texRect.height) - rectSize / 2, rectSize, rectSize);
                    EditorGUI.DrawRect(headRect, new Color(0f, 1f, 0f, .7f));
                }
                CerberusHead.heads = previousValue;
            }
        }
    }
#endif
}
