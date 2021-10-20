using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno{
#if UNITY_EDITOR
    public static class EditorUtils {

        public static Rect DrawSeparator(Rect position, float thickness= 2f, float padding = 5f){
            Rect lineRect = new Rect(position.x, position.y + padding + thickness, position.width, thickness);
            EditorGUI.DrawRect(lineRect, Color.grey);

            position.position = new Vector2(position.x, position.y + padding * 2f + thickness);

            return position;
        }

        public static Texture2D GetCroppedTexture(Sprite sprite){
            Texture2D croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                      (int)sprite.textureRect.y,
                                                      (int)sprite.textureRect.width,
                                                      (int)sprite.textureRect.height);
            croppedTexture.SetPixels(pixels);
            croppedTexture.Apply();

            return croppedTexture;
        }
    }
#endif
}
