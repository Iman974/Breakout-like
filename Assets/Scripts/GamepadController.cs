using UnityEngine;
using System.Collections;

public class GamepadController : MonoBehaviour {

    [SerializeField] private float minHorizontal = -5.25f, maxHorizontal = 5.25f;
    [SerializeField] private GameObject smokeEffect;
    [SerializeField] private Vector2 smokeEffectOffset;
    [SerializeField] private float firstPosLerpTime = 0.5f; // Mobile input

    private Camera mainCamera;
    private SpriteRenderer gamepadRenderer;
    private float upperYBound;
    private GameManager GMinstance;
    private float xAcceleration;
    private Rigidbody2D rb2D;
    //private bool startSmoothing = false;

    private void Awake() {
        gamepadRenderer = GetComponent<SpriteRenderer>();
        upperYBound = gamepadRenderer.bounds.max.y;
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start () {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        GMinstance = GameManager.Instance;
    }

    private IEnumerator SmoothStart() { // For mobile input
        //startSmoothing = true;
        float firstMousePositionX = mainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, 0f)).x;

        for (float currentLerpTime = 0f; currentLerpTime < firstPosLerpTime; currentLerpTime += Time.deltaTime) {
            transform.position = Vector2.Lerp(transform.position, new Vector2(firstMousePositionX, transform.position.y),
                Mathf.Sin((currentLerpTime / firstPosLerpTime) * Mathf.PI * 0.5f));
            firstMousePositionX = mainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, 0f)).x;
            yield return null;
        }
        //startSmoothing = false;
    }

    private void FixedUpdate() { // Makes the gamepad follow the mouse
        UpdateGamepadPosition();
    }

    public void UpdateGamepadPosition() {
        Vector2 nextPosition = new Vector2(GMinstance.MousePositionX, transform.position.y);

        if (Input.GetButton("Fire1")) {
            nextPosition = new Vector2(Mathf.Clamp(nextPosition.x, minHorizontal, maxHorizontal), nextPosition.y);
            if (Mathf.Approximately(nextPosition.x, minHorizontal) || Mathf.Approximately(nextPosition.x, maxHorizontal)) {
            }
        }
        transform.position = nextPosition;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Vector2 contactPoint = new Vector2(other.transform.position.x, upperYBound);

        //Debug.DrawRay(other.transform.position, ballRb2D.velocity.normalized, Color.cyan, 2f);
        other.GetComponent<Ball>().Direction = new Vector2((contactPoint - (Vector2)gamepadRenderer.bounds.min).x - 1.5f, 1f);
        // It is better to use already existing things than to create and throw away var over & over
        //Debug.DrawRay(other.transform.position, ballRb2D.velocity.normalized, Color.red, 2f);

        //Destroy(Instantiate(smokeEffect, ballContacts[0].point + smokeEffectOffset, Quaternion.identity), 0.5f);
    }
}
