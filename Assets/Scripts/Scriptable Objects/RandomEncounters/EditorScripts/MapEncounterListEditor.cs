using System.Collections;
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
		private int selectedLevel = 0;
		float accumulatedDifficulty = 0;
		List<EncounterGroup> validEncounterGroups;
		private const float PORTRAIT_SIZE = 48f;

		public void OnEnable(){
			encounterGroups = serializedObject.FindProperty("encounterGroups");
			selectedLevel = -1;
			validEncounterGroups = new List<EncounterGroup>();
			accumulatedDifficulty = 0;
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			ShowPreviews();

			EditorGUILayout.Space(20f);

			EditorGUILayout.PropertyField(encounterGroups);

			serializedObject.ApplyModifiedProperties();
		}

		private void ShowPreviews(){
			EditorGUILayout.BeginHorizontal(EditorStyles.boldLabel);
			// Level selection
			int previousLevel = selectedLevel;
			for(int i = 0; i < 5; i++){
				if(GUILayout.Button($"level{i}")){
					selectedLevel = i;
				}
			}
			selectedLevel = Mathf.Clamp(selectedLevel, 0, 4);
			bool selectedLevelChanged = selectedLevel != previousLevel;
			EditorGUILayout.EndHorizontal();

			// Dsiplay selected level
			EditorGUILayout.LabelField($"Selected level: {selectedLevel}", EditorStyles.boldLabel);
			EditorGUILayout.Space();

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			EditorGUILayout.Space();
			// Calculate values if needed
			accumulatedDifficulty = selectedLevelChanged? 0 : accumulatedDifficulty;
			if(selectedLevelChanged) validEncounterGroups.Clear();

			// Repopulate encounter list if needed
			for(int i = 0; selectedLevelChanged && i < encounterGroups.arraySize; i++){
				SerializedProperty groupProp = encounterGroups.GetArrayElementAtIndex(i);
				if(groupProp == null || groupProp.objectReferenceValue == null) continue;

				SerializedObject obj = new SerializedObject(groupProp.objectReferenceValue);
				if(!obj.FindProperty("canEncounter").GetArrayElementAtIndex(selectedLevel).boolValue) continue;

				EncounterGroup encounterGroup = obj.targetObject as EncounterGroup;
				accumulatedDifficulty += encounterGroup.DifficultyRating;
				validEncounterGroups.Add(encounterGroup);
			}
			// Calculate chances as needed
			if(selectedLevelChanged){
				validEncounterGroups.Sort((first, second) => first.DifficultyRating.CompareTo(second.DifficultyRating));
				accumulatedDifficulty = accumulatedDifficulty == 0? 1f : accumulatedDifficulty;
			}

			// Display info on all encounter groups as calculated
			bool isFirst = true;
			foreach(EncounterGroup encounterGroup in validEncounterGroups){
				if(!isFirst) EditorUtils.DrawSeparator(EditorGUILayout.GetControlRect());
				isFirst = false;

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField($"Chance = {((encounterGroup.DifficultyRating/accumulatedDifficulty)*100):##0.##}%", GUILayout.MinHeight(PORTRAIT_SIZE), GUILayout.MaxWidth(100f));
				// Show encounter info
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

			EditorGUILayout.Space();
			EditorGUILayout.EndVertical();
		}
	}
#endif
}
