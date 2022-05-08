using UnityEngine;

namespace FinalInferno {
    [RequireComponent(typeof(Movable))]
    public class CharacterOW : MonoBehaviour, IOverworldSkillListener {
        [SerializeField] private Character characterSO;
        [SerializeField] private OverworldSkill sprintSkill;
        public Character CharacterSO => characterSO;
        [SerializeField] private bool isMain;

        private SpriteRenderer spriteRenderer = null;
        [SerializeField] private MoveTo moveTo = null;
        [SerializeField] private Movable movable = null;
        public bool CanMove {
            get => movable ? movable.CanMove : false;
            set {
                if (movable != null) {
                    movable.CanMove = value;
                }
            }
        }

        public static CharacterOW MainOWCharacter => Party.Instance.characters?[0]?.OverworldInstance;

        public static bool PartyCanMove {
            get => Party.Instance.characters?[0]?.OverworldInstance?.CanMove ?? false;
            set {
                foreach (Character character in Party.Instance.characters) {
                    SetCharacterInstanceCanMove(character, value);
                }
            }
        }

        private static void SetCharacterInstanceCanMove(Character character, bool value) {
            if (character?.OverworldInstance != null) {
                character.OverworldInstance.CanMove = value;
            }
        }

        public static void ReloadParty() {
            foreach (Character character in Party.Instance.characters) {
                ReloadCharacter(character);
            }
        }

        private static void ReloadCharacter(Character character) {
            if (!character || !character.archetype) {
                return;
            }
            Hero hero = character.archetype;
            CharacterOW characterOW = character.OverworldInstance;
            SetupCharacterSprite(hero, characterOW);
            SetupCharacterAnimator(hero, characterOW);
        }

        public void Awake() {
            try {
                RegisterCharacterOWInstance();
            } catch (System.Exception error) {
                Debug.LogError(error.Message, this);
                return;
            }

            Hero hero = characterSO.archetype;
            SetupCharacterSprite(hero, this);
            SetupCharacterAnimator(hero, this);
            ResetMovementComponents();
        }

        private void RegisterCharacterOWInstance() {
            characterSO.OverworldInstance = this;
            if (characterSO.OverworldInstance != this) {
                throw new System.Exception($"Failed to set {name} as overworld instance of {characterSO.name}");
            }
        }

        private static void SetupCharacterSprite(Hero hero, CharacterOW characterOW) {
            if (hero == null || !hero.spriteOW) {
                return;
            }
            characterOW.spriteRenderer = Utils.GetOrAddComponent<SpriteRenderer>(characterOW.gameObject);
            characterOW.spriteRenderer.sprite = hero.spriteOW;
            characterOW.spriteRenderer.spriteSortPoint = SpriteSortPoint.Pivot;
        }

        private static void SetupCharacterAnimator(Hero hero, CharacterOW characterOW) {
            if (hero == null || !hero.animatorOW) {
                return;
            }
            Animator anim = Utils.GetOrAddComponent<Animator>(characterOW.gameObject);
            anim.runtimeAnimatorController = hero.animatorOW;
        }

        private void ResetMovementComponents() {
            movable = GetComponent<Movable>();
            moveTo.Reset();
            movable.CanMove = false;
        }

        public void Start() {
            if (isMain) {
                return;
            }
            SetMovementFollowTarget();
            spriteRenderer.sortingOrder = MainOWCharacter.spriteRenderer.sortingOrder;
        }

        private void SetMovementFollowTarget() {
            int characterIndex = Party.Instance.characters.IndexOf(characterSO);
            Character targetCharacter = Party.Instance.characters[characterIndex - 1];
            (moveTo as MoveToTarget).target = targetCharacter.OverworldInstance?.transform;
        }

        public void OnDestroy() {
            if (characterSO.OverworldInstance == this) {
                characterSO.OverworldInstance = null;
            }
        }

        #region SprintCallbacks
        public void OnEnable() {
            sprintSkill?.AddActivationListener(this);
        }

        public void OnDisable() {
            sprintSkill?.RemoveActivationListener(this);
        }

        public void ActivatedSkill(OverworldSkill skill) {
            if (skill != sprintSkill) {
                return;
            }
            float moveSpeedChange = sprintSkill?.effects[0].value1 ?? 0;
            movable.MoveSpeed += moveSpeedChange;
        }

        public void DeactivatedSkill(OverworldSkill skill) {
            if (skill != sprintSkill) {
                return;
            }
            float moveSpeedChange = sprintSkill?.effects[0].value1 ?? 0;
            movable.MoveSpeed -= moveSpeedChange;
        }
        #endregion
    }
}
