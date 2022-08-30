using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace FinalInferno {
    public class RECalculator : MonoBehaviour, IOverworldSkillListener {
        public static bool encountersEnabled = true;
        [Header("Asset References")]
        [SerializeField] private Transform playerObj = null;
        [SerializeField] private MapEncounterList mapEncounterList = null;
        [SerializeField] private EncounterRate encounterRate = null;
        [Header("Encounter Skills Info")]
        [SerializeField] private OverworldSkill encounterIncreaseSkill = null;
        [SerializeField] private FloatVariable encounterIncDistWalked = null;
        private float EncounterIncDistWalked {
            get => encounterIncDistWalked ? encounterIncDistWalked.Value : 0;
            set {
                if (encounterIncDistWalked)
                    encounterIncDistWalked.UpdateValue(value);
            }
        }
        private float encounterIncDist = 0;
        [SerializeField] private OverworldSkill encounterDecreaseSkill = null;
        [SerializeField] private FloatVariable encounterDecDistWalked = null;
        private float EncounterDecDistWalked {
            get => encounterDecDistWalked ? encounterDecDistWalked.Value : 0;
            set {
                if (encounterDecDistWalked)
                    encounterDecDistWalked.UpdateValue(value);
            }
        }
        private float encounterDecDist = 0;
        private float skillModifier = 0;
        [Header("Battle Info")]
        [SerializeField] public Sprite battleBG = null;
        [SerializeField] private AudioClip battleBGM = null;
        [SerializeField] private AudioClip overworldBGM = null;

        private float baseEncounterRate = 0f;
        private float rateIncreaseValue = 0f;
        private Fog.Dialogue.Agent agent = null;
        private float curEncounterRate = 0f;
        private Vector2 lastCheckPosition = Vector2.zero;
        private Vector2 lastPosition = Vector2.zero;
        private float distanceWalked = 0f;
        private const float distanceTreshold = 1.0f;
        private bool isSafeArea = false;

        [Header("Expected value = TriggerChangeScene")]
        [SerializeField] private FinalInferno.UI.FSM.ButtonClickDecision decision;

        #region Initial Setup
        private void Start() {
            InitPlayerPositionVariables();
            FindDialogueAgent();
            InitEncounterCalculationVariables();
            StaticReferences.BGM.PlaySong(overworldBGM);
            InitEncounterSkillsVariables();
            DisableUpdateIfSafeArea();
        }

        private void InitPlayerPositionVariables() {
            playerObj = CharacterOW.MainOWCharacter?.transform;
            lastCheckPosition = playerObj ? (Vector2)playerObj.position : Vector2.zero;
            lastPosition = lastCheckPosition;
        }

        private void FindDialogueAgent() {
            if (agent || !CharacterOW.MainOWCharacter) {
                return;
            }
            agent = CharacterOW.MainOWCharacter.GetComponent<Fog.Dialogue.Agent>();
        }

        private void InitEncounterCalculationVariables() {
            distanceWalked = 0f;
            ReadEncounterRateSOParameters();
            curEncounterRate = baseEncounterRate;
        }

        private void ReadEncounterRateSOParameters() {
            if (encounterRate == null) {
                return;
            }
            baseEncounterRate = encounterRate.BaseEncounterRate;
            rateIncreaseValue = encounterRate.RateIncreaseValue;
            distanceWalked = encounterRate.MinFreeWalkDistance - encounterRate.FreeWalkDistance;
        }

        private void InitEncounterSkillsVariables() {
            encounterIncDist = encounterIncreaseSkill ? encounterIncreaseSkill.effects[0].value2 : 0;
            EncounterIncDistWalked = encounterIncDist;
            encounterDecDist = encounterDecreaseSkill ? encounterDecreaseSkill.effects[0].value2 : 0;
            EncounterDecDistWalked = encounterDecDist;
            skillModifier = 1.0f;
        }

        private void DisableUpdateIfSafeArea() {
            if ((curEncounterRate >= float.Epsilon || rateIncreaseValue >= float.Epsilon)
                && mapEncounterList != null) {
                return;
            }
            curEncounterRate = 0;
            rateIncreaseValue = 0;
            isSafeArea = true;
        }
        #endregion

        #region Checking Encounters
        // A checagem precisa acontecer no LateUpdate para evitar conflito com o update do sistema de dialogo
        private void LateUpdate() {
            if (ShouldCheckEncounter()) {
                float distance = CalculateDistanceWalked();
                CheckDistanceTresholdAndEncounter(distance);
            }
        }

        private bool ShouldCheckEncounter() {
            return encountersEnabled && CharacterOW.PartyCanMove && (agent == null || agent.CanInteract);
        }

        private float CalculateDistanceWalked() {
            if (playerObj == null) {
                return 0f;
            }
            return Vector2.Distance(lastPosition, playerObj.position);
        }

        private void CheckDistanceTresholdAndEncounter(float distance) {
            distanceWalked += distance;
            if (distanceWalked < distanceTreshold) {
                lastPosition = playerObj.position;
                return;
            }
            if (CheckEncounter(distanceWalked)) {
                return;
            }
            UpdateSkillDistances();
            UpdatePositions();
            distanceWalked = 0f;
        }

        // A chamada da função espera que o valor de distance seja 1.0f
        // Se houver frame drop e o jogador andar distancias maiores do que deveria sem checar,
        // A proxima checagem de batalha terá uma chance maior pois o incremento é linear
        private bool CheckEncounter(float distance) {
            if (isSafeArea || !mapEncounterList.HasEncounterGroup) {
                return false;
            }
            bool foundBattle = RollForBattle();
            if (!foundBattle) {
                curEncounterRate += rateIncreaseValue * distance;
                return false;
            }
            BlockMovementAndInteractions();
            EncounterGroup result = CalculateEncounterGroup();
            SetupAndStartBattle(result);
            return true;
        }

        private bool RollForBattle() {
            if (curEncounterRate * skillModifier == 0f) {
                return false;
            }
            return Random.Range(0.0f, 100.0f) <= (curEncounterRate * skillModifier);
        }

        private void BlockMovementAndInteractions() {
            CharacterOW.PartyCanMove = false;
            if (agent != null) {
                agent.BlockInteractions();
            }
        }

        private EncounterGroup CalculateEncounterGroup() {
            int partyLevel = Party.Instance.ScaledLevel;
            ReadOnlyDictionary<EncounterGroup, float> chanceDict = mapEncounterList.GetChancesForLevel(partyLevel);
            List<EncounterGroup> encounterGroups = new List<EncounterGroup>(chanceDict.Keys);
            encounterGroups.Sort((first, second) => first.DifficultyRating.CompareTo(second.DifficultyRating));
            EncounterGroup result = RollEncounterGroup(encounterGroups, chanceDict);
            return result;
        }

        private EncounterGroup RollEncounterGroup(List<EncounterGroup> orderedGroupList, ReadOnlyDictionary<EncounterGroup, float> chanceDict) {
            float roll = Random.Range(0f, 100.0f);
            EncounterGroup result = orderedGroupList[0];
            float cummulativeChance = 0;
            for (int i = 0; i < orderedGroupList.Count && roll > cummulativeChance; i++) {
                result = orderedGroupList[i];
                cummulativeChance += chanceDict[result];
            }
            return result;
        }

        private void SetupAndStartBattle(EncounterGroup result) {
            // Calculo de level foi movido para Enemy.cs e Party.cs
            // A função é chamada no script de preview LoadEnemiesPreview.cs
            if (encounterDecreaseSkill)
                encounterDecreaseSkill.Deactivate();
            if (encounterIncreaseSkill)
                encounterIncreaseSkill.Deactivate();
            FinalInferno.UI.ChangeSceneUI.isBattle = true;
            FinalInferno.UI.ChangeSceneUI.battleBG = battleBG;
            FinalInferno.UI.ChangeSceneUI.battleBGM = battleBGM;
            FinalInferno.UI.ChangeSceneUI.battleEnemies = result.GetEnemies();
            decision.Click();
        }

        private void UpdateSkillDistances() {
            EncounterIncDistWalked += ((EncounterIncDistWalked < (float.MaxValue - distanceWalked)) ? distanceWalked : 0);
            EncounterDecDistWalked += ((EncounterDecDistWalked < (float.MaxValue - distanceWalked)) ? distanceWalked : 0);
        }

        private void UpdatePositions() {
            lastCheckPosition = playerObj.position;
            lastPosition = playerObj.position;
        }
        #endregion

        #region Encounter Skills Callbacks
        public void OnEnable() {
            if (encounterIncreaseSkill)
                encounterIncreaseSkill.AddActivationListener(this);
            if (encounterDecreaseSkill)
                encounterDecreaseSkill.AddActivationListener(this);
        }

        public void OnDisable() {
            if (encounterIncreaseSkill)
                encounterIncreaseSkill.RemoveActivationListener(this);
            if (encounterDecreaseSkill)
                encounterDecreaseSkill.RemoveActivationListener(this);
        }

        public void ActivatedSkill(OverworldSkill skill) {
            if (skill == null) {
                return;
            }
            if (skill == encounterIncreaseSkill) {
                ActivateEncounterIncreaseSkill(skill);
            } else if (skill == encounterDecreaseSkill) {
                ActivateEncounterDecreaseSkill(skill);
            }
        }

        private void ActivateEncounterIncreaseSkill(OverworldSkill skill) {
            if (encounterDecreaseSkill)
                encounterDecreaseSkill.Deactivate();
            EncounterIncDistWalked = 0;
            skillModifier = skill.effects[0].value1;
        }

        private void ActivateEncounterDecreaseSkill(OverworldSkill skill) {
            if (encounterIncreaseSkill)
                encounterIncreaseSkill.Deactivate();
            EncounterDecDistWalked = 0;
            skillModifier = skill.effects[0].value1;
        }

        public void DeactivatedSkill(OverworldSkill skill) {
            if (skill == encounterDecreaseSkill || skill == encounterIncreaseSkill) {
                skillModifier = 1.0f;
                if (skill == encounterDecreaseSkill) {
                    EncounterDecDistWalked = encounterDecDist;
                } else {
                    EncounterIncDistWalked = encounterIncDist;
                }
            }
        }
        #endregion
    }
}
