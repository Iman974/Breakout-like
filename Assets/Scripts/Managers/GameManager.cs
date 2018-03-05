using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    [System.Serializable]
    public enum State {
        LAUNCH,
        PLAYING,
        RESTART,
        GAMEOVER,
        WIN,
        EARLYEXIT
    }

    //[SerializeField] private Text winText, loseText;
    [SerializeField] private TextAnimation centerTextAnimation;
    [SerializeField] private GameObject pausePannel;
    //[SerializeField] private TextAnimation scoreTextAnimation;
    [SerializeField] private GameObject mainBallObject;
    [SerializeField] private GameObject gamepadObject;
    //[SerializeField] private Text livesText;
    //[SerializeField] private Image scoreboard;
    //[SerializeField] private GameObject starsContainerUI;
    [SerializeField] private float comboTime = 0.75f;
    [SerializeField] private int startedEarlyPenalty = -15;
    //[SerializeField] private Color scoreUpColor = Color.green;
    //[SerializeField] private Color scoreDownColor = Color.red;
    [SerializeField] private int lives = 1;
    [SerializeField] private float startBallDistanceYFromGamePad = 1f;
    [SerializeField] private float[] scoreStarLevels = new float[] { 1f, 1.5f, 2f };

    private GamepadController gamepad;
    private Camera mainCamera;
    private int score;
    private int currentCombo = 1;
    private float initialComboTime;
    private UIManager UiManager;
    private bool gamePaused;

    public static GameManager Instance { get; private set; }
    public float MousePositionX {
        get {
            return mainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, 0f)).x;
        }
    }
    public LevelData CurrentLevelData { get; set; }

    private State gameState;
    public State GameState {
        get {
            return gameState;
        }
        set {
            gameState = value;
            UiManager.OnGameStateChanged();
            if (value > State.PLAYING) {
                PowerUpSpawner.Instance.StopSpawning();
                if (value == State.WIN) {
                    CurrentLevelData.IsDone = true;
                }
            } else if (value == State.PLAYING) {
                PowerUpSpawner.Instance.StartSpawning();
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
                UiManager.UpdateLives();

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
            return;
        }
        #endregion
        initialComboTime = comboTime;
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(centerTextAnimation.transform.parent.gameObject);
        UiManager = GetComponent<UIManager>();
        gameObject.SetActive(false);
        PowerUpSpawner.Instance.enabled = true;
    }

    private void OnEnable() {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        centerTextAnimation.transform.parent.gameObject.SetActive(true); // Enables the game canvas

        Instantiate(mainBallObject, CurrentLevelData.MainBallPosition, Quaternion.identity);
        gamepad = Instantiate(gamepadObject, CurrentLevelData.GamepadPosition, Quaternion.identity).GetComponent<GamepadController>();

        StartCoroutine(StartGame());
    }

    private void OnDisable() {
        if (centerTextAnimation != null) {
            centerTextAnimation.transform.parent.gameObject.SetActive(false); // Disables the game canvas
        }
    }

    private IEnumerator StartGame(bool countDown = true) {
        Lives = lives;
        AddToScore(0);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameState = State.LAUNCH;

        while (CustomAnimation.IsAnimationRunning) {
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
        GameState = State.PLAYING;
    }

    public void RemoveBrick(int scoreValue) {
        AddToScore(scoreValue * currentCombo);
        if (comboTime > 0f) {
            currentCombo++;
            comboTime = initialComboTime;
        }

        if (Brick.BrickColliders.Count == 0) {
            GameState = State.WIN;
        }
    }

    private void AddToScore(int valueToAdd) {
        score += valueToAdd;
        UiManager.UpdateScore(score);
        //if (valueToAdd > 0) {
        //    scoreTextAnimation.ColorToSet = scoreUpColor;
        //} else if (valueToAdd < 0) {
        //    scoreTextAnimation.ColorToSet = scoreDownColor;
        //}

        //if (!scoreTextAnimation.IsAnimationRunning) {
        //    StartCoroutine(scoreTextAnimation.StartAnimation(Animation.COLOR, textToDisplay: "Score: " + score));
        //} else {
        //    scoreTextAnimation.SetText("Score: " + score);
        //}
    }

    private void Update() {
        if (comboTime > 0f) {
            comboTime -= Time.deltaTime;
        } else {
            currentCombo = 1;
            comboTime = initialComboTime;
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            /*gameObject.SetActive(false);
            LevelManager.GoToMainMenu();*/
            PauseGame(!gamePaused);
        }
    }

    public void PauseGame(bool pause) {
        gamePaused = pause;
        Cursor.visible = pause;
        pausePannel.SetActive(pause);
        Time.timeScale = pause ? 0f : 1f;
    }

    public void GoToMainMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void RestartGameLevel() {
        Instantiate(mainBallObject, new Vector2(0f, gamepad.transform.position.y + startBallDistanceYFromGamePad), Quaternion.identity);

        StartCoroutine(centerTextAnimation.StartAnimation(Animation.ALPHA, textToDisplay: "One more time !"));
        StartCoroutine(StartGame(false));
    }

    public int CalculateStars() {
        int stars = 0;
        foreach (var scoreLevel in scoreStarLevels) {
            if (score >= Brick.TotalScoreValue * scoreLevel) {
                stars++;
            }
        }
        return stars;
    }
}
