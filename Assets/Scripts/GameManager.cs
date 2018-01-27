using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    [SerializeField] private Ball mainBall;

    private void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame() {
        while (!AnimatedAppear.AnimationsOver) {
            yield return null;
        }

        Cursor.lockState = CursorLockMode.None;
        mainBall.Launch();
    }
}
