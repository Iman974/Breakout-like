using UnityEngine;
using System.Collections;

public class GamepadController : MonoBehaviour {

    [SerializeField] private float minHorizontal = -5.25f, maxHorizontal = 5.25f;
    [SerializeField] private GameObject smokeEffect;
    [SerializeField] private Vector2 smokeEffectOffset;
    [SerializeField] private float firstPosLerpTime = 0.5f; // Mobile input
    [SerializeField] private float checkIfMovingRate = 0.04f;
    [SerializeField] private float minImpactRequiredAcceleration = 10f;

    private Camera mainCamera;
    private SpriteRenderer gamepadRenderer;
    private float upperYBound;
    private GameManager GMinstance;
    private float xAcceleration;
    private float xBeginDrag;
    private Rigidbody2D rb2D;
    private float accelerationTime;
    private bool impactTriggered;
    private float dragDirection = 1f;
    private Vector2 nextPosition;
    private bool clampHorizontal;
    //private bool startSmoothing = false;

    private void Awake() {
        gamepadRenderer = GetComponent<SpriteRenderer>();
        upperYBound = gamepadRenderer.bounds.max.y;
        rb2D = GetComponent<Rigidbody2D>();
        nextPosition.y = transform.position.y;
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

    private void Update() {
        if (Input.GetButton("Fire1") && transform.position.x >= minHorizontal && transform.position.x <= maxHorizontal) {
            clampHorizontal = true;

            int i = 0;
            if (Input.GetButtonDown("Fire1")) {
                ResetAcceleration();
                InvokeRepeating("Timer", 0f, 0.02f);
                InvokeRepeating("CheckIfMoving", 0f, checkIfMovingRate);
            }
            if (Mathf.Approximately(nextPosition.x, minHorizontal) || Mathf.Approximately(nextPosition.x, maxHorizontal)) {
                xAcceleration = Mathf.Abs(transform.position.x - xBeginDrag) / accelerationTime;
                if (!impactTriggered && xAcceleration >= minImpactRequiredAcceleration) {
                    TriggerImpact();
                }
            } else if (impactTriggered) {
                impactTriggered = false;
            }
        } else if (Input.GetButtonUp("Fire1")) {
            CancelInvoke();
            impactTriggered = false; // Not necessary ?
            clampHorizontal = false;
        }
    }

    private void CheckIfMoving() {
        if (transform.hasChanged == false) {
            ResetAcceleration();
            return;
        }
        transform.hasChanged = false;

        if (dragDirection != (transform.position.x > xBeginDrag ? 1f : -1f)) {
            dragDirection = -dragDirection;
            ResetAcceleration(); // This whole test doesn't work if drag started outside clampHorizontal
        }
    }

    public void UpdateGamepadPosition() {
        nextPosition.x = GMinstance.MousePositionX;

        if (clampHorizontal) {
            nextPosition.x = Mathf.Clamp(nextPosition.x, minHorizontal, maxHorizontal); 
        }
        transform.position = nextPosition;
    }

    private void ResetAcceleration() {
        accelerationTime = 0f;
        xBeginDrag = transform.position.x;
    }

    private void TriggerImpact() {
        impactTriggered = true;
        ResetAcceleration();
        // Trigger the impact effect relatively to the acceleration -> xAcceleration / minImpactRequiredAcceleration

        Debug.Log(xAcceleration);
    }

    private void Timer() {
        accelerationTime += 0.02f;
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
