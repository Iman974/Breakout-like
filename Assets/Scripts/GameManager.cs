using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField] private Text winText;
    [SerializeField] private TextAnimation countdownTextAnimation;
    [SerializeField] private Text scoreText;
    [SerializeField] private float comboTime = 0.75f;

    private GamepadController gamepad;
    private Camera mainCamera;
    private Ball mainBall;
    private int numberOfBricks;
    private int score;
    private int currentCombo = 1;
    private float initialComboTime;

    public static GameManager Instance { get; private set; }

    private void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }
        #endregion
        initialComboTime = comboTime;
    }

    private void Start() {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        numberOfBricks = GameObject.FindWithTag("Bricks").transform.childCount;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gamepad = GameObject.FindWithTag("GameController").GetComponent<GamepadController>();
        mainBall = Ball.MainBall;
        UpdateScore();

        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame() {
        while (!AnimatedAppear.AnimationsOver) {
            yield return null;
        }
        Cursor.lockState = CursorLockMode.None;

        countdownTextAnimation.StartAnimation(1f, "3", "2", "1", "Go !" );

        while (!Input.GetButtonUp("Fire1")) {
            float mousePositionX = mainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, 0f)).x;
            gamepad.transform.position = new Vector2(mousePositionX, transform.position.y);
            mainBall.transform.position = new Vector2(mousePositionX, mainBall.transform.position.y);
            yield return null;
        }
        mainBall.Launch();
    }

    public void RemoveBrick(int scoreValue) {
        numberOfBricks--;
        score += scoreValue * currentCombo;
        if (comboTime > 0f) {
            currentCombo++;
            comboTime = initialComboTime;
        }
        UpdateScore();

        if (numberOfBricks == 0) {
            winText.gameObject.SetActive(true);
        }
    }

    private void UpdateScore() {
        scoreText.text = "Score: " + score;
    }

    private void Update() {
        if (comboTime > 0f) {
            comboTime -= Time.deltaTime;
        } else {
            currentCombo = 1;
            comboTime = initialComboTime;
        }
    }
}
