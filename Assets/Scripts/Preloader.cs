using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour {

    //[SerializeField] private GameObject levelManagerObject;
    [SerializeField] private LevelsInfoData levelsInfoData;

    public LevelsInfoData LevelsInfoData {
        get {
            return levelsInfoData;
        }
    }

    private void Start() {
        SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
    }
}
