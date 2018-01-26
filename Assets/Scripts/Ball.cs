using UnityEngine;

public class Ball : MonoBehaviour {

    [SerializeField] private float minXStartDirection = -1f, maxXStartDirection = 1f, minYStartDirection = 0.2f, maxYStartDirection = 1f;
    [SerializeField] private bool showStartAngle = true;

    private Rigidbody2D rb2D;

    public float speed = 6f;

    private void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void LaunchBall() {
        rb2D.velocity = new Vector2(Random.Range(minXStartDirection, maxXStartDirection),
            Random.Range(minYStartDirection, maxYStartDirection)).normalized * speed;
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
