using UnityEngine;

public class Ball : MonoBehaviour {

    [SerializeField] private float minStartDirectionAngle = -50f, maxStartDirectionAngle = 50f;
    [SerializeField] private float xFactor;
    [SerializeField] private float YFactor;
    [SerializeField] private float minXDirection, maxXDirection, minYDirection, maxYDirection;

    private Rigidbody2D rb2D;
    private Vector3 initial;

    public float speed = 6f;

    private void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
        initial = transform.position;
    }

    private void Start () {
        rb2D.velocity = new Vector2(Random.Range(minXDirection, maxXDirection),
            Random.Range(minYDirection, maxYDirection)).normalized * speed;
    }

    private void FixedUpdate () {
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + Random.Range(minXDirection, maxXDirection),
            transform.position.y + Random.Range(minYDirection, maxYDirection)));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
