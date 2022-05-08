using UnityEngine;
using UnityEngine.Tilemaps;

namespace FinalInferno {
    [RequireComponent(typeof(Camera)), RequireComponent(typeof(UnityEngine.U2D.PixelPerfectCamera))]
    public class CameraController : MonoBehaviour {
        [SerializeField] private Transform target;
        [SerializeField] private float freedomDistance = 2;
        [SerializeField] private float smoothing = 0.6f;
        private Tilemap[] tilemaps = null;
        private bool HasTileMaps => tilemaps != null && tilemaps.Length > 0;
        private Vector2 tilemapsMin;
        private Vector2 tilemapsMax;
        private Vector2 lowerBounds;
        private Vector2 upperBounds;
        private new Camera camera;
        private float cameraHalfHeight;

        private void Awake() {
            camera = GetComponent<Camera>();
            UpdateBounds();
        }

        private void UpdateBounds() {
            tilemaps = FindObjectsOfType<Tilemap>();
            if (HasTileMaps) {
                UpdateBoundsWithTilemaps();
            } else {
                RemoveCameraBounds();
            }
        }

        private void UpdateBoundsWithTilemaps() {
            ResetBoundsLimitValues();
            foreach (Tilemap tilemap in tilemaps) {
                UpdateBoundsLimits(tilemap);
            }
            SaveCameraBounds();
        }

        private void ResetBoundsLimitValues() {
            tilemapsMin = new Vector2(float.MaxValue, float.MaxValue);
            tilemapsMax = new Vector2(float.MinValue, float.MinValue);
        }

        private void UpdateBoundsLimits(Tilemap tilemap) {
            Vector2 tilemapMinPoint = GetTilemapMinPoint(tilemap);
            Vector2 tilemapMaxPoint = GetTilemapMaxPoint(tilemap);
            tilemapsMin = Vector2.Min(tilemapsMin, tilemapMinPoint);
            tilemapsMax = Vector2.Max(tilemapsMax, tilemapMaxPoint);
        }

        private static Vector2 GetTilemapMinPoint(Tilemap tilemap) {
            return tilemap.transform.TransformPoint(tilemap.localBounds.min);
        }

        private static Vector2 GetTilemapMaxPoint(Tilemap tilemap) {
            return tilemap.transform.TransformPoint(tilemap.localBounds.max);
        }

        private void SaveCameraBounds() {
            cameraHalfHeight = camera.orthographicSize;
            float cameraHalfWidth = cameraHalfHeight * camera.aspect;
            Vector2 cameraOffset = new Vector2(cameraHalfWidth, cameraHalfHeight);
            lowerBounds = tilemapsMin + cameraOffset;
            upperBounds = tilemapsMax - cameraOffset;
        }

        private void RemoveCameraBounds() {
            lowerBounds = new Vector2(float.MinValue, float.MinValue);
            upperBounds = new Vector2(float.MaxValue, float.MaxValue);
        }

        private void Start() {
            UpdateBoundsIfChanged();
            target = CharacterOW.MainOWCharacter?.transform;
        }

        private void UpdateBoundsIfChanged() {
            if (cameraHalfHeight != camera.orthographicSize) {
                UpdateBounds();
            }
        }

        private void LateUpdate() {
            UpdateBoundsIfChanged();
            if (target != null) {
                FollowTargetWithinBounds();
            } else {
                StayInPositionWithinBounds();
            }
        }

        private void FollowTargetWithinBounds() {
            Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
            Vector2 targetPosition = new Vector2(target.position.x, target.position.y);
            Vector2 newPosition = CalculateNewPosition(currentPosition, targetPosition);
            MoveToPositionWithinBounds(newPosition);
        }

        private Vector2 CalculateNewPosition(Vector2 currentPosition, Vector2 targetPosition) {
            float distance = Vector2.Distance(currentPosition, targetPosition);
            if (distance > freedomDistance) {
                return LerpTowardsTargetWithSmoothing(currentPosition, targetPosition);
            }
            return currentPosition;
        }

        private Vector2 LerpTowardsTargetWithSmoothing(Vector2 currentPosition, Vector2 targetPosition) {
            float distance = Vector2.Distance(currentPosition, targetPosition);
            targetPosition -= currentPosition;
            targetPosition.Normalize();
            targetPosition *= (distance - freedomDistance);
            targetPosition += currentPosition;
            return Vector2.Lerp(currentPosition, targetPosition, smoothing);
        }

        private void MoveToPositionWithinBounds(Vector2 newPosition) {
            newPosition.x = Mathf.Clamp(newPosition.x, lowerBounds.x, upperBounds.x);
            newPosition.y = Mathf.Clamp(newPosition.y, lowerBounds.y, upperBounds.y);
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }

        private void StayInPositionWithinBounds() {
            Vector2 samePosition = new Vector2(transform.position.x, transform.position.y);
            MoveToPositionWithinBounds(samePosition);
        }
    }
}
