using UnityEngine;

public class PowerUpInGame : MonoBehaviour {
    
    public PowerUp powerUp;

    //private float timeSinceInstantiation;
    private Vector2 startPosition;
    private AnimationCurve powerUpXMovement;
    private AnimationCurve powerUpYMovement;
    private float relativeTargetedPositionY = -10f;
    private float horizontalMovementMaxBound;
    private float timeToEvalY;
    private float yMovementSpeed;
    private float timeToEvalX;
    private float xMovementSpeed;
    private Vector2 nextPosition;

    private void Start() {
        //GetComponent<CircleCollider2D>().radius = powerUp.colliderRadius;
        //GetComponent<SpriteRenderer>().sprite = powerUp.powerUpSprite;
        startPosition = transform.position;
        //powerUp.InitPower();

        powerUpXMovement = powerUp.horizontalMovement;
        powerUpYMovement = powerUp.verticalMovement;
        relativeTargetedPositionY = startPosition.y + powerUp.targetedRelativePosition.y;
        yMovementSpeed = powerUp.yMovementSpeed;
        xMovementSpeed = powerUp.xMovementSpeed;
        horizontalMovementMaxBound = powerUp.xMovementMaxBound;
    }

    private void FixedUpdate() {
        timeToEvalY += yMovementSpeed;
        timeToEvalX += xMovementSpeed;

        nextPosition.x = startPosition.x + (horizontalMovementMaxBound * powerUpXMovement.Evaluate(timeToEvalX));
        nextPosition.y = startPosition.y + (relativeTargetedPositionY * powerUpYMovement.Evaluate(timeToEvalY));

        transform.position = nextPosition;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Ball")) {
            return;
        }

        Destroy(gameObject, 1f);
        //powerUp.TriggerPower();
        //Destroy(gameObject); // WHY ??!
        // Play pick up anim
    }
}
