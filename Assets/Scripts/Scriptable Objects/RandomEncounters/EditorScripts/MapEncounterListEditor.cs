using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno {
#if UNITY_EDITOR
    [CustomEditor(typeof(MapEncounterList))]
    public class MapEncounterListEditor : Editor {
        private SerializedProperty encounterGroups;
        private SerializedProperty difficultyFactor;
        private int selectedLevelIndex = 0;
        private float selectedDifficultyFactor = 0f;
        private List<EncounterGroup> validEncounterGroups;
        private List<float> storedMultipliers;
        private ReadOnlyDictionary<EncounterGroup, float> chancesDict;
        private const float PORTRAIT_SIZE = 48f;
        private const float SMALL_LABEL_WIDTH = 20f;
        private const float previewSpacing = 20f;
        private const float portraitSpacing = 20f;
        private const float chanceLabelWidth = 100f;

        public void OnEnable() {
            FindSerializedFieldProperties();
            InitVariables();
        }

        private void FindSerializedFieldProperties() {
            encounterGroups = serializedObject.FindProperty("encounterGroupItems");
            difficultyFactor = serializedObject.FindProperty("difficultyFactor");
            SaveMultipliers();
        }

        private void SaveMultipliers() {
            storedMultipliers = new List<float>();
            for (int i = 0; i < encounterGroups.arraySize; i++) {
                storedMultipliers.Add(encounterGroups.GetArrayElementAtIndex(i).FindPropertyRelative("chanceMultiplier").floatValue);
            }
        }

        private void InitVariables() {
            selectedLevelIndex = -1;
            selectedDifficultyFactor = difficultyFactor.floatValue;
            validEncounterGroups = new List<EncounterGroup>();
            chancesDict = new ReadOnlyDictionary<EncounterGroup, float>(new Dictionary<EncounterGroup, float>());
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            ShowPreviews();
            EditorGUILayout.Space(previewSpacing);
            EditorGUILayout.PropertyField(encounterGroups);
            serializedObject.ApplyModifiedProperties();
        }

        private void ShowPreviews() {
            float previousDifficultyFactor = selectedDifficultyFactor;
            DisplayDifficultyFactorButtons();
            EditorGUILayout.Space();
            int previousIndex = selectedLevelIndex;
            DisplayLevelSelectionButtons();
            EditorGUILayout.Space();
            bool parametersChanged = CheckForParameterChanges(previousDifficultyFactor, previousIndex);
            UpdateAndShowSelectedPreview(parametersChanged);
        }

        private void DisplayDifficultyFactorButtons() {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Difficulty Factor", EditorStyles.boldLabel);
            DisplayDifficultyFactorDecreaseButton();
            EditorGUILayout.LabelField($"{(selectedDifficultyFactor):0.0}", GUILayout.MaxWidth(SMALL_LABEL_WIDTH));
            DisplayDifficultyFactorIncreaseButton();
            ApplyDifficultyFactorChanges();
            EditorGUILayout.EndHorizontal();
        }

        private void DisplayDifficultyFactorDecreaseButton() {
            if (GUILayout.Button("-", GUILayout.MaxWidth(SMALL_LABEL_WIDTH)))
                selectedDifficultyFactor = Mathf.Max(0.0f, difficultyFactor.floatValue - 0.1f);
        }

        private void DisplayDifficultyFactorIncreaseButton() {
            if (GUILayout.Button("+", GUILayout.MaxWidth(SMALL_LABEL_WIDTH)))
                selectedDifficultyFactor = Mathf.Min(1f, difficultyFactor.floatValue + 0.1f);
        }

        private void ApplyDifficultyFactorChanges() {
            difficultyFactor.floatValue = selectedDifficultyFactor;
            serializedObject.ApplyModifiedProperties();
        }

        private void DisplayLevelSelectionButtons() {
            EditorGUILayout.BeginHorizontal(EditorStyles.boldLabel);
            for (int level = 1; level <= 5; level++) {
                if (GUILayout.Button($"level {level}"))
                    selectedLevelIndex = level - 1;
            }
            EditorGUILayout.EndHorizontal();
            selectedLevelIndex = Mathf.Clamp(selectedLevelIndex, 0, 4);
            EditorGUILayout.LabelField($"Selected level: {selectedLevelIndex + 1}", EditorStyles.boldLabel);
        }

        private bool CheckForParameterChanges(float previousDifficultyFactor, int previousIndex) {
            bool parametersChanged = selectedLevelIndex != previousIndex;
            parametersChanged |= (Mathf.Abs(previousDifficultyFactor - selectedDifficultyFactor) > Mathf.Epsilon);
            parametersChanged |= CheckMultiplierChanges();
            return parametersChanged;
        }

        private bool CheckMultiplierChanges() {
            bool hasChanged = encounterGroups.arraySize != storedMultipliers.Count;
            for (int index = 0; !hasChanged && index < encounterGroups.arraySize; index++) {
                float currentMultiplier = GetSerializedMultiplierAtIndex(index);
                hasChanged = storedMultipliers[index] != currentMultiplier;
            }
            if (hasChanged)
                SaveMultipliers();
            return hasChanged;
        }

        private float GetSerializedMultiplierAtIndex(int index) {
            return encounterGroups.GetArrayElementAtIndex(index).FindPropertyRelative("chanceMultiplier").floatValue;
        }

        private void UpdateAndShowSelectedPreview(bool parametersChanged) {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.Space();
            if (parametersChanged) {
                ResetListAndDictionary();
                RepopulateValidEncountersList();
                RecalculateEncounterChances();
            }
            DisplayEncounterInfo();
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        private void ResetListAndDictionary() {
            validEncounterGroups.Clear();
            chancesDict = (target as MapEncounterList).GetChancesForLevel(selectedLevelIndex + 1);
        }

        private void RepopulateValidEncountersList() {
            for (int index = 0; index < encounterGroups.arraySize; index++) {
                SerializedProperty groupProp = GetGroupPropAtIndex(index);
                if (groupProp == null || groupProp.objectReferenceValue == null)
                    continue;

                SerializedObject obj = new SerializedObject(groupProp.objectReferenceValue);
                if (!obj.FindProperty("canEncounter").GetArrayElementAtIndex(selectedLevelIndex).boolValue)
                    continue;

                EncounterGroup encounterGroup = obj.targetObject as EncounterGroup;
                validEncounterGroups.Add(encounterGroup);
            }
        }

        private SerializedProperty GetGroupPropAtIndex(int index) {
            SerializedProperty itemAtIndex = encounterGroups.GetArrayElementAtIndex(index);
            return itemAtIndex?.FindPropertyRelative("group");
        }

        private void RecalculateEncounterChances() {
            validEncounterGroups.Sort((first, second) => chancesDict[first].CompareTo(chancesDict[second]));
        }

        private void DisplayEncounterInfo() {
            bool isFirst = true;
            foreach (EncounterGroup encounterGroup in validEncounterGroups) {
                DrawSeparatorIfNecessary(isFirst);
                isFirst = false;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Chance = {((chancesDict[encounterGroup])):##0.##}%",
                                            GUILayout.MinHeight(PORTRAIT_SIZE),
                                            GUILayout.MaxWidth(chanceLabelWidth));
                DrawEncounterGroupEnemies(encounterGroup);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.LabelField($"\"{encounterGroup.name}\"", EditorStyles.boldLabel);
            }
        }

        private static void DrawSeparatorIfNecessary(bool isFirst) {
            if (!isFirst)
                EditorUtils.DrawSeparator(EditorGUILayout.GetControlRect());
        }

        private static void DrawEncounterGroupEnemies(EncounterGroup encounterGroup) {
            Rect currentRect = EditorGUILayout.GetControlRect();
            for (int index = 0; index < 4; index++) {
                if (encounterGroup[index] == null)
                    continue;
                currentRect = DrawEnemyPortrait(encounterGroup, currentRect, index);
            }
        }

        private static Rect DrawEnemyPortrait(EncounterGroup encounterGroup, Rect currentRect, int index) {
            Vector2 position = currentRect.position + new Vector2(index * (portraitSpacing + PORTRAIT_SIZE), 0f);
            EditorGUI.DrawTextureTransparent(new Rect(position, new Vector2(PORTRAIT_SIZE, PORTRAIT_SIZE)),
                                            EditorUtils.GetCroppedTexture(encounterGroup[index].Portrait),
                                            ScaleMode.ScaleToFit);
            return currentRect;
        }
    }
#endif
}
