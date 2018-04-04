using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
#pragma warning disable 0414
    [SerializeField] private float minXStartDirection = -1f, maxXStartDirection = 1f, minYStartDirection = 0.2f, maxYStartDirection = 1f;
    [SerializeField] private bool isMainBall = false;
    [SerializeField] private float speedUpDelay = 4f;
    [SerializeField] private float speedUpMultiplier = 1.075f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float speed = 6f;

    //private float oldXVelocity;
    private SpriteRenderer thisRenderer;

    public static Ball Main { get; private set; }
    public bool DoSpeedUpOverTime { get; set; }
    public float Radius { get; private set; }
    public Rigidbody2D Rb2D { get; private set; }

    /// <summary>
    /// Shorthand for writing transform.position.
    /// </summary>
    public Vector2 Position {
        get {
            return transform.position;
        }
        set {
            transform.position = value;
        }
    }

    public Vector2 Direction {
        get {
            return Rb2D.velocity.normalized;
        }
        set {
            Rb2D.velocity = value.normalized * speed;
        }
    }

    public float Speed {
        get {
            return speed;
        }
        set {
            Rb2D.velocity = Rb2D.velocity.normalized * value;

            if (value != speed) {
                speed = value;
            }
        }
    }

    private void Awake() {
        if (isMainBall) {
            Main = this;
        }

        Rb2D = GetComponent<Rigidbody2D>();
        thisRenderer = GetComponent<SpriteRenderer>();
        DoSpeedUpOverTime = true;
        Radius = thisRenderer.bounds.center.x - thisRenderer.bounds.min.x;
    }

    private void Start() {
        LevelManager.SceneUnload += OnLevelBeginUnload;
    }

    public void Launch() { // should the ball be launched in a random direction or may the player be able to choose ?
        Rb2D.isKinematic = false;
        //Direction = new Vector2(Random.Range(minXStartDirection, maxXStartDirection),
        //    Random.Range(minYStartDirection, maxYStartDirection));
        Direction = Vector2.up;

        StartCoroutine(SpeedUpOverTime());
    }

    private void LateUpdate() {
        Rb2D.velocity = Rb2D.velocity.normalized * speed;
    }

    private IEnumerator SpeedUpOverTime() {
        while (speed < maxSpeed || !DoSpeedUpOverTime) {
            if (!DoSpeedUpOverTime) {
                yield return null;
            }

            yield return new WaitForSeconds(speedUpDelay);
            Speed *= speedUpMultiplier;
        }
        speed = maxSpeed;
    }

    private void OnLevelBeginUnload() {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        LevelManager.SceneUnload -= OnLevelBeginUnload;
    }
}
