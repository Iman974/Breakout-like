using UnityEngine;

/// <summary>
/// Game independent Power Up logic supporting 2D and 3D modes.
/// When collected, a Power Up has visuals switched off, but the Power Up gameobject exists until it is time for it to expire
/// Subclasses of this must:
/// 1. Implement PowerUpPayload()
/// 2. Optionally Implement PowerUpHasExpired() to remove what was given in the payload
/// 3. Call PowerUpHasExpired() when the power up has expired or tick ExpiresImmediately in inspector
/// </summary>
[RequireComponent(typeof(CircleCollider2D), typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class _PowerUp : MonoBehaviour {

    [SerializeField] protected string powerUpName;
    //public string powerUpExplanation;
    //public string powerUpQuote;
    //[Tooltip ("Tick true for power ups that are instant use, eg a health addition that has no delay before expiring")]
    [SerializeField] protected GameObject pickUpEffect;
    [SerializeField] protected AudioClip soundEffect;
    [SerializeField] protected float powerUpDuration;
    [SerializeField] protected AnimationCurve verticalMovement = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] protected AnimationCurve horizontalMovement = AnimationCurve.Constant(0f, 1f, 0f);
    [SerializeField] protected Vector2 targetedRelativePosition = new Vector2(0f, -10f);
    [SerializeField] protected float yMovementSpeed = 0.0007f;
    [SerializeField] protected float xMovementSpeed = 0.0007f;
    [SerializeField] protected float xMovementMaxBound = 2f;

    protected SpriteRenderer spriteRenderer;
    protected Collider2D thisCollider;
    protected bool expiresImmediately;
    //protected PowerUpState powerUpState;

    protected Vector2 startPosition; // Is all this needed to be protected or can put private ?
    protected float relativeTargetedPositionY = -10f;
    protected float timeToEvalY;
    protected float timeToEvalX;
    protected Vector2 nextPosition;
    protected bool collected;

    protected virtual void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer> ();
        thisCollider = GetComponent<Collider2D>();
        if (powerUpDuration == 0) {
            expiresImmediately = true;
        }
    }

    protected virtual void Start() {
        //powerUpState = PowerUpState.InAttractMode;
        //GetComponent<CircleCollider2D>().radius = powerUpData.colliderRadius;
        //GetComponent<SpriteRenderer>().sprite = powerUpData.powerUpSprite;
        startPosition = transform.position;
        //powerUp.InitPower();

        relativeTargetedPositionY = startPosition.y + targetedRelativePosition.y;
    }

    protected void FixedUpdate() {
        if (!collected) {
            timeToEvalY += yMovementSpeed;
            timeToEvalX += xMovementSpeed;

            nextPosition.x = startPosition.x + (xMovementMaxBound * horizontalMovement.Evaluate(timeToEvalX));
            nextPosition.y = startPosition.y + (relativeTargetedPositionY * verticalMovement.Evaluate(timeToEvalY));

            transform.position = nextPosition;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Ball")) {
            return;
        }
        PowerUpCollected();
    }

    protected virtual void PowerUpCollected() {
        // We only care if we've been collected by the player
        //if (gameObjectCollectingPowerUp.tag != "Player") {
        //    return;
        //}

        // We only care if we've not been collected before
        //if (powerUpState == PowerUpState.IsCollected || powerUpState == PowerUpState.IsExpiring) {
        //    return;
        //}
        //powerUpState = PowerUpState.IsCollected;

        // We must have been collected by a player, store handle to player for later use      
        //playerBrain = gameObjectCollectingPowerUp.GetComponent<PlayerBrain> ();

        // We move the power up game object to be under the player that collect it, this isn't essential for functionality 
        // presented so far, but it is neater in the gameObject hierarchy
        //gameObject.transform.parent = playerBrain.gameObject.transform;
        //gameObject.transform.position = playerBrain.gameObject.transform.position;
        collected = true;
        spriteRenderer.enabled = false;
        thisCollider.enabled = false;

        // Collection effects
        PowerUpEffects();           

        // Payload      
        PowerUpPayload();

        // Send message to any listeners
        //foreach (GameObject go in EventSystemListeners.main.listeners) {
        //    ExecuteEvents.Execute<IPowerUpEvents> (go, null, (x, y) => x.OnPowerUpCollected (this, playerBrain));
        //}

        // Now the power up visuals can go away
        spriteRenderer.enabled = false;
    }

    protected virtual void PowerUpEffects() {
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
        //if (powerUpState == PowerUpState.IsExpiring) {
        //    return;
        //}
        //powerUpState = PowerUpState.IsExpiring;

        // Send message to any listeners
        //foreach (GameObject go in EventSystemListeners.main.listeners) {
        //    ExecuteEvents.Execute<IPowerUpEvents> (go, null, (x, y) => x.OnPowerUpExpired (this, playerBrain));
        //}
        //Debug.Log("Power Up has expired, removing after a delay for: " + gameObject.name);
        Destroy(gameObject, 10f);
    }
}
