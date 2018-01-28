using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private enum State {
        PLAYING,
        GAMEOVER,
        WIN
    }

    [SerializeField] private Text winText, loseText;
    [SerializeField] private TextAnimation countdownTextAnimation;
    [SerializeField] private TextAnimation scoreTextAnimation;
    [SerializeField] private Text livesText;
    [SerializeField] private float comboTime = 0.75f;
    [SerializeField] private int startedEarlyPenalty = -15;
    [SerializeField] private Color scoreUpColor = Color.green;
    [SerializeField] private Color scoreDownColor = Color.red;
    [SerializeField] private int lives = 1;
    [SerializeField] private PowerUp[] powerUps;
    [SerializeField] private float minPowerUpDelay = 3f, maxPowerUpDelay = 8f;
    [SerializeField] private GameObject powerUpObject;

    private GamepadController gamepad;
    private Camera mainCamera;
    private Ball mainBall;
    private int numberOfBricks;
    private int score;
    private int currentCombo = 1;
    private float initialComboTime;
    private State state;
    private float powerUpTimer;

    public static GameManager Instance { get; private set; }
    public int Lives {
        get {
            return lives;
        }
        set {
            if (state != State.WIN) {
                lives = value;
                livesText.text = "x" + (lives - 1);
                if (lives <= 0) {
                    GameOver();
                }
            }
        }
    }

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
        AddToScore(0);

        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame() {
        while (AnimatedReveal.IsAnimationRunning) {
            yield return null;
        }
        Cursor.lockState = CursorLockMode.None;

        StartCoroutine(countdownTextAnimation.StartAnimation(Animation.SIZE, 1f, "3", "2", "1", "Go !" ));

        mainBall.Rb2D.isKinematic = true;
        while (!Input.GetButtonUp("Fire1")) {
            float mousePositionX = mainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, 0f)).x;
            gamepad.transform.position = new Vector2(mousePositionX, transform.position.y);
            mainBall.transform.position = new Vector2(mousePositionX, mainBall.transform.position.y);
            yield return null;
        }
        if (countdownTextAnimation.CurrentAnimatingString != null && countdownTextAnimation.CurrentAnimatingString != "Go !") {
            AddToScore(startedEarlyPenalty);
        }
        mainBall.Rb2D.isKinematic = false;
        mainBall.Launch();
    }

    public void RemoveBrick(int scoreValue) {
        numberOfBricks--;
        AddToScore(scoreValue * currentCombo);
        if (comboTime > 0f) {
            currentCombo++;
            comboTime = initialComboTime;
        }

        if (numberOfBricks == 0) {
            Win();
        }
    }

    private void AddToScore(int valueToAdd) {
        score += valueToAdd;
        if (valueToAdd > 0) {
            scoreTextAnimation.ColorToSet = scoreUpColor;
        } else if (valueToAdd < 0) {
            scoreTextAnimation.ColorToSet = scoreDownColor;
        }

        if (!scoreTextAnimation.IsAnimationRunning) {
            StartCoroutine(scoreTextAnimation.StartAnimation(Animation.COLOR, textToDisplay: "Score: " + score));
        } else {
            scoreTextAnimation.SetText("Score: " + score);
        }
    }

    private void Update() {
        if (comboTime > 0f) {
            comboTime -= Time.deltaTime;
        } else {
            currentCombo = 1;
            comboTime = initialComboTime;
        }

        powerUpTimer += Time.deltaTime;
        if (powerUpTimer >= minPowerUpDelay && true) {
            powerUpTimer = -50f;
            PowerUpInGame instantiatedPowerUp = Instantiate(powerUpObject, Vector2.zero, Quaternion.identity).GetComponent<PowerUpInGame>();
            instantiatedPowerUp.powerUp = powerUps[Random.Range(0, powerUps.Length)];
        }
    }

    private void GameOver() {
        loseText.gameObject.SetActive(true);
        state = State.GAMEOVER;
    }

    private void Win() {
        winText.gameObject.SetActive(true);
        state = State.WIN;
    }
}
