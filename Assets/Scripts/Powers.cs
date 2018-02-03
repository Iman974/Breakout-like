using UnityEngine;
using System.Collections;

public class Powers : MonoBehaviour {

    [SerializeField] private float speedUpMultiplier;
    [SerializeField] private int divisionBallCount = 3;
    [SerializeField] private GameObject subDivisionBall;
    [SerializeField] private float subBalldistanceFromMainBall = 1f;

    public static Powers Instance;

    private float speedBeforePowerUp;

    private void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }
        #endregion
    }

    private IEnumerator SpeedUp(float powerUpDuration) {
        Ball.MainBall.DoSpeedUpOverTime = false;
        speedBeforePowerUp = Ball.MainBall.Speed;
        Ball.MainBall.Speed *= speedUpMultiplier;

        yield return new WaitForSeconds(powerUpDuration);

        Ball.MainBall.Speed = speedBeforePowerUp;
        Ball.MainBall.DoSpeedUpOverTime = true;
    }

    private IEnumerator Destruction(float powerUpDuration) {
        foreach (Collider2D collider in Brick.brickColliders) {
            collider.isTrigger = true;
        }

        yield return new WaitForSeconds(powerUpDuration);

        foreach (Collider2D collider in Brick.brickColliders) {
            collider.isTrigger = false;
        }
    }

    private IEnumerator Division() {
        for (int i = 0; i < divisionBallCount; i++) {
            Ball newSubBall = Instantiate(subDivisionBall, (Vector2)Ball.MainBall.transform.position +
                Ball.MainBall.Direction.normalized * subBalldistanceFromMainBall, Quaternion.identity).GetComponent<Ball>();
            newSubBall.Speed = Ball.MainBall.Speed;
            newSubBall.Direction = Ball.MainBall.Direction;
            newSubBall.transform.RotateAround(Ball.MainBall.transform.position, Vector3.forward, 10f * i);
        }
        yield return null;
    }
}
