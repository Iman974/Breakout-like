using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour {

    private void Start() {
        SceneManager.LoadScene("Menu");
    }
}
