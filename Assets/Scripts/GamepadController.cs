using UnityEngine;

public class GamepadController : MonoBehaviour {

    [SerializeField] private float minHorizontal, maxHorizontal;

    private Camera mainCamera;
    private SpriteRenderer gamepadRenderer;
    private Rigidbody2D ballRb2D;
    private ContactPoint2D[] ballContacts = new ContactPoint2D[10];
    private float ballSpeed;

    private void Awake() {
        gamepadRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start () {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        ballRb2D = GameObject.FindWithTag("Ball").GetComponent<Rigidbody2D>();
        ballSpeed = ballRb2D.GetComponent<Ball>().speed;
    }

    private void FixedUpdate() { // Makes the gamepad follow the mouse
        transform.position = new Vector2(mainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, 0f)).x, transform.position.y);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        other.GetContacts(ballContacts);

        ballRb2D.velocity = new Vector2(CalculateForceDirection(), 1f).normalized * ballSpeed;
    }

    private float CalculateForceDirection() {
        float forceX = ((new Vector2(gamepadRenderer.bounds.center.x, ballContacts[0].point.y) - ballContacts[0].point)).sqrMagnitude;

        return ballContacts[0].point.x < transform.position.x ? -forceX : forceX;
    }
}
