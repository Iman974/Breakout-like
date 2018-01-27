using UnityEngine;
using System.Collections;

public class GamepadController : MonoBehaviour {

    [SerializeField] private float minHorizontal, maxHorizontal;
    [SerializeField] private GameObject smokeEffect;
    [SerializeField] private Vector2 smokeEffectOffset;
    [SerializeField] private float firstPosLerpTime = 0.5f;

    private Camera mainCamera;
    private SpriteRenderer gamepadRenderer;
    private float upperYBound;
    //private bool startSmoothing = false;

    private void Awake() {
        gamepadRenderer = GetComponent<SpriteRenderer>();
        upperYBound = gamepadRenderer.bounds.max.y;
    }

    private void Start () {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        //StartCoroutine(SmoothStart());
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
        transform.position = new Vector2(mainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, 0f)).x,
            transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Vector2 contactPoint = new Vector2(other.transform.position.x, upperYBound);

        //Debug.DrawRay(other.transform.position, ballRb2D.velocity.normalized, Color.cyan, 2f);
        other.GetComponent<Ball>().SetDirection(new Vector2((contactPoint - (Vector2)gamepadRenderer.bounds.min).x - 1.5f, 1f));
        //Debug.DrawRay(other.transform.position, ballRb2D.velocity.normalized, Color.red, 2f);

        //Destroy(Instantiate(smokeEffect, ballContacts[0].point + smokeEffectOffset, Quaternion.identity), 0.5f);
    }
}
