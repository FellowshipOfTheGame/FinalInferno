using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FinalInferno {
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ChangeRule))]
    public class ChangeRuleDrawer : PropertyDrawer {
        private SerializedProperty quest, eventFlag, animationFlag, toggleValue;
        private int questFlagIndex, animationFlagIndex;
        private Rect questRect;
        private Rect eventRect;
        private Rect animRect;
        private Rect toggleRect;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SerializedProperty _quest = property.FindPropertyRelative("quest");
            Animator anim = Selection.activeGameObject.GetComponent<Animator>();
            int i = 1 + ((_quest != null && _quest.objectReferenceValue != null) ? 1 : 0) + ((anim != null && anim.runtimeAnimatorController != null) ? 1 : 0);
            return (i * EditorGUIUtility.singleLineHeight) + 10f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            FindSerializedStructProperties(property);
            DrawQuestField(position);
            DrawEventFlagFieldIfNecessary();
            DrawAnimationFlagFieldIfPossible(position);
            EditorGUI.EndProperty();
        }

        private void FindSerializedStructProperties(SerializedProperty property) {
            quest = property.FindPropertyRelative("quest");
            eventFlag = property.FindPropertyRelative("eventFlag");
            animationFlag = property.FindPropertyRelative("animationFlag");
            toggleValue = property.FindPropertyRelative("newValue");
        }

        private void DrawQuestField(Rect position) {
            questRect = new Rect(position);
            questRect.y += 5f;
            questRect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(questRect, quest);
        }

        private void DrawEventFlagFieldIfNecessary() {
            eventRect = (quest.objectReferenceValue == null) ? questRect : NewRectBelow(questRect);
            if (quest.objectReferenceValue != null) {
                DrawEventFlagField();
            } else {
                eventFlag.stringValue = "";
            }
        }

        private Rect NewRectBelow(Rect rect) {
            Rect returnValue = new Rect(rect);
            returnValue.y += rect.height;
            returnValue.height = EditorGUIUtility.singleLineHeight;
            return returnValue;
        }

        private void DrawEventFlagField() {
            Quest _quest = quest.objectReferenceValue as Quest;
            string[] keys = _quest.FlagNames;
            int indexOfSerializedFlag = System.Array.IndexOf(keys, eventFlag.stringValue);
            questFlagIndex = Mathf.Clamp(indexOfSerializedFlag, 0, Mathf.Max(keys.Length - 1, 0));
            questFlagIndex = EditorGUI.Popup(eventRect, "Event", questFlagIndex, keys);
            eventFlag.stringValue = (keys.Length > 0) ? keys[questFlagIndex] : "";
        }

        private void DrawAnimationFlagFieldIfPossible(Rect position) {
            Animator anim = Selection.activeGameObject.GetComponent<Animator>();
            ReenableAnimator(anim);
            if (anim == null || anim.runtimeAnimatorController == null) {
                LogSelectionErrorIfNecessary(anim);
                animRect = new Rect(eventRect);
                animationFlag.stringValue = "";
            } else {
                DrawAnimationFlagField(position, anim);
            }
        }

        private static void ReenableAnimator(Animator anim) {
            // Não sei pq precisa disso, mas precisa disso
            if(anim != null){
                anim.enabled = false;
                anim.enabled = true;
            }
        }

        private void LogSelectionErrorIfNecessary(Animator anim) {
            if(anim == null) {
                // TO DO: transformar anim em uma propriedade persistente inicializada on enable para evitar isso
                string errorMessage = $"Selected object without animator ({Selection.activeGameObject.name}) while drawing a ChangeRule in inspector\n";
                errorMessage += $"Animation flag has been set to empty string, avoid locking inspectors that draw ChangeRule properties";
                Debug.LogError(errorMessage);
            }
        }

        private void DrawAnimationFlagField(Rect position, Animator anim) {
            CalculateAnimationFlagFieldRects(position);
            DrawParametersPopupField(anim);
            toggleValue.boolValue = EditorGUI.Toggle(toggleRect, "", toggleValue.boolValue);
        }

        private void CalculateAnimationFlagFieldRects(Rect position) {
            animRect = NewRectBelow(eventRect);
            animRect.size = new Vector2(position.size.x - 40, animRect.size.y);
            toggleRect = new Rect(animRect);
            toggleRect.size = new Vector2(position.size.x - animRect.size.x, animRect.size.y);
        }

        private void DrawParametersPopupField(Animator anim) {
            List<string> parameters = GetBoolParameters(anim);
            animationFlagIndex = Mathf.Clamp(parameters.IndexOf(animationFlag.stringValue), 0, Mathf.Max(parameters.Count - 1, 0));
            animationFlagIndex = EditorGUI.Popup(animRect, "Animation flag", animationFlagIndex, parameters.ToArray());
            animationFlag.stringValue = (parameters.Count > 0) ? parameters[animationFlagIndex] : "";
        }

        private static List<string> GetBoolParameters(Animator anim) {
            AnimatorControllerParameter[] allParamaters = anim.parameters;
            List<string> parameters = new List<string>();
            foreach (AnimatorControllerParameter param in allParamaters) {
                if (param.type == AnimatorControllerParameterType.Bool) {
                    parameters.Add(param.name);
                }
            }
            return parameters;
        }
    }

#endif

}