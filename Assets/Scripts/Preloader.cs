using UnityEngine;

public class Preloader : MonoBehaviour {

    private void Start() {
        GameManager.Instance.GoToMainMenu();
    }
}
