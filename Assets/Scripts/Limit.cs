using UnityEngine;

public class Limit : MonoBehaviour {

    [SerializeField] private float closeDistance = 1f;
    [SerializeField] private Vector2 impactForce;

    private Collider2D thisCollider;
    private Vector2 ballClosestPoint;
    private Collider2D ballCollider;
    private GamepadController mainGamepad;
    private float inverseMaxAcceleration;

    public static Limit[] Instances = new Limit[4];

    private void Awake() {
        thisCollider = GetComponent<Collider2D>();

        Instances[(int)System.Char.GetNumericValue(name[name.Length - 1]) - 1] = this;
    }

    private void Start() {
        ballCollider = Ball.MainBall.GetComponent<Collider2D>();
        mainGamepad = GameObject.FindWithTag("GameController").GetComponent<GamepadController>();
        inverseMaxAcceleration = 1f / mainGamepad.maxAcceleration;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Ball")) {
            Destroy(other.gameObject, 2f);
            return;
        }

        GameManager.Instance.Lives--;
        Destroy(other.gameObject, 2f);
        if (GameManager.Instance.GameState == GameManager.State.PLAYING) {
            GameManager.Instance.Invoke("RestartGame", 2f);
        }
    }

    public void Impact() {
        if (thisCollider.isTrigger) {
            return;
        }
        if (ballCollider == null) {
            ballCollider = Ball.MainBall.GetComponent<Collider2D>();
        }

        Vector2 limitClosestPoint = thisCollider.bounds.ClosestPoint(Ball.MainBall.transform.position);
        ballClosestPoint = ballCollider.bounds.ClosestPoint(limitClosestPoint);

        if ((ballClosestPoint - limitClosestPoint).sqrMagnitude <= closeDistance * closeDistance) {
            Ball.MainBall.Direction += impactForce * (mainGamepad.XAcceleration * inverseMaxAcceleration);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + (impactForce.normalized * closeDistance));
    }
}
