using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Handles the UI when the player is in a level.
/// </summary>
public class InGameUIManager : MonoBehaviour {

    private enum GameCanvasElements {
        WinText = 1,
        GameoverText,
        Lives = 4,
        ScoreboardPanel = 8,
        //Sb_score = 16,
        //Sb_lives = 32,
        //Sb_time = 64,
        PausePanel = 128,
        ResumeButton = 256,
        WinGameOverScoreboardPause = WinText | GameoverText | ScoreboardPanel | PausePanel,
        Stars = 512
    }

#pragma warning disable 0649
#pragma warning disable 0414

    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private GameObject scoreboardPanel, pausePanel;
    [SerializeField] private GameObject livesTextContainer;
    [SerializeField] private Slider loadingBar;
    [SerializeField] private Color scoreUpColor = Color.green;
    [SerializeField] private Color scoreDownColor = Color.red;
    [SerializeField] private AnimationCurve smoothLoadingAnimation;
    [SerializeField] private float smoothLoadingSpeed = 1f;

    private GameObject starsImagesContainer;
    private GameObject resumeButton;
    private Text scoreboardScoreText, scoreboardTimeText, scoreboardLivesText;
    private TextAnimation mainText;
    private Text winText, loseText;
    private Text scoreText, livesText;

    private static bool instantiated;
    //private static UIManager instance;

    //public bool IsLoading { get; private set; }

    private void Awake() {
        if (instantiated == true) {
            Destroy(gameObject);
            return;
        }
        instantiated = true;
    }

    private void Start() {
        starsImagesContainer = scoreboardPanel.transform.Find("Stars").gameObject;//scoreboardPanel.transform.GetChild(3).gameObject;
        //livesContainer = livesText.transform.parent.gameObject;

        resumeButton = pausePanel.transform.Find("ResumeButton").gameObject;

        //scoreboardPanel = gameCanvas.transform.Find("ScoreboardPanel").gameObject;
        //pausePanel = gameCanvas.transform.Find("PausePanel").gameObject;

        scoreboardScoreText = scoreboardPanel.transform.Find("ScoreText").GetComponent<Text>();
        scoreboardTimeText = scoreboardPanel.transform.Find("TimeText").GetComponent<Text>();
        scoreboardLivesText = scoreboardPanel.transform.Find("LivesleftText").GetComponent<Text>();

        mainText = gameCanvas.transform.Find("MainText").GetComponent<TextAnimation>();
        scoreText = gameCanvas.transform.Find("ScoreText").GetComponent<Text>();
        winText = gameCanvas.transform.Find("WinText").GetComponent<Text>();
        loseText = gameCanvas.transform.Find("GameOverText").GetComponent<Text>();
        livesText = livesTextContainer.GetComponentInChildren<Text>();

        DontDestroyOnLoad(gameCanvas);
        DontDestroyOnLoad(loadingBar.transform.parent.gameObject);
    }

    public void OnLevelLoad(AsyncOperation loadingOperation, bool cursorVisibleAtEnd = false) {
        loadingBar.transform.parent.gameObject.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        loadingOperation.allowSceneActivation = false;

        StartCoroutine(ShowLoadingProgress(loadingOperation, cursorVisibleAtEnd));
    }

    private IEnumerator ShowLoadingProgress(AsyncOperation loadingOperation, bool cursorVisibleAtEnd) {
        float newProgress = 0f, startValue = 0f, timeToEval = 0f;
        bool isLerpOver = true;

        while (!Mathf.Approximately(loadingBar.value, loadingBar.maxValue)) {
            yield return null;

            if (!Mathf.Approximately(loadingOperation.progress, newProgress)) {
                newProgress = loadingOperation.progress;
                startValue = loadingBar.value;
                timeToEval = 0f;
            } else if (isLerpOver) {
                continue;
            }

            timeToEval += smoothLoadingSpeed * Time.deltaTime;
            loadingBar.value = Mathf.Lerp(startValue, newProgress, smoothLoadingAnimation.Evaluate(timeToEval));

            isLerpOver = timeToEval >= 1f;
        }

        loadingOperation.allowSceneActivation = true;
        yield return new WaitForEndOfFrame();

        loadingBar.transform.parent.gameObject.SetActive(false);
        loadingBar.value = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = cursorVisibleAtEnd;
    }

