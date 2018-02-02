using UnityEngine;

public class Limit : MonoBehaviour {

    [SerializeField] private float triggerPushTime = 5f;

    private float closeTimer = 0f;

    private void FixedUpdate() {
        /*closeTimer += 0.02f;
        if (closeTimer >= triggerPushTime) {
            mainBall.SetDirection(new Vector2(transform.position.x - mainBall.transform.position.x, transform.position.y));
        }*/
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Ball")) {
            return;
        }

        GameManager.Instance.Lives--;
        Destroy(other.gameObject, 2f);
        if (GameManager.Instance.GameState != GameManager.State.WIN) {
            GameManager.Instance.Invoke("RestartGame", 2f);
        }
    }
}
