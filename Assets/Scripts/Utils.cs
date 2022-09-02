using UnityEngine;


namespace FinalInferno {
    public static class Utils {
        public static T GetOrAddComponent<T>(GameObject gameObject) where T : Component {
            T component = gameObject.GetComponent<T>();
            return component ? component : gameObject.AddComponent<T>();
        }

        public static T GetComponentIfNull<T>(this Component monoBehaviour, T currentValue) where T : Component {
            return currentValue ? currentValue : monoBehaviour.GetComponent<T>();
        }

        public static T GetComponentIfNull<T>(this GameObject monoBehaviour, T currentValue) where T : Component {
            return currentValue ? currentValue : monoBehaviour.GetComponent<T>();
        }

        public static void DestroyIfExists<T>(T objectToDestroy) where T : Object {
            if (objectToDestroy)
                Object.Destroy(objectToDestroy);
        }
    }
}
