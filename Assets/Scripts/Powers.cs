using UnityEngine;
using System.Collections;

public class Powers : MonoBehaviour {

    [SerializeField] private float speedUpMultiplier;
    [SerializeField] private int divisionBallCount = 3;
    [SerializeField] private GameObject subDivisionBall;
    [SerializeField] private float subBallDistanceFromMainBall = 0.5f;
    [SerializeField] private float subBallDivisionOffset = 5f;
    [SerializeField] private float subBallSpeedMultiplier = 1.5f;

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
        bool isDivisionEven = divisionBallCount % 2 == 0;
        float count = isDivisionEven ? (divisionBallCount - 1) * 0.5f : (divisionBallCount - 1) / 2f;
        float subBallSpeed = Ball.MainBall.Speed * subBallSpeedMultiplier;

        Ball firstSubBall = InstantiateSubBall((Vector2)Ball.MainBall.transform.position + (Ball.MainBall.Direction *
                subBallDistanceFromMainBall), ((Ball.MainBall.Radius * 100f) * -count) + (subBallDivisionOffset * -count));
        firstSubBall.Speed = subBallSpeed;
        firstSubBall.Direction = firstSubBall.transform.position - Ball.MainBall.transform.position;

        for (int i = 0; i < divisionBallCount; i++) {
            Ball newSubBall = InstantiateSubBall(firstSubBall.transform.position, i * ((firstSubBall.Radius * 100f) +
                subBallDivisionOffset));

            newSubBall.Speed = subBallSpeed;
            newSubBall.Direction = newSubBall.transform.position - Ball.MainBall.transform.position;
        }
        yield return null;
    }

    private Ball InstantiateSubBall(Vector2 atPosition, float rotationAroundMainBall) {
        Ball newSubBall = Instantiate(subDivisionBall, atPosition, Quaternion.identity).GetComponent<Ball>();
        newSubBall.transform.RotateAround(Ball.MainBall.transform.position, Vector3.forward, rotationAroundMainBall);

        return newSubBall;
    }
}
