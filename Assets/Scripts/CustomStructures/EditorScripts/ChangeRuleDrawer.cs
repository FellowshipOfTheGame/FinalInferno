using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FinalInferno {
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ChangeRule))]
    public class ChangeRuleDrawer : PropertyDrawer {
        private SerializedProperty animationFlag, toggleValue;
        private int animationFlagIndex;
        private Rect animRect;
        private Rect toggleRect;
        private const float toggleFieldSize = 40f;
        private QuestEventField questEventField = new QuestEventField();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            float questEventFieldHeight = questEventField.GetFieldHeight(property);
            Animator anim = Selection.activeGameObject.GetComponent<Animator>();
            float animFieldHeight = (anim != null && anim.runtimeAnimatorController != null) ? EditorGUIUtility.singleLineHeight : 0;
            return questEventFieldHeight + animFieldHeight + 10f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            FindSerializedStructProperties(property);
            position.y += 5f;
            questEventField.DrawQuestEventField(position);
            DrawAnimationFlagFieldIfPossible(position);
            EditorGUI.EndProperty();
        }

        private void FindSerializedStructProperties(SerializedProperty property) {
            questEventField.FindSerializedStructProperties(property);
            animationFlag = property.FindPropertyRelative("animationFlag");
            toggleValue = property.FindPropertyRelative("newValue");
        }

        private void DrawAnimationFlagFieldIfPossible(Rect position) {
            Animator anim = Selection.activeGameObject.GetComponent<Animator>();
            ReenableAnimator(anim);
            if (anim == null || anim.runtimeAnimatorController == null) {
                LogSelectionErrorIfNecessary(anim);
                animRect = new Rect(questEventField.EventRect);
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
            animRect = EditorUtils.NewRectBelow(questEventField.EventRect);
            animRect.size = new Vector2(position.size.x - toggleFieldSize, animRect.size.y);
            toggleRect = new Rect(animRect);
            toggleRect.x += animRect.size.x;
            toggleRect.size = new Vector2(toggleFieldSize, animRect.size.y);
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