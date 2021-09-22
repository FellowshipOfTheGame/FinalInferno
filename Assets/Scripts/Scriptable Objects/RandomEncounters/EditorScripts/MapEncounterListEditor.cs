using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno{
#if UNITY_EDITOR
    [CustomEditor(typeof(MapEncounterList))]
	public class MapEncounterListEditor : Editor{
		private SerializedProperty encounterGroups;
		private SerializedProperty difficultyFactor;
		private int selectedLevel = 0;
		private float selectedDifficultyFactor = 0f;
		private List<EncounterGroup> validEncounterGroups;
		private List<float> storedMultipliers;
        private ReadOnlyDictionary<EncounterGroup, float> chancesDict;
		private const float PORTRAIT_SIZE = 48f;
		private const float SMALL_LABEL_WIDTH = 20f;

		public void OnEnable(){
			encounterGroups = serializedObject.FindProperty("encounterGroupItems");
			difficultyFactor = serializedObject.FindProperty("difficultyFactor");
			selectedLevel = -1;
			selectedDifficultyFactor = difficultyFactor.floatValue;
			validEncounterGroups = new List<EncounterGroup>();
			storedMultipliers = new List<float>();
			SaveMultipliers();
			chancesDict = new ReadOnlyDictionary<EncounterGroup, float>(new Dictionary<EncounterGroup, float>());
		}

		private void SaveMultipliers(){
			storedMultipliers.Clear();
			for(int i = 0; i < encounterGroups.arraySize; i++){
				storedMultipliers.Add(encounterGroups.GetArrayElementAtIndex(i).FindPropertyRelative("chanceMultiplier").floatValue);
			}
		}

		private bool CheckMultiplierChange(){
			bool hasChanged = encounterGroups.arraySize != storedMultipliers.Count;
			for(int i = 0; !hasChanged && i < encounterGroups.arraySize; i++){
				float currentMultiplier = encounterGroups.GetArrayElementAtIndex(i).FindPropertyRelative("chanceMultiplier").floatValue;
				hasChanged = storedMultipliers[i] != currentMultiplier;
			}
			if(hasChanged) SaveMultipliers();
			return hasChanged;
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			ShowPreviews();

			EditorGUILayout.Space(20f);

			EditorGUILayout.PropertyField(encounterGroups);

			serializedObject.ApplyModifiedProperties();
		}

		private void ShowPreviews(){
			float previousDifficultyFactor = selectedDifficultyFactor;
			DisplayDifficultyFactorButton();
			EditorGUILayout.Space();

			// Seleção de level da party
			int previousLevel = selectedLevel;
			EditorGUILayout.BeginHorizontal(EditorStyles.boldLabel);
			for(int i = 0; i < 5; i++){
				if(GUILayout.Button($"level {i+1}")){
					selectedLevel = i;
				}
			}
			EditorGUILayout.EndHorizontal();

			// Mostra o level selecionado
			EditorGUILayout.LabelField($"Selected level: {selectedLevel+1}", EditorStyles.boldLabel);
			EditorGUILayout.Space();

			selectedLevel = Mathf.Clamp(selectedLevel, 0, 4);
			bool parametersChanged = selectedLevel != previousLevel;
			parametersChanged |= (Mathf.Abs(previousDifficultyFactor - selectedDifficultyFactor) > Mathf.Epsilon);
			parametersChanged |= CheckMultiplierChange();

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			EditorGUILayout.Space();
			// Calcula os valores apenas caso precise
			if(parametersChanged){
				validEncounterGroups.Clear();
				chancesDict = (target as MapEncounterList).GetChancesForLevel(selectedLevel);
			}

			// Repopula a lista de encontros validos caso precise
			for(int i = 0; parametersChanged && i < encounterGroups.arraySize; i++){
				SerializedProperty groupProp = GetGroupPropAtIndex(i);
				if(groupProp == null || groupProp.objectReferenceValue == null) continue;

				SerializedObject obj = new SerializedObject(groupProp.objectReferenceValue);
				if(!obj.FindProperty("canEncounter").GetArrayElementAtIndex(selectedLevel).boolValue) continue;

				EncounterGroup encounterGroup = obj.targetObject as EncounterGroup;
				validEncounterGroups.Add(encounterGroup);
			}
			// Calcula as chances caso precise
			if(parametersChanged){
				validEncounterGroups.Sort((first, second) => first.DifficultyRating.CompareTo(second.DifficultyRating));
			}

			// Mostra a informação calculada de todos os encontros para o level selecionado
			DisplayEncounterInfo();

			EditorGUILayout.Space();
			EditorGUILayout.EndVertical();
		}

		private SerializedProperty GetGroupPropAtIndex(int index){
			SerializedProperty itemAtIndex = encounterGroups.GetArrayElementAtIndex(index);
			return itemAtIndex?.FindPropertyRelative("group");
		}

		private void DisplayDifficultyFactorButton(){
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Difficulty Factor", EditorStyles.boldLabel);
			if(GUILayout.Button("-", GUILayout.MaxWidth(SMALL_LABEL_WIDTH))){
				selectedDifficultyFactor = Mathf.Max(0.0f, difficultyFactor.floatValue - 0.1f);
			}
			EditorGUILayout.LabelField($"{(selectedDifficultyFactor):0.0}", GUILayout.MaxWidth(SMALL_LABEL_WIDTH));
			if(GUILayout.Button("+", GUILayout.MaxWidth(SMALL_LABEL_WIDTH))){
				selectedDifficultyFactor = Mathf.Min(1f, difficultyFactor.floatValue + 0.1f);
			}
			difficultyFactor.floatValue = selectedDifficultyFactor;
			serializedObject.ApplyModifiedProperties();
			EditorGUILayout.EndHorizontal();
		}

		private void DisplayEncounterInfo(){
			bool isFirst = true;
			foreach(EncounterGroup encounterGroup in validEncounterGroups){
				if(!isFirst) EditorUtils.DrawSeparator(EditorGUILayout.GetControlRect());
				isFirst = false;

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField($"Chance = {((chancesDict[encounterGroup])):##0.##}%", GUILayout.MinHeight(PORTRAIT_SIZE), GUILayout.MaxWidth(100f));
				// Mostra informação do encontro
				Rect currentRect = EditorGUILayout.GetControlRect();
				for(int i = 0; i < 4; i++){
					if(encounterGroup[i] == null) continue;
					Vector2 position = new Vector2(currentRect.position.x + i * (20f + PORTRAIT_SIZE), currentRect.position.y);
					EditorGUI.DrawTextureTransparent(new Rect(position, new Vector2(PORTRAIT_SIZE, PORTRAIT_SIZE)),
													EditorUtils.GetCroppedTexture(encounterGroup[i].Portrait),
													ScaleMode.ScaleToFit);
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.LabelField($"\"{encounterGroup.name}\"", EditorStyles.boldLabel);
			}
		}
	}
#endif
}
