using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {

    [SerializeField] private LevelsInfoData levelsInfo;
    [SerializeField] private Slider loadingBar;
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private RectTransform levelPanel;

    private static LevelManager instance;
    private Camera mainCamera;

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start() {
        mainCamera = Camera.main;

        foreach (var world in levelsInfo.worlds) {
            for (int i = 0; i < world.levels.Count; i++) {
                Text levelText = Instantiate(levelButtonPrefab, Vector3.zero, Quaternion.identity, levelPanel).GetComponentInChildren<Text>();
                levelText.text = (i + 1).ToString();
                LevelButton levelButton = levelText.GetComponentInParent<LevelButton>();
                levelButton.level = world.levels[i];
            }
        }
    }

    private void OnDestroy() {
        instance = null;
    }

    public void PlayGameLevel(string levelName) {
        StartCoroutine(LoadGameLevel(levelName));
    }

    private IEnumerator LoadGameLevel(string levelName) {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelName);
        if (loadOperation == null) {
            yield break;
        }
        loadOperation.completed += EnableGameManager;
        GameManager.Instance.CurrentLevelData = levelsInfo[levelName].levelData;

        while (!loadOperation.isDone) {
            loadingBar.transform.parent.gameObject.SetActive(true);
            loadingBar.value = loadOperation.progress;
            yield return null;
        }
    }

    private void EnableGameManager(AsyncOperation operation) {
        GameManager.Instance.gameObject.SetActive(true);
        operation.completed -= EnableGameManager;
    }

}
