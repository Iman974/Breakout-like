using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

    [SerializeField] private float minXStartDirection = -1f, maxXStartDirection = 1f, minYStartDirection = 0.2f, maxYStartDirection = 1f;
    [SerializeField] private bool isMainBall = false;
    [SerializeField] private float speedUpDelay = 4f;
    [SerializeField] private float speedUpMultiplier = 1.075f;
    [SerializeField] private float maxSpeed = 10f;

    //private float oldXVelocity;
    private GameManager GMinstance;

    public float speed = 6f;
    public static Ball MainBall { get; private set; }
    public Rigidbody2D Rb2D { get; private set; }

    private void Awake() {
        if (isMainBall) {
            MainBall = this;
        }
        Rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        GMinstance = GameManager.Instance;
    }

    public void Launch() { // should the ball be launched in a random direction or may the player be able to choose ?
        Rb2D.velocity = new Vector2(Random.Range(minXStartDirection, maxXStartDirection),
            Random.Range(minYStartDirection, maxYStartDirection)).normalized * speed;
        StartCoroutine(SpeedUpOverTime());
    }

    private void FixedUpdate() {
        /*if (Mathf.Approximately(rb2D.velocity.x, oldXVelocity)) {

        }*/
    }

    public void AlignXToGamepad() {
        transform.position = new Vector2(GMinstance.MousePositionX, transform.position.y);
    }

    public void SetDirection(Vector2 direction) {
        Rb2D.velocity = direction.normalized * speed;
    }

    private IEnumerator SpeedUpOverTime() {
        while (speed < maxSpeed) {
            yield return new WaitForSeconds(speedUpDelay);
            Rb2D.velocity *= speedUpMultiplier;
            speed *= speedUpMultiplier;
        }
        speed = maxSpeed;
        Rb2D.velocity = Rb2D.velocity.normalized * speed;
    }

    private void OnDestroy() {
        MainBall = null;
    }
}
