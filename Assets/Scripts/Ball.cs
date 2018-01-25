using UnityEngine;

public class Ball : MonoBehaviour {

    [SerializeField] private float speed;
    [SerializeField] private float minStartDirectionAngle = -50f, maxStartDirectionAngle = 50f;

    private Rigidbody2D rb2D;

    private void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start () {
        Debug.DrawRay(transform.position, Random.insideUnitCircle * 5f, Color.green, 5f);
        rb2D.AddForce(new Vector2(Random.Range(minStartDirectionAngle, maxStartDirectionAngle), speed), ForceMode2D.Impulse);
        rb2D.velocity = Vector2.ClampMagnitude(rb2D.velocity, speed);
    }

    private void FixedUpdate () {
	}
}
