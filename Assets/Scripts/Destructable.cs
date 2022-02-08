using UnityEngine;

public class Destructable : MonoBehaviour {
    public void DestroyObject() {
        Destroy(gameObject);
    }
}
