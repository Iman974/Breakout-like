using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public enum State {
        PLAYING,
        GAMEOVER,
        WIN
    }

    [SerializeField] private Text winText, loseText;
    [SerializeField] private TextAnimation centerTextAnimation;
    [SerializeField] private TextAnimation scoreTextAnimation;
    [SerializeField] private GameObject mainBallObject;
    [SerializeField] private Text livesText;
    [SerializeField] private float comboTime = 0.75f;
    [SerializeField] private int startedEarlyPenalty = -15;
    [SerializeField] private Color scoreUpColor = Color.green;
    [SerializeField] private Color scoreDownColor = Color.red;
    [SerializeField] private int lives = 1;
    [SerializeField] private float startBallDistanceYFromGamePad = 1f;

    private GamepadController gamepad;
    private Camera mainCamera;
    private int numberOfBricks;
    private int score;
    private int currentCombo = 1;
    private float initialComboTime;

    public static GameManager Instance { get; private set; }
    public float MousePositionX { get; private set; }

    private State gameState;
    public State GameState {
        get {
            return gameState;
        }
        set {
            gameState = value;
            if (value == State.WIN) {
                winText.gameObject.SetActive(true);
                PowerUpSpawner.Instance.CancelInvoke();
            } else if (value == State.GAMEOVER) {
                loseText.gameObject.SetActive(true);
            }
        }
    }

    public int Lives {
        get {
            return lives;
        }
        set {
            if (GameState != State.WIN) {
                lives = value;
                UpdateLivesOnUI();

                if (lives <= 0) {
                    GameState = State.GAMEOVER;
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
        gamepad = GameObject.FindWithTag("GameController").GetComponent<GamepadController>();
        Lives = lives;
        AddToScore(0);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame(bool countDown = true) {
        while (AnimatedReveal.IsAnimationRunning) {
            yield return null;
        }
        Cursor.lockState = CursorLockMode.None;

        if (countDown) {
            StartCoroutine(centerTextAnimation.StartAnimation(Animation.SIZE, 1f, "3", "2", "1", "Go !"));
        }

        Ball.MainBall.transform.parent = gamepad.transform;
        Ball.MainBall.transform.localPosition = new Vector3(0f, Ball.MainBall.transform.localPosition.y);

        while (!Input.GetButtonUp("Fire1")) {
            yield return null;
        }
        if (countDown && centerTextAnimation.CurrentAnimatingString != null && centerTextAnimation.CurrentAnimatingString != "Go !") {
            AddToScore(startedEarlyPenalty);
        }
        Ball.MainBall.transform.parent = null;
        Ball.MainBall.Launch();
    }

    public void RemoveBrick(int scoreValue) {
        numberOfBricks--;
        AddToScore(scoreValue * currentCombo);
        if (comboTime > 0f) {
            currentCombo++;
            comboTime = initialComboTime;
        }

        if (numberOfBricks == 0) {
            GameState = State.WIN;
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
    }

    private void UpdateLivesOnUI() {
        if (lives > 1) {
            livesText.text = "x" + (lives - 1);
        } else {
            livesText.transform.parent.gameObject.SetActive(false);
        }
    }

    public void RestartGame() {
        Ball newMainBall = Instantiate(mainBallObject, new Vector2(0f, gamepad.transform.position.y + startBallDistanceYFromGamePad),
            Quaternion.identity).GetComponent<Ball>();
        newMainBall.GetComponent<AnimatedReveal>().enabled = false;

        StartCoroutine(centerTextAnimation.StartAnimation(Animation.ALPHA, textToDisplay: "One more time !"));
        StartCoroutine(StartGame(false));
    }

    private void FixedUpdate() {
        MousePositionX = mainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, 0f)).x;
    }
}
