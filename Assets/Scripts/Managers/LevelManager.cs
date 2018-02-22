using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {

    [SerializeField] private LevelNamesData levelsNames;
    [SerializeField] private Slider loadingBar;

    private static LevelManager instance;

    [HideInInspector] public int currentWorld = 1;

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy() {
        instance = null;
    }

    public void SetCurrentWorld(Text senderText) {
        currentWorld = GetNumberInString(senderText.text);
    }

    public void PlayGameLevel(Text levelText) {
        StartCoroutine(LoadLevel(currentWorld + "-" + GetNumberInString(levelText.text)));
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

    public static int GetNumberInString(string toParse) {
        if (toParse != string.Empty) {
            return int.Parse(System.Text.RegularExpressions.Regex.Match(toParse, @"\d+").Value);
        }
        throw new System.Exception("Empty string");
    }
}
