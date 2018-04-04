using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    [System.Serializable]
    public enum State {
        OutOfLevel,
        Launch,
        Playing,
        Restart,
        GameOver,
        Win,
        //NotGameOverNorRestart = ~(GameOver | Restart) // Further research required
    }

    //[SerializeField] private Text winText, loseText;
    [SerializeField] private TextAnimation mainText;
    //[SerializeField] private TextAnimation scoreTextAnimation;
    [SerializeField] private GameObject mainBallPrefab;
    [SerializeField] private GamepadController gamepadPrefab;
    //[SerializeField] private Text livesText;
    //[SerializeField] private Image scoreboard;
    //[SerializeField] private GameObject starsContainerUI;
    [SerializeField] private float comboTime = 0.75f;
    [SerializeField] private int startedEarlyPenalty = -15;
    //[SerializeField] private Color scoreUpColor = Color.green;
    //[SerializeField] private Color scoreDownColor = Color.red;
    [SerializeField] private int lives = 1;
    [SerializeField] private float[] scoreStarLevels = new float[] { 1f, 1.5f, 2f };
    [SerializeField] private AnimationCurve restartAnimation;
    [SerializeField] private float restartSpeed = 2f;

    private GamepadController gamepad;
    private Camera mainCamera;
    private int score;
    private int currentCombo = 1;
    private float initialComboTime;
    private bool gamePaused;
    private PowerUpSpawner powerUpSpawner;
    private bool isLevelInitializing;
    private bool isReloading;

    /// <summary>
    /// Was the game paused the previous frame ?
    /// </summary>
    private bool wasGamePaused;

    private string saveFilePath;
    private Level currentLevel;

    /// <summary>
    /// The time in seconds since the start of the level (when the ball was lauched for the first time).
    /// </summary>
    public static float PlayTime { get; private set; }

    public static GameManager Instance { get; private set; }

    public static float MousePositionX {
        get {
            return Instance.mainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, 0f)).x; // Same as Camera.main... because of instance not cached ?
        }
    }

    public static LevelData LevelData {
        get {
            return Instance.currentLevel.Data;
        }
    }

    public static InGameUIManager UiManager { get; private set; }

    public static State GameState { get; private set; }

    public int Lives {
        get {
            return lives;
        }
        set {
            if (GameState != State.Win) { // Problem

                bool isLoss = value < lives;
                lives = value;

                if (isLoss) {
                    OnLoseLife();
                }
            }
        }
    }

    /// <summary>
    /// How many stars the player has earned since the very start of the game.
    /// </summary>
    public static int TotalStarsCount { get; private set; }

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
        saveFilePath = Application.persistentDataPath + "/gamesave.sav";

        DontDestroyOnLoad(gameObject);

        UiManager = GetComponent<InGameUIManager>();
        powerUpSpawner = GetComponent<PowerUpSpawner>();

        LoadProgress();

        enabled = false;
    }

    // This is called when a level is loaded, just before OnEnable message.
    public void OnLevelLoaded(Scene scene, LoadSceneMode mode) {
        if (!isReloading) {
            LevelManager.SceneUnload += OnBeginLevelUnload;
            //SceneManager.sceneUnloaded += OnLevelUnloading;
            //gameObject.SetActive(true);
            UiManager.OnLevelLoaded(); // Use events for those later maybe ?

            currentLevel = LevelManager.LevelsInfo[scene.name];
        } else {
            isReloading = false;
        }

        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;

        ProcessLevelData();
        StartCoroutine(InitLevel());
        StartCoroutine(StartLevel());
    }

    private void ProcessLevelData() {
        Instantiate(mainBallPrefab, LevelData.MainBallPosition, Quaternion.identity);

        gamepad = Instantiate(gamepadPrefab, LevelData.GamepadPosition, Quaternion.identity);
    }

    private IEnumerator InitLevel() {
        isLevelInitializing = true;
        Lives = LevelData.LevelLives;

        AddToScore(0);
        //InitBrickPowers();

        while (CustomAnimation.IsAnimationRunning) {
            yield return null;
        }

        Cursor.lockState = CursorLockMode.None;
        enabled = true;

        //StartCoroutine(mainText.StartAnimation(Animation.SIZE, 1f, "3", "2", "1", "Go !"));
        isLevelInitializing = false;
    }

    private static void InitBrickPowers() {
        //List<Brick> bricks = new List<Brick>(System.Array.ConvertAll(GameObject.FindGameObjectsWithTag("Brick"), x => x.GetComponent<Brick>()));

        for (int i = 0; i < LevelData.PoweredBrickCount; i++) {
            //int randomIndex = Random.Range(0, bricks.Count);

            //BrickPower brickPower = bricks[randomIndex].gameObject.AddComponent<DestroyBrickPower>();
            //bricks.RemoveAt(randomIndex);

            //brickPower = LevelData.BrickPowers[Random.Range(0, LevelData.BrickPowers.Length)];
        }
    }

    private IEnumerator StartLevel(bool makeRestart = false) {
        GameState = State.Launch;

        Ball.Main.transform.parent = gamepad.transform;
        Ball.Main.transform.localPosition = new Vector3(0f, Ball.Main.transform.localPosition.y);

        while (isLevelInitializing || !(Input.GetButtonUp("Fire1") && !wasGamePaused)) {
            yield return null;
        }

        if (!makeRestart) {
            PlayTime = Time.time;

            if (mainText.AnimatingString != null && mainText.AnimatingString != "Go !") {//
                AddToScore(startedEarlyPenalty); // Go away !
            }//
        }

        Ball.Main.transform.parent = null;
        Ball.Main.Launch();

        powerUpSpawner.enabled = true;
        powerUpSpawner.OnBallLaunch();

        GameState = State.Playing;
    }

    public void RemoveBrick(int scoreValue) {
        AddToScore(scoreValue * currentCombo);

        if (comboTime > 0f) {
            currentCombo++;
            comboTime = initialComboTime;
        }

        if (Brick.Bricks.Count == 0) {
            GameState = State.Win;
            EndGame();
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
            wasGamePaused = true;
            PauseGame(!gamePaused);
        } else if (wasGamePaused) {
            wasGamePaused = false;
        }
    }

    //public void UnPauseGame() {
    //    PauseGame(false);
    //}

    public void PauseGame(bool pause) {
        if (gamePaused == pause || GameState == State.Win) {
            return;
        }

        gamePaused = pause;
        Time.timeScale = pause ? 0f : 1f;

        //enabled = !pause;
        //powerUpSpawner.enabled = !pause;
        SetEnableOnGameBehaviours(!pause);

        UiManager.OnGamePause(pause);
    }

    public void GoToMainMenu() {
        LevelManager.LoadLevelAsync("Menu");
    }

    /// <summary>
    /// Shorthand for disabling the main gameplay component.
    /// </summary>
    /// <param name="enable">
    /// Enable or disable the main gameplay components ?
    /// </param>
    private void SetEnableOnGameBehaviours(bool enable) {
        enabled = enable;
        powerUpSpawner.enabled = enable;

        if (gamepad != null) {
            gamepad.enabled = enable; // Does it need to be disabled as well as the other behaviours ?
        }
    }

    public void ReloadLevel() {
        isReloading = true;
        PauseGame(false);
        powerUpSpawner.StopSpawning();

        SetEnableOnGameBehaviours(false);
        Cursor.lockState = CursorLockMode.Locked;

        if (GameState != State.GameOver && GameState != State.Restart/*(GameState & State.NotGameOverNorRestart) != 0 -> Research about that*/) {
            StartCoroutine(RevertToStart());
        } else {
            LevelManager.ReloadCurrentLevel();
        }
    }

    /// <summary>
    /// Animates a sort of rewind effect before the level can reload.
    /// </summary>
    private IEnumerator RevertToStart() {
        Ball.Main.Rb2D.isKinematic = true;
        Ball.Main.Speed = 0f;

        Vector2 startBallPosition = Ball.Main.Position;
        Vector2 startGamepadPosition = gamepad.Position;

        for (float timeToEval = 0f; timeToEval < 1f; timeToEval += restartSpeed * Time.deltaTime) {
            Ball.Main.Position = Vector2.Lerp(startBallPosition, LevelData.MainBallPosition, restartAnimation.Evaluate(timeToEval));
            gamepad.Position = Vector2.Lerp(startGamepadPosition, LevelData.GamepadPosition, restartAnimation.Evaluate(timeToEval));

            yield return null;
        }

        //Cursor.lockState = CursorLockMode.Locked;
        LevelManager.ReloadCurrentLevel();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.lockState = CursorLockMode.None;
    }

    private void ResetScore() {
        score = 0;
        UiManager.UpdateScore(0);
    }

    private void OnLoseLife() {
        if (GameState != State.Playing) {
            return;
        }

        UiManager.UpdateLives();
        powerUpSpawner.StopSpawning();

        if (lives <= 0) {
            GameState = State.GameOver;
            EndGame();
            return;
        }

        GameState = State.Restart;

        Invoke("NextLife", 2f);
    }

    /// <summary>
    /// Gives back a ball to the player to play again and handles restart.
    /// </summary>
    private void NextLife() {
        Instantiate(mainBallPrefab, LevelData.MainBallPosition, Quaternion.identity);

        powerUpSpawner.DestroyPowerUps();
        UiManager.OnBeforeBallRelaunch();

        StartCoroutine(StartLevel(true));
    }

    private void EndGame() {
        SetEnableOnGameBehaviours(false);

        powerUpSpawner.DestroyPowerUps();
        powerUpSpawner.StopSpawning();

        if (GameState == State.Win) {
            Win();
        } else {
            GameOver();
        }
    }

    private void GameOver() {
        GameState = State.GameOver;

        UiManager.DisplayGameOver();
    }

    private void Win() {
        GameState = State.Win;

        PlayTime = Mathf.Round(Time.time - PlayTime);
        UiManager.DisplayWin();

        LevelStats currentLevelStats = LevelManager.LevelsInfo[currentLevel.Name].Stats;
        int newStarsCount = CountStars();

        if (!currentLevelStats.IsDone) {
            currentLevelStats.StarsCount = newStarsCount;
            currentLevelStats.IsDone = true;

            TotalStarsCount += newStarsCount;
        } else if (newStarsCount > currentLevelStats.StarsCount) { // New record
            TotalStarsCount += newStarsCount - currentLevelStats.StarsCount;
            currentLevelStats.StarsCount = newStarsCount;
        }
        
        //ProgressSave.SaveProgress(saveFilePath);
    }

    /// <summary>
    /// Counts how many stars the player has earned based on his score.
    /// </summary>
    public int CountStars() {
        int stars = 0;

        foreach (var scoreLevel in scoreStarLevels) {
            if (score >= Brick.TotalScoreValue * scoreLevel) {
                stars++;
            }
        }
        return stars;
    }

    /// <summary>
    /// Loads the game progress from the save file into the game.
    /// </summary>
    private void LoadProgress() {
        ProgressSave progressSave = ProgressSave.GetProgressSave(saveFilePath);

        if (progressSave == null) {
            return;
        }

        LevelsInfoData levelsInfo = FindObjectOfType<Preloader>().LevelsInfoData;

        for (int i = 0; i < levelsInfo.TotalLevelCount; i++) {
            levelsInfo[i].Stats = progressSave[levelsInfo[i].Name];
        }
        TotalStarsCount = progressSave.TotalStarsCount;
    }

    // This is called as the loading bar appears and the level starts unloading.
    public void OnBeginLevelUnload() {
        //Debug.Log("Unload");
        ResetScore();
        powerUpSpawner.StopSpawning();
        Brick.ResetBricks();

        if (IsInvoking("NextLife")) {
            CancelInvoke();
        }

        PauseGame(false);
        UiManager.OnLevelUnload(isReloading);

        if (isReloading) {
            return;
        }

        LevelManager.SceneUnload -= OnBeginLevelUnload;
        SceneManager.sceneLoaded -= OnLevelLoaded;

        currentLevel = null;
        GameState = State.OutOfLevel;
        enabled = false;
        powerUpSpawner.enabled = false;
        //gameObject.SetActive(false);
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnLevelLoaded;
        LevelManager.SceneUnload -= OnBeginLevelUnload;
    }
}
