using UnityEngine;

public class TimedDestroy : MonoBehaviour {

    [Tooltip("The ball will be destroyed after it has hit this amount of colliders.")]
    [SerializeField] private int hitCount;
    public int HitCount {
        get {
            return hitCount;
        }
        set {
            hitCount = value;
        }
    }

    private int collisionCount = 0;
    private Camera mainCamera;

    private void Start() {
        mainCamera = Camera.main;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        collisionCount++;
        if (collisionCount >= hitCount) {
            Destroy(gameObject);
        }
    }

    private void LateUpdate() {
        Vector2 currentPosition = mainCamera.WorldToViewportPoint(transform.position);

        if (currentPosition.x > 1f || currentPosition.x < 0f || currentPosition.y > 1f || currentPosition.y < 0f) {
            Destroy(gameObject, 0.25f);
        }
    }
}
