using UnityEngine;

public class DontDestroyThis : MonoBehaviour {
    public void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
