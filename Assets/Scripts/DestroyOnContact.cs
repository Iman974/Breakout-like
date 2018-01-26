using UnityEngine;

public class DestroyOnContact : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
    }
}
