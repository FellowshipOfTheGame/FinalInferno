using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno {
#if UNITY_EDITOR
    [CustomPreview(typeof(Unit))]
    public class UnitPreview : ObjectPreview {
        protected const float scaleFactor = 0.8f;
        protected Texture2D tex = null;
        protected Texture2D bg = null;

        public override GUIContent GetPreviewTitle() {
            return new GUIContent("Battle Preview");
        }

        public override bool HasPreviewGUI() {
            Unit unit = target as Unit;
            return unit && unit.BattleSprite;
        }

        public override void OnPreviewGUI(Rect Rect, GUIStyle background) {
            Unit unit = target as Unit;
            if (unit == null) {
                return;
            }
            InitTextures(unit);
            Rect texRect = CalculateTextureRect(Rect);
            EditorGUI.DrawTextureTransparent(texRect, bg, ScaleMode.StretchToFill);
            GUI.DrawTexture(texRect, tex, ScaleMode.ScaleToFit);
            DrawHeadIndicator(unit, texRect);
        }

        private void InitTextures(Unit unit) {
            if (tex != null && bg != null) {
                return;
            }
            tex = EditorUtils.GetCroppedTexture(unit.BattleSprite);
            CreateBackgroundTexture();
        }

        private void CreateBackgroundTexture() {
            Color32[] transparency = CreateTransparentColorArray(tex.width * tex.height);
            bg = new Texture2D(tex.width, tex.height, tex.format, false, false);
            bg.SetPixels32(transparency);
            bg.Apply();
        }

        private static Color32[] CreateTransparentColorArray(int length) {
            Color32[] transparency = new Color32[length];
            for (int i = 0; i < transparency.Length; i++) {
                transparency[i] = Color.clear;
            }
            return transparency;
        }

        private Rect CalculateTextureRect(Rect rect) {
            float aspectRatio = tex.height / (float)tex.width;
            if (ShouldScaleUsingWidth(rect, aspectRatio)) {
                return CalculateRectScalingToWidth(rect, aspectRatio);
            } else {
                return CalculateRectScalingToHeight(rect, aspectRatio);
            }
        }

        private bool ShouldScaleUsingWidth(Rect rect, float aspectRatio) {
            return tex.width > tex.height && rect.height > aspectRatio * rect.width;
        }

        private static Rect CalculateRectScalingToWidth(Rect rect, float aspectRatio) {
            float scaledWidth = scaleFactor * rect.width;
            float xPosition = rect.center.x - (scaledWidth / 2.0f);
            float yPosition = rect.center.y - (scaledWidth / 2.0f) * aspectRatio;
            return new Rect(xPosition, yPosition, scaledWidth, scaledWidth * aspectRatio);
        }

        private static Rect CalculateRectScalingToHeight(Rect rect, float aspectRatio) {
            float scaledHeight = scaleFactor * rect.height;
            float xPosition = rect.center.x - (scaledHeight / 2.0f) / aspectRatio;
            float yPosition = rect.center.y - (scaledHeight / 2.0f);
            return new Rect(xPosition, yPosition, scaledHeight / aspectRatio, scaledHeight);
        }

        protected virtual void DrawHeadIndicator(Unit unit, Rect texRect) {
            float rectSize = 0.1f * Mathf.Max(texRect.width, texRect.height);
            Vector2 headRectPosition = CalculateHeadCenterPosition(unit, texRect) - new Vector2(rectSize / 2, rectSize / 2);
            Rect headRect = new Rect(headRectPosition.x, headRectPosition.y, rectSize, rectSize);
            EditorGUI.DrawRect(headRect, new Color(0f, 1f, 0f, .7f));
        }

        protected static Vector2 CalculateHeadCenterPosition(Unit unit, Rect texRect) {
            float xPosition = texRect.x + (unit.EffectsRelativePosition.x * texRect.width);
            float yPosition = texRect.yMax - (unit.EffectsRelativePosition.y * texRect.height);
            return new Vector2(xPosition, yPosition);
        }
    }
#endif
}
