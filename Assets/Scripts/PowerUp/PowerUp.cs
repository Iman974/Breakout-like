using UnityEngine;

/// <summary>
/// When collected, a Power Up has visuals switched off, but the Power Up gameobject exists until it is time for it to expire
/// Subclasses of this must:
/// 1. Implement PowerUpPayload()
/// 2. Optionally Implement PowerUpHasExpired() to remove what was given in the payload
/// 3. Call PowerUpHasExpired() when the power up has expired or tick ExpiresImmediately in inspector
/// </summary>
[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class PowerUp : MonoBehaviour {

    [SerializeField] protected string powerUpName;
    //public string powerUpExplanation;
    //public string powerUpQuote;
    [SerializeField] protected GameObject pickUpEffect;
    [SerializeField] protected AudioClip soundEffect;
    [SerializeField] protected float powerUpDuration;
    [SerializeField] protected AnimationCurve verticalMovement = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] protected AnimationCurve horizontalMovement = AnimationCurve.Constant(0f, 1f, 0f);
    [SerializeField] protected Vector2 targetedRelativePosition = new Vector2(0f, -10f);
    [SerializeField] protected float yMovementSpeed = 0.0007f;
    [SerializeField] protected float xMovementSpeed = 0.0007f;
    [SerializeField] protected float xMovementBound = 2f;

    protected SpriteRenderer spriteRenderer;
    protected Collider2D thisCollider;
    protected bool expiresImmediately;

    private Vector2 startPosition;
    private float relativeTargetedPositionY = -10f;
    private float timeToEvalY;
    private float timeToEvalX;
    private Vector2 nextPosition;
    private bool collected;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer> ();
        thisCollider = GetComponent<Collider2D>();
        if (powerUpDuration == 0) {
            expiresImmediately = true;
        }
    }

    private void Start() {
        startPosition = transform.position;
        relativeTargetedPositionY = startPosition.y + targetedRelativePosition.y;
    }

    private void FixedUpdate() {
        if (!collected) {
            timeToEvalY += yMovementSpeed;
            timeToEvalX += xMovementSpeed;

            nextPosition.x = startPosition.x + (xMovementBound * horizontalMovement.Evaluate(timeToEvalX));
            nextPosition.y = startPosition.y + (relativeTargetedPositionY * verticalMovement.Evaluate(timeToEvalY));

            transform.position = nextPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Ball")) {
            return;
        }
        PowerUpCollected();
    }

    private void PowerUpCollected() {
        collected = true;
        spriteRenderer.enabled = false;
        thisCollider.enabled = false;

        PowerUpEffects();           
        PowerUpPayload();

        // Send message to any listeners
        //foreach (GameObject go in EventSystemListeners.main.listeners) {
        //    ExecuteEvents.Execute<IPowerUpEvents> (go, null, (x, y) => x.OnPowerUpCollected (this, playerBrain));
        //}
    }

    private void PowerUpEffects() {
        if (pickUpEffect != null) {
            Destroy(Instantiate(pickUpEffect), 2f);
        }

        //if (soundEffect != null) {
        //    MainGameController.main.PlaySound (soundEffect);
        //}
    }

    protected virtual void PowerUpPayload() {
        // If we're instant use we also expire self immediately
        if (expiresImmediately) {
            PowerUpHasExpired();
        }
    }

    protected virtual void PowerUpHasExpired() {
        // Send message to any listeners
        //foreach (GameObject go in EventSystemListeners.main.listeners) {
        //    ExecuteEvents.Execute<IPowerUpEvents> (go, null, (x, y) => x.OnPowerUpExpired (this, playerBrain));
        //}
        Destroy(gameObject);
    }
}