    public void OnLevelLoaded() {
        if (!gameCanvas.activeSelf) {
            gameCanvas.SetActive(true);
        }
    }

    public void OnGamePause(bool pause) {
        Cursor.visible = pause;
        SetActiveUIElements(GameCanvasElements.PausePanel, pause);
    }

    public void OnBeforeBallRelaunch() {
        StartCoroutine(mainText.StartAnimation(Animation.ALPHA, textToDisplay: "One more time !"));
    }

    public void UpdateLives() {
        int lives = GameManager.Instance.Lives;

        if (lives > 1) {
            livesText.text = "x" + (lives - 1);
        } else {
            SetActiveUIElements(GameCanvasElements.Lives, false);
        }
    }

    public void UpdateScore(int newScore) {
        if (scoreText == null) {
            return;
        }

        scoreText.text = "Score: " + newScore;
    }

    /// <summary>
    /// Makes a specific amount of stars appear based on the player's score.
    /// </summary>
    /// <param name="starsCount">
    /// How many stars to make appear.
    /// </param>
    public void ShowStars(int starsCount) {
        for (int i = 0; i < starsCount; i++) {
            starsImagesContainer.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Displays everything related to when the player wins.
    /// </summary>
    public void DisplayWin() {
        SetActiveUIElements(GameCanvasElements.WinText | GameCanvasElements.ScoreboardPanel, true);

        scoreboardScoreText.text = scoreText.text;
        scoreboardTimeText.text = "Time: " + GameManager.PlayTime + "s";
        scoreboardLivesText.text = "Lives left: " + GameManager.Instance.Lives + "/" + GameManager.LevelData.LevelLives;

        ShowStars(GameManager.Instance.CountStars());
    }

    /// <summary>
    /// Displays everything related to when the player loses.
    /// </summary>
    public void DisplayGameOver() {
        SetActiveUIElements(GameCanvasElements.GameoverText | GameCanvasElements.PausePanel, true);
        SetActiveUIElements(GameCanvasElements.ResumeButton, false);
        Cursor.visible = true;
    }

    public void OnLevelUnload(bool isReloading = false) {
        if (!isReloading) {
            gameCanvas.SetActive(false);
        }

        SetActiveUIElements(GameCanvasElements.WinGameOverScoreboardPause | GameCanvasElements.Stars, false);
        SetActiveUIElements(GameCanvasElements.ResumeButton, true);
        Cursor.visible = false;
    }

    /// <summary>
    /// Shorthand for setting active or disabling multiple UI elements at a time.
    /// </summary>
    /// <param name="element">
    /// Which elements to enable/disable.
    /// </param>
    /// <param name="active">
    /// Enable or disable the given elements ?
    /// </param>
    private void SetActiveUIElements(GameCanvasElements element, bool active) {
        if ((element & GameCanvasElements.GameoverText) != 0) {
            loseText.gameObject.SetActive(active);
        }

        //if ((element & UIElements.Sb_lives) != 0) {
        //    scoreboardLivesText.gameObject.SetActive(active);
        //}

        //if ((element & UIElements.Sb_score) != 0) {
        //    scoreboardScoreText.gameObject.SetActive(active);
        //}

        //if ((element & UIElements.Sb_time) != 0) {
        //    scoreboardTimeText.gameObject.SetActive(active);
        //}

        if ((element & GameCanvasElements.ScoreboardPanel) != 0) {
            scoreboardPanel.gameObject.SetActive(active);
        }

        if ((element & GameCanvasElements.WinText) != 0) {
            winText.gameObject.SetActive(active);
        }

        if ((element & GameCanvasElements.Lives) != 0) {
            livesTextContainer.SetActive(active);
        }

        if ((element & GameCanvasElements.PausePanel) != 0) {
            pausePanel.gameObject.SetActive(active);
        }

        if ((element & GameCanvasElements.ResumeButton) != 0) {
            resumeButton.SetActive(active);
        }

        if ((element & GameCanvasElements.Stars) != 0) {
            for (int i = 0; i < starsImagesContainer.transform.childCount; i++) {
                starsImagesContainer.transform.GetChild(i).gameObject.SetActive(active);
            }
        }
    }
}
