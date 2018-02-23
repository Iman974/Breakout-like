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

    [HideInInspector] public int currentWorld = 1; // Use the name of the level stored instead (1-1)

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start() {
        RectTransform levelButtonRectTransform = levelButtonPrefab.GetComponent<RectTransform>();
        float levelButtonWidth = levelButtonRectTransform.rect.width;
        float levelButtonHeight = levelButtonRectTransform.rect.height;
        Vector2 levelButtonPos;
        mainCamera = Camera.main;

        foreach (var world in levelsInfo.worlds) {
            for (int i = 0; i < world.levels.Count; i++) {
                levelButtonPos.x = levelPanel.rect.xMin + (levelButtonWidth * 0.5f);
                levelButtonPos.y = levelPanel.rect.yMax - (levelButtonHeight * 0.5f);

                Transform levelButtonTransform = Instantiate(levelButtonPrefab, Vector3.zero, Quaternion.identity, levelPanel).transform;
                levelButtonTransform.localPosition = levelButtonPos;
                Button levelButton = levelButtonTransform.GetComponent<Button>();
                
            }
        }
    }

    private void OnDestroy() {
        instance = null;
    }

    public void SetCurrentWorld(Text senderText) {
        currentWorld = RegexUtility.GetNumberInString(senderText.text);
    }

    public void PlayGameLevel(int level) {
        StartCoroutine(LoadLevel(currentWorld + "-" + level));
    }

    private IEnumerator LoadLevel(string levelName, bool enableGM = true) {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelName);
        if (enableGM) {
            loadOperation.completed += EnableGameManager;
        }

        while (!loadOperation.isDone) {
            loadingBar.transform.parent.gameObject.SetActive(true);
            loadingBar.value = loadOperation.progress;
            yield return null;
        }
    }

    private static void EnableGameManager(AsyncOperation operation) {
        GameManager.Instance.gameObject.SetActive(true);
        operation.completed -= EnableGameManager;
    }

}
