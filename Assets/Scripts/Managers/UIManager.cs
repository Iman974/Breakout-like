using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField] private Text winText, loseText;
    [SerializeField] private Text scoreText, livesText;
    [SerializeField] private GameObject scoreboardPanel;

    [SerializeField] private Color scoreUpColor = Color.green;
    [SerializeField] private Color scoreDownColor = Color.red;

    private GameObject starsContainer;
    private Text scoreboardText;
    private Text scoreboardTimeText;
    private Text scoreboardLivesText;

    private static bool instantiated;
    //private static UIManager instance;

    private void Awake() {
        if (instantiated == true) {
            Destroy(gameObject);
            return;
        }
        instantiated = true;
    }

    private void OnEnable() {
        starsContainer = scoreboardPanel.transform.GetChild(3).gameObject;
    }

    public void UpdateLives() {
        int lives = GameManager.Instance.Lives;
        if (lives > 1) {
            livesText.text = "x" + (lives - 1);
        } else {
            livesText.transform.parent.gameObject.SetActive(false);
        }
    }

    public void UpdateScore(int newScore) {
        scoreText.text = "Score: " + newScore;
    }

    public void ShowStars(int amount) {
        for (int i = 0; i < amount; i++) {
            starsContainer.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void OnGameStateChanged() {
        GameManager.State newState = GameManager.Instance.GameState;
        switch (newState) {
            case GameManager.State.WIN:
                winText.gameObject.SetActive(true);
                scoreboardPanel.gameObject.SetActive(true);
                ShowStars(GameManager.Instance.CalculateStars());
                break;
            case GameManager.State.GAMEOVER:
                loseText.gameObject.SetActive(true);
                break;
        }
    }
}
