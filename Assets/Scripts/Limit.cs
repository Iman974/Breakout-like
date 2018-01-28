using UnityEngine;

public class Limit : MonoBehaviour {

    [SerializeField] private float triggerPushTime = 5f;

    private float closeTimer = 0f;
    private Ball mainBall;

    private void Start() {
        mainBall = Ball.MainBall;
    }

    private void FixedUpdate() {
        /*closeTimer += 0.02f;
        if (closeTimer >= triggerPushTime) {
            mainBall.SetDirection(new Vector2(transform.position.x - mainBall.transform.position.x, transform.position.y));
        }*/
    }
}
