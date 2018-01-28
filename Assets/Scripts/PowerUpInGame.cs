using UnityEngine;

public class PowerUpInGame : MonoBehaviour {

    public PowerUp powerUp;

    private float timeSinceInstantiation;

    private void Start() {
        GetComponent<CircleCollider2D>().radius = powerUp.colliderRadius;
        GetComponent<SpriteRenderer>().sprite = powerUp.powerUpSprite;
    }

    private void Update() {
        timeSinceInstantiation += Time.deltaTime;
    }

    private void FixedUpdate() {
        transform.position += new Vector3(powerUp.horizontalMovement.Evaluate(timeSinceInstantiation),
            powerUp.fallSpeed.Evaluate(timeSinceInstantiation));
    }
}
