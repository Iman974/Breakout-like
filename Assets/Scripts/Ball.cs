using UnityEngine;

public class Ball : MonoBehaviour {

    [SerializeField] private float minXStartDirection = -1f, maxXStartDirection = 1f, minYStartDirection = 0.2f, maxYStartDirection = 1f;
    [SerializeField] private bool showStartAngle = false;
    [SerializeField] private bool mainBall = false;

    private Rigidbody2D rb2D;
    //private float oldXVelocity;

    public float speed = 6f;
    public static Ball MainBall { get; private set; }

    private void Awake() {
        if (mainBall) {
            MainBall = this;
        }
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void Launch() { // should the ball be launched in a random direction or may the player be able to choose ?
        rb2D.velocity = new Vector2(Random.Range(minXStartDirection, maxXStartDirection),
            Random.Range(minYStartDirection, maxYStartDirection)).normalized * speed;
    }

    private void FixedUpdate() {
        /*if (Mathf.Approximately(rb2D.velocity.x, oldXVelocity)) {

        }*/
    }

    public void SetDirection(Vector2 direction) {
        rb2D.velocity = direction.normalized * speed;
    }

    private void OnDrawGizmosSelected() {
        if (showStartAngle) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + Random.Range(minXStartDirection, maxXStartDirection),
                transform.position.y + Random.Range(minYStartDirection, maxYStartDirection)));
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 1f);
        }
    }
}
