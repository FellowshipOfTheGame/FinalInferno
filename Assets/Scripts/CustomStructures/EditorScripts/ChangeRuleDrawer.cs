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
        private const float marginSize = 5f;
        private QuestEventField questEventField = new QuestEventField();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            float questEventFieldHeight = questEventField.GetFieldHeight(property);
            Animator anim = Selection.activeGameObject.GetComponent<Animator>();
            float animFieldHeight = (anim != null && anim.runtimeAnimatorController != null) ? EditorGUIUtility.singleLineHeight : 0;
            return questEventFieldHeight + animFieldHeight + (2 * marginSize);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            FindSerializedStructProperties(property);
            position.y += marginSize;
            questEventField.DrawQuestEventField(position);
            DrawAnimationFlagFieldIfPossible(position, property);
            EditorGUI.EndProperty();
        }

        private void FindSerializedStructProperties(SerializedProperty property) {
            questEventField.FindSerializedStructProperties(property);
            animationFlag = property.FindPropertyRelative("animationFlag");
            toggleValue = property.FindPropertyRelative("newValue");
        }

        private void DrawAnimationFlagFieldIfPossible(Rect position, SerializedProperty property) {
            MonoBehaviour monoBehaviour = property.serializedObject.targetObject as MonoBehaviour;
            Animator animator = monoBehaviour?.GetComponent<Animator>();
            ReenableAnimator(animator);
            if (HasAnyNullReference(monoBehaviour, animator, property)) {
                DontDrawAnimationFlagField();
                return;
            }
            DrawAnimationFlagField(position, animator);
        }

        private static void ReenableAnimator(Animator animator) {
            // Não sei pq precisa dessa função, mas precisa dessa função
            if(animator != null){
                animator.enabled = false;
                animator.enabled = true;
            }
        }

        private bool HasAnyNullReference(MonoBehaviour monoBehaviour, Animator animator, SerializedProperty property) {
            if (monoBehaviour == null) {
                LogMonoBehaviourError(property);
                return true;
            }
            if (animator == null) {
                LogAnimatorError(monoBehaviour);
                return true;
            }
            return animator.runtimeAnimatorController == null;
        }

        private void LogMonoBehaviourError(SerializedProperty property) {
            string errorMessage = $"Property of type ChangeRule was added to a non-MonoBehaviour object\n"; 
            errorMessage += $"Animation flag has been set to empty string";
            Debug.LogError(errorMessage, property.serializedObject.targetObject);
        }

        private void LogAnimatorError(MonoBehaviour monoBehaviour) {
            string errorMessage = $"Property of type ChangeRule could not find Animator component in object {monoBehaviour.gameObject.name}";
            errorMessage += $"Animation flag has been set to empty string";
            Debug.LogError(errorMessage, monoBehaviour.gameObject);
        }

        private void DontDrawAnimationFlagField() {
            animRect = new Rect(questEventField.EventRect);
            animationFlag.stringValue = "";
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