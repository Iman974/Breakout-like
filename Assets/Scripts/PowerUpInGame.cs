using UnityEngine;

public class PowerUpInGame : MonoBehaviour {

    public PowerUp powerUp;

    private float timeSinceInstantiation;
    private Vector2 startPosition;

    private void Start() {
        GetComponent<CircleCollider2D>().radius = powerUp.colliderRadius;
        GetComponent<SpriteRenderer>().sprite = powerUp.powerUpSprite;
        startPosition = transform.position;
    }

    private void Update() {
        timeSinceInstantiation += Time.deltaTime;
    }

    private void FixedUpdate() {
        transform.position = startPosition + new Vector2(powerUp.horizontalMovement.Evaluate(timeSinceInstantiation),
            powerUp.verticalMovement.Evaluate(timeSinceInstantiation));
    }
}
