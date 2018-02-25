using UnityEngine;

public class DestroyAfterHitCount : MonoBehaviour {

    // Delete this script after and put it in ball script
    public int maxHitCount = 4;

    private int collisionCount = 0;

    private void OnCollisionEnter2D(Collision2D other) {
        collisionCount++;
        if (collisionCount >= maxHitCount) {
            Destroy(gameObject);
        }
    }
}
