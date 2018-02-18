﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [System.Serializable]
    public enum State {
        MAINMENU,
        LAUNCH,
        PLAYING,
        RESTART,
        GAMEOVER,
        WIN
    }

    //[SerializeField] private Text winText, loseText;
    [SerializeField] private TextAnimation centerTextAnimation;
    //[SerializeField] private TextAnimation scoreTextAnimation;
    [SerializeField] private GameObject mainBallObject;
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
    //[SerializeField] private State debugState;

    private GamepadController gamepad;
    private Camera mainCamera;
    private int score;
    private int currentCombo = 1;
    private float initialComboTime;
    private UIManager UiManager;

    public static GameManager Instance { get; private set; }
    public float MousePositionX { get; private set; }

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
        }
        #endregion
        initialComboTime = comboTime;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        gamepad = GameObject.FindWithTag("GameController").GetComponent<GamepadController>();
        UiManager = GetComponent<UIManager>();

        GameState = State.MAINMENU;
        StartCoroutine(StartGame());
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

        if (Brick.brickColliders.Count == 0) {
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
        /*if (debugState != 0) {
            GameState = State.WIN;
            debugState = 0;
        }*/
    }

    public void RestartGame() {
        GameState = State.RESTART;
        Ball newMainBall = Instantiate(mainBallObject, new Vector2(0f, gamepad.transform.position.y + startBallDistanceYFromGamePad),
            Quaternion.identity).GetComponent<Ball>();

        StartCoroutine(centerTextAnimation.StartAnimation(Animation.ALPHA, textToDisplay: "One more time !"));
        StartCoroutine(StartGame(false));
    }

    private void FixedUpdate() {
        MousePositionX = mainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, 0f)).x;
    }

    public int CalculateStars() {
        int stars = 0;
        foreach (var scoreLevel in scoreStarLevels) {
            if (score >= Brick.totalScoreValue * scoreLevel) {
                stars++;
            }
        }
        return stars;
    }
}
