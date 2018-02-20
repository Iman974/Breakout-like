using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {

    [SerializeField] private Slider loadingBar;

    [HideInInspector] public int currentWorld = 1;

    private class World {
        public Level[] levels;
        public int worldNumber;
    }

    private class Level {
        public int levelNumber;
    }

    private World[] worlds;

    public void SetCurrentWorld(Text senderText) {
        currentWorld = GetNumberInString(senderText.text);
        //SceneManager.sceneCountInBuildSettings
    }

    public void PlayGameLevel(Text levelText) {
        StartCoroutine(LoadLevel(currentWorld + "-" + GetNumberInString(levelText.text)));
    }

    private IEnumerator LoadLevel(string levelName) {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelName);
        loadOperation.completed += EnableGameManager;

        while (!loadOperation.isDone) {
            loadingBar.transform.parent.gameObject.SetActive(true);
            loadingBar.value = Mathf.Clamp01(loadOperation.progress / 0.9f);
            yield return null;
        }
    }

    private void EnableGameManager(AsyncOperation operation) {
        GameManager.Instance.gameObject.SetActive(true);
        operation.completed -= EnableGameManager;
    }

    private int GetNumberInString(string toParse) {
        return int.Parse(System.Text.RegularExpressions.Regex.Match(toParse, @"\d+").Value);
    }
}
