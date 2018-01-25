using UnityEngine;

public class DestroyOnContact : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other) {
        Destroy(gameObject, 0.02f);
    }
}
