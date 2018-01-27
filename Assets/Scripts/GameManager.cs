using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField] private Text winText;

    private GamepadController gamepad;
    private Camera mainCamera;
    private Ball mainBall;
    private int numberOfBricks;

    public static GameManager Instance { get; private set; }

    private void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }
        #endregion
    }

    private void Start() {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        numberOfBricks = GameObject.FindWithTag("Bricks").transform.childCount;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gamepad = GameObject.FindWithTag("GameController").GetComponent<GamepadController>();
        mainBall = Ball.MainBall;

        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame() {
        while (!AnimatedAppear.AnimationsOver) {
            yield return null;
        }
        Cursor.lockState = CursorLockMode.None;
        
        while (!Input.GetButtonUp("Fire1")) {
            float mousePositionX = mainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, 0f)).x;
            gamepad.transform.position = new Vector2(mousePositionX, transform.position.y);
            mainBall.transform.position = new Vector2(mousePositionX, mainBall.transform.position.y);
            yield return null;
        }
        mainBall.Launch();
    }

    public void RemoveBrick() {
        numberOfBricks--;

        if (numberOfBricks == 0) {
            winText.gameObject.SetActive(true);
        }
    }
}
