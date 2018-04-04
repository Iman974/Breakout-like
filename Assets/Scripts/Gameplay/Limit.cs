using UnityEngine;

public class Limit : MonoBehaviour {

    [SerializeField] private bool isHorizontal;
    [SerializeField] private float closeDistance = 0.25f;
    [SerializeField] private Vector2 impactForce;

    private Collider2D thisCollider;
    private Vector2 ballClosestPoint;
    private Collider2D ballCollider;
    private GamepadController mainGamepad;
    private float inverseMaxAcceleration;
    private float closeDistanceSquared;

    public static Limit[] Instances = new Limit[4];

    private void Awake() {
        thisCollider = GetComponent<Collider2D>();

        Instances[(int)System.Char.GetNumericValue(name[name.Length - 1]) - 1] = this;

        closeDistanceSquared = closeDistance * closeDistance;
    }

    private void Start() {
        ballCollider = Ball.Main.GetComponent<Collider2D>();
        mainGamepad = GameObject.FindWithTag("GameController").GetComponent<GamepadController>();

        inverseMaxAcceleration = 1f / mainGamepad.maxAcceleration;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!thisCollider.isTrigger) {
            return;
        }

        Destroy(other.gameObject, 1f);

        if (other.CompareTag("Ball")) {
            GameManager.Instance.Lives--;
        }
    }

    public void Impact() {
        if (thisCollider.isTrigger) {
            return;
        }

        if (ballCollider == null) {
            ballCollider = Ball.Main.GetComponent<Collider2D>();
        }

        Vector2 limitClosestPoint = thisCollider.bounds.ClosestPoint(Ball.Main.Position);
        ballClosestPoint = ballCollider.bounds.ClosestPoint(limitClosestPoint);

        if ((ballClosestPoint - limitClosestPoint).sqrMagnitude <= closeDistanceSquared) {
            Ball.Main.Direction += impactForce * (mainGamepad.XAcceleration * inverseMaxAcceleration);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + (impactForce.normalized * closeDistance));
    }
}
