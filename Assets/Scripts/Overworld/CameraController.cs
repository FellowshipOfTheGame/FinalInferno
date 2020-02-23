using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FinalInferno{
    [RequireComponent(typeof(Camera)), RequireComponent(typeof(UnityEngine.U2D.PixelPerfectCamera))]
    public class CameraController : MonoBehaviour
    {
        //private Grid grid;
        private Tilemap[] tilemaps = null;
        private Vector2 lowerBounds;
        private Vector2 upperBounds;
        private float cameraHalfHeight;
        private new Camera camera;
        [SerializeField] private Transform target;
        [SerializeField] private float freedomDistance = 2;
        [SerializeField] private float smoothing = 0.6f;

        private void UpdateBounds(){
            tilemaps = FindObjectsOfType<Tilemap>();
            if(tilemaps != null && tilemaps.Length > 0){
                float tilemapMinX = float.MaxValue;
                float tilemapMinY = float.MaxValue;
                float tilemapMaxX = float.MinValue;
                float tilemapMaxY = float.MinValue;
                foreach(Tilemap tilemap in tilemaps){
                    Vector3 minPoint = tilemap.transform.TransformPoint(tilemap.localBounds.min);
                    Vector3 maxPoint = tilemap.transform.TransformPoint(tilemap.localBounds.max);
                    if(minPoint.x < tilemapMinX)
                        tilemapMinX = minPoint.x;
                    if(minPoint.y < tilemapMinY)
                        tilemapMinY = minPoint.y;
                    if(maxPoint.x > tilemapMaxX)
                        tilemapMaxX = maxPoint.x;
                    if(maxPoint.y > tilemapMaxY)
                        tilemapMaxY = maxPoint.y;
                }
                cameraHalfHeight = camera.orthographicSize;
                float cameraHalfWidth = cameraHalfHeight * camera.aspect;
                //Debug.Log("Camera size: " + cameraHalfWidth + "," + cameraHalfHeight);
                lowerBounds = new Vector2(tilemapMinX + cameraHalfWidth, tilemapMinY + cameraHalfHeight);
                upperBounds = new Vector2(tilemapMaxX - cameraHalfWidth, tilemapMaxY - cameraHalfHeight);
                
                //Debug.Log("lowerBounds = " + lowerBounds);
                //Debug.Log("upperBounds = " + upperBounds);
            }else{
                lowerBounds = new Vector2(float.MinValue, float.MinValue);
                upperBounds = new Vector2(float.MaxValue, float.MaxValue);
            }
        }

        void Awake(){
            camera = GetComponent<Camera>();
            UpdateBounds();
        }

        // Start is called before the first frame update
        void Start()
        {
            // Pixel Perfect camera altera o tamanho da camera depois do awake, então essa checagem é necessaria
            if(cameraHalfHeight != camera.orthographicSize)
                UpdateBounds();
            target = CharacterOW.MainOWCharacter?.transform;
        }

        void LateUpdate(){
            // Pixel Perfect camera altera o tamanho da camera depois do awake, então essa checagem é necessaria
            if(cameraHalfHeight != camera.orthographicSize)
                UpdateBounds();

            if(target == null){
                Vector2 samePosition = new Vector2(transform.position.x, transform.position.y);
                samePosition.x = Mathf.Clamp(samePosition.x, lowerBounds.x, upperBounds.x);
                samePosition.y = Mathf.Clamp(samePosition.y, lowerBounds.y, upperBounds.y);
                transform.position = new Vector3(samePosition.x, samePosition.y, transform.position.z);
            }else{
                Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
                Vector2 targetPosition = new Vector2(target.position.x, target.position.y);
                Vector2 newPosition;
                float distance = Vector2.Distance(currentPosition, targetPosition);
                if(distance > freedomDistance){
                    targetPosition -= currentPosition;
                    targetPosition.Normalize();
                    targetPosition *= (distance - freedomDistance);
                    targetPosition += currentPosition;
                    newPosition = Vector2.Lerp(currentPosition, targetPosition, smoothing);
                }else{
                    newPosition = currentPosition;
                }
                newPosition.x = Mathf.Clamp(newPosition.x, lowerBounds.x, upperBounds.x);
                newPosition.y = Mathf.Clamp(newPosition.y, lowerBounds.y, upperBounds.y);
                transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
            }
        }
    }
}
