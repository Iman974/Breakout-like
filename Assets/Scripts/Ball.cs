using UnityEngine;

public class Ball : MonoBehaviour {

    [SerializeField] private float speed;
    [SerializeField] private float minStartDirectionAngle = -50f, maxStartDirectionAngle = 50f;
    [SerializeField] private float xFactor;
    [SerializeField] private float YFactor;
    [SerializeField] private float minXDirection, maxXDirection, minYDirection, maxYDirection;

    private Rigidbody2D rb2D;

    private float debug;
    private Vector3 initial;

    private void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
        initial = transform.position;
    }

    private void Start () {
        /*rb2D.AddForce(new Vector2(Random.Range(minXDirection, maxXDirection),
            Random.Range(minYDirection, maxYDirection)), ForceMode2D.Impulse);*/
        rb2D.velocity = new Vector2(Random.Range(minXDirection, maxXDirection), Random.Range(minYDirection, maxYDirection));
    }

    private void FixedUpdate () {
        debug += 0.02f;
        if (debug >= 1f) {
            debug = 0f;
            rb2D.velocity = Vector2.zero;
            transform.position = initial;
            rb2D.velocity = new Vector2(Random.Range(minXDirection, maxXDirection), Random.Range(minYDirection, maxYDirection));
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + Random.Range(minXDirection, maxXDirection),
            transform.position.y + Random.Range(minYDirection, maxYDirection)));
    }
}
