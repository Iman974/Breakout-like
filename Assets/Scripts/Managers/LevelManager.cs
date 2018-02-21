using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {

    [SerializeField] private Slider loadingBar;
    [SerializeField] private LevelsInfo levelsInfo;

    private static bool instantiated;

    [HideInInspector] public int currentWorld = 1;

    private void Awake() {
        if (instantiated == true) {
            Destroy(gameObject);
            return;
        }
        instantiated = true;
        //DontDestroyOnLoad(gameObject);
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
            if (loadingBar != null) {
                loadingBar.transform.parent.gameObject.SetActive(true);
                loadingBar.value = Mathf.Clamp01(loadOperation.progress / 0.9f);
            }
            yield return null;
        }
    }

    public void ReturnToMainMenu() {
        StartCoroutine(LoadLevel("Menu", false));
    }

    private static void EnableGameManager(AsyncOperation operation) {
        GameManager.Instance.gameObject.SetActive(true);
        operation.completed -= EnableGameManager;
    }

    public static int GetNumberInString(string toParse) {
        return int.Parse(System.Text.RegularExpressions.Regex.Match(toParse, @"\d+").Value);
    }
}
