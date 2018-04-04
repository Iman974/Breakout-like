using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public static LevelsInfoData LevelsInfo { get; private set; }

    public delegate void OnLevelUnloadCallback();
    public static event OnLevelUnloadCallback SceneUnload;

    //private static LevelManager instance;

    //private void Awake() {
    //    #region Private Singleton
    //    if (instance != null) {
    //        Destroy(gameObject);
    //        return;
    //    }
    //    instance = this;
    //    #endregion
    //}

    private void Start() {
        if (LevelsInfo == null) {
            LevelsInfo = FindObjectOfType<Preloader>().LevelsInfoData; // if we keep private singleton, then all this is not required anymore
            SceneManager.UnloadSceneAsync(0);
        }

        SceneManager.sceneLoaded += GameManager.Instance.OnLevelLoaded;
    }

    //private void OnDestroy() {
    //    instance = null;
    //}

    //public void PlayGameLevel(string levelName) {
    //    StartCoroutine(LoadGameLevel(levelName));
    //}

    public static void LoadLevelAsync(string levelName) {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(levelName);

        if (loadingOperation == null) {
            throw new System.NullReferenceException("The loading operation failed.");
        }

        if (SceneUnload != null) {
            SceneUnload.Invoke();
        }

        if (SceneManager.GetActiveScene().name == "Menu") { // Really needed ?!
            GameManager.UiManager.OnLevelLoad(loadingOperation);
        } else {
            GameManager.UiManager.OnLevelLoad(loadingOperation, true);
        }
    }

    //public static void GoToMainMenu() {
    //    LoadLevel("Menu");
    //}

    public static void ReloadCurrentLevel() {
        if (SceneUnload != null) {
            SceneUnload.Invoke();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
