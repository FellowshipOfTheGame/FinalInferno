using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno {
#if UNITY_EDITOR
    [CustomEditor(typeof(EncounterRate))]
    public class EncounterRateEditor : Editor {
        private SerializedProperty baseEncounterRate;
        private SerializedProperty rateIncreaseValue;
        private SerializedProperty freeWalkDistance;
        private float baseRate = float.MinValue;
        private float increase = float.MinValue;
        private float distanceWalked = 0f;
        private float halfDistance = 0f;
        private AnimationCurve curve = null;

        public void OnEnable() {
            FindSerializedFieldProperties();
            InitCurveAndVariables();
        }

        private void FindSerializedFieldProperties() {
            baseEncounterRate = serializedObject.FindProperty("baseEncounterRate");
            rateIncreaseValue = serializedObject.FindProperty("rateIncreaseValue");
            freeWalkDistance = serializedObject.FindProperty("freeWalkDistance");
        }

        private void InitCurveAndVariables() {
            baseRate = baseEncounterRate.floatValue;
            increase = rateIncreaseValue.floatValue;
            distanceWalked = freeWalkDistance.intValue;
            halfDistance = distanceWalked;
            curve = new AnimationCurve();
            curve.AddKey(0f, 0f);
        }

        public override void OnInspectorGUI() {
            DrawScriptableObjectFields();
            InitCurveAndVariables();
            float successChance = 0;
            if (CurveHasMultiplePoints()) {
                successChance = CalculateCurvePoints();
            }
            WriteCalculatedEncounterValues();
            EditorGUILayout.Space();
            DrawAccumulatedEncounterRateGraph(successChance);
        }

        private void DrawScriptableObjectFields() {
            serializedObject.Update();
            EditorGUILayout.PropertyField(baseEncounterRate);
            EditorGUILayout.PropertyField(rateIncreaseValue);
            EditorGUILayout.PropertyField(freeWalkDistance);
            serializedObject.ApplyModifiedProperties();
        }

        private bool CurveHasMultiplePoints() {
            return baseRate > float.Epsilon || increase > float.Epsilon;
        }

        private float CalculateCurvePoints() {
            int steps = 0;
            float successChance = Mathf.Clamp(1f - (baseRate / 100f), 0f, 1f);
            while (successChance >= 0.05f && steps < 999) {
                AddNewPointToCurve(steps, successChance);
                if (successChance >= 0.5f) {
                    halfDistance = distanceWalked;
                }
                baseRate += increase;
                distanceWalked += 1.0f;
                successChance *= Mathf.Clamp((1f - (baseRate / 100f)), 0f, 1f);
                steps++;
            }
            if (steps > 0) {
                AddLastCurvePoint(successChance);
            }
            return successChance;
        }

        private void AddNewPointToCurve(int steps, float successChance) {
            curve.AddKey(distanceWalked, 100f - (successChance * 100f));
            if (steps == 0) {
                AnimationUtility.SetKeyRightTangentMode(curve, curve.keys.Length - 2, AnimationUtility.TangentMode.Constant);
                AnimationUtility.SetKeyLeftTangentMode(curve, curve.keys.Length - 2, AnimationUtility.TangentMode.Constant);
                AnimationUtility.SetKeyLeftTangentMode(curve, curve.keys.Length - 1, AnimationUtility.TangentMode.Constant);
            } else {
                AnimationUtility.SetKeyLeftTangentMode(curve, curve.keys.Length - 1, AnimationUtility.TangentMode.Linear);
            }
            AnimationUtility.SetKeyRightTangentMode(curve, curve.keys.Length - 1, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyBroken(curve, curve.keys.Length - 2, true);
            AnimationUtility.SetKeyBroken(curve, curve.keys.Length - 1, true);
        }

        private void AddLastCurvePoint(float successChance) {
            curve.AddKey(distanceWalked, 100f - (successChance * 100f));
            AnimationUtility.SetKeyLeftTangentMode(curve, curve.keys.Length - 1, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(curve, curve.keys.Length - 1, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyBroken(curve, curve.keys.Length - 2, true);
            AnimationUtility.SetKeyBroken(curve, curve.keys.Length - 1, true);
        }

        private void WriteCalculatedEncounterValues() {
            EditorGUILayout.LabelField("(1.0 units is 1 tile)");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"Distance walked 50% of the time:{((baseRate > float.Epsilon) ? Mathf.Min(halfDistance, 999) : 999)}{((baseRate > float.Epsilon && halfDistance <= 999) ? "" : "+")}", GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField($"Distance walked  5% of the time:{((baseRate > float.Epsilon) ? Mathf.Min(distanceWalked, 999) : 999)}{((baseRate > float.Epsilon && distanceWalked <= 999) ? "" : "+")}", GUILayout.ExpandWidth(true));
        }

        private void DrawAccumulatedEncounterRateGraph(float successChance) {
            EditorGUILayout.LabelField("Accumulated encounter chance per steps taken:");
            GUIStyle style = new GUIStyle(EditorStyles.label);
            style.alignment = TextAnchor.UpperRight;
            EditorGUILayout.LabelField("", $"{((distanceWalked <= 999) ? 95 : (100f - (successChance * 100f))):##.##}%", style);
            GUI.enabled = false;
            EditorGUILayout.CurveField(curve, GUILayout.MinHeight(200f));
            GUI.enabled = true;
            EditorGUILayout.LabelField("0", ((baseRate > float.Epsilon && distanceWalked <= 999) ? $"{distanceWalked}" : "999+"), style);
        }
    }
#endif
}
