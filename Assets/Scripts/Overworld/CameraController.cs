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
        [SerializeField] private Transform target;
        [SerializeField] private float freedomDistance = 2;
        [SerializeField] private float smoothing = 0.6f;

        void Awake(){
            Camera camera = GetComponent<Camera>();
            //grid = FindObjectOfType<Grid>();
            tilemaps = FindObjectsOfType<Tilemap>();
            if(tilemaps != null && tilemaps.Length > 0){
                // TO DO: consertar isso aqui
                // Nem sei se essa parte de setar os bounds ta funcionando direito, precisamos definir a resolução do jogo
                float tilemapMinX = float.MaxValue;
                float tilemapMinY = float.MaxValue;
                float tilemapMaxX = float.MinValue;
                float tilemapMaxY = float.MinValue;
                foreach(Tilemap tilemap in tilemaps){
                    if(tilemap.localBounds.min.x < tilemapMinX)
                        tilemapMinX = tilemap.localBounds.min.x;
                    if(tilemap.localBounds.min.y < tilemapMinY)
                        tilemapMinY = tilemap.localBounds.min.y;
                    if(tilemap.localBounds.max.x > tilemapMaxX)
                        tilemapMaxX = tilemap.localBounds.max.x;
                    if(tilemap.localBounds.max.y > tilemapMaxY)
                        tilemapMaxY = tilemap.localBounds.max.y;
                }
                float cameraHalfHeight = camera.orthographicSize;
                float cameraHalfWidth = cameraHalfHeight * camera.aspect;
                Debug.Log(cameraHalfWidth + "," + cameraHalfHeight);
                lowerBounds = new Vector2(tilemapMinX + cameraHalfWidth, tilemapMinY + cameraHalfHeight);
                upperBounds = new Vector2(tilemapMaxX - cameraHalfWidth, tilemapMaxY - cameraHalfHeight);
                
                Debug.Log(lowerBounds);
                Debug.Log(upperBounds);
            }else{
                lowerBounds = new Vector2(float.MinValue, float.MinValue);
                upperBounds = new Vector2(float.MaxValue, float.MaxValue);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            target = CharacterOW.MainOWCharacter.transform;
        }

        void LateUpdate(){
            float distance = Vector2.Distance(transform.position, target.position);
            if(distance > freedomDistance){
                Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
                Vector2 targetPosition = new Vector2(target.position.x, target.position.y);
                targetPosition -= currentPosition;
                targetPosition.Normalize();
                targetPosition *= (distance - freedomDistance);
                targetPosition += currentPosition;
                Vector2 newPosition = Vector2.Lerp(currentPosition, targetPosition, smoothing);
                newPosition.x = Mathf.Clamp(newPosition.x, lowerBounds.x, upperBounds.x);
                newPosition.y = Mathf.Clamp(newPosition.y, lowerBounds.y, upperBounds.y);
                transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
            }
        }
    }
}
