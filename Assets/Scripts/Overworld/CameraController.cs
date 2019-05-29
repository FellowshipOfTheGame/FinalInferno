using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FinalInferno{
    [RequireComponent(typeof(Camera)), RequireComponent(typeof(UnityEngine.U2D.PixelPerfectCamera))]
    public class CameraController : MonoBehaviour
    {
        //private Grid grid;
        private Tilemap tilemap;
        private Vector2 lowerBounds;
        private Vector2 upperBounds;
        [SerializeField] private Transform target;
        [SerializeField] private float freedomDistance = 2;
        [SerializeField] private float smoothing = 0.6f;

        void Awake(){
            Camera camera = GetComponent<Camera>();
            //grid = FindObjectOfType<Grid>();
            tilemap = FindObjectOfType<Tilemap>();
            if(tilemap){
                float cameraHalfHeight = camera.orthographicSize;
                float cameraHalfWidth = (camera.orthographicSize * Screen.width) / Screen.height;
                lowerBounds = new Vector2(tilemap.localBounds.min.x + cameraHalfWidth, tilemap.localBounds.min.y + cameraHalfHeight);
                upperBounds = new Vector2(tilemap.localBounds.max.x - cameraHalfWidth, tilemap.localBounds.max.y - cameraHalfHeight);
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
