using UnityEngine;
using System.Collections;

public class Powers : MonoBehaviour {

    [SerializeField] private float speedUpMultiplier;
    [SerializeField] private int divisionBallCount = 3;
    [SerializeField] private GameObject subDivisionBall;
    [SerializeField] private float subBalldistanceFromMainBall = 1f;
    [SerializeField] private float subBallDivisionOffset = 5f;

    public static Powers Instance;

    private float speedBeforePowerUp;
    private bool isDivisionEven;

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
        isDivisionEven = divisionBallCount % 2 == 0;
        int count = isDivisionEven ? divisionBallCount / 2 : (divisionBallCount - 1) / 2;

        Ball firstSubBall = Instantiate(subDivisionBall, (Vector2)Ball.MainBall.transform.position +
            Ball.MainBall.Direction.normalized * subBalldistanceFromMainBall, Quaternion.identity).GetComponent<Ball>(); // To Change
        firstSubBall.Speed = Ball.MainBall.Speed;
        firstSubBall.Direction = Ball.MainBall.Direction;

        firstSubBall.transform.RotateAround(Ball.MainBall.transform.position, Vector3.forward, ((firstSubBall.Radius * 100f) *
            (0 - count)) + (subBallDivisionOffset * (0 - count)));

        for (int i = 1; i < divisionBallCount; i++) {
            Ball newSubBall = Instantiate(subDivisionBall, (Vector2)firstSubBall.transform.position,
                Quaternion.identity).GetComponent<Ball>();

            newSubBall.transform.RotateAround(Ball.MainBall.transform.position, Vector3.forward, i * (firstSubBall.Radius * 100f) +
                (subBallDivisionOffset * i));
        }
        yield return null;
    }
}
