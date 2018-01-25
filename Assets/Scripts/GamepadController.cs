using UnityEngine;

public class GamepadController : MonoBehaviour {

    [SerializeField] private float minHorizontal, maxHorizontal;

    private Camera mainCamera;
    private SpriteRenderer gamepadRenderer;

    private void Awake() {
        gamepadRenderer = GetComponent<SpriteRenderer>();
    }

    void Start () {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    private void FixedUpdate() { // Makes the gamepad follow the mouse
        Vector2 nextPosition = new Vector2(mainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, 0f)).x, transform.position.y);
        /*float nextLeftBoundX = gamepadRenderer.bounds.min.x + gamepadRenderer.bounds.center.x + nextPosition.x - transform.position.x;
        float nextRightBoundX = gamepadRenderer.bounds.max.x + nextPosition.x;
        Debug.Log(nextLeftBoundX);*/
        if (true/*nextLeftBoundX >= minHorizontal || nextRightBoundX <= maxHorizontal*/) {
            transform.position = nextPosition;
        }
    }
}
