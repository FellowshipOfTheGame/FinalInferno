using UnityEngine;

namespace FinalInferno {
    public static class Utils {
        public static T GetOrAddComponent<T>(GameObject gameObject) where T : Component {
            T component = gameObject.GetComponent<T>();
            component ??= gameObject.AddComponent<T>();
            return component;
        }
    }
}
