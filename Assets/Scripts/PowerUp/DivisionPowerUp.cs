using UnityEngine;

public class DivisionPowerUp : PowerUp {

    [SerializeField] private GameObject subDivisionBall;
    [SerializeField] private int divisionBallCount = 3;
    //[SerializeField] private float subBallDistanceFromMainBall = 0.5f;
    [SerializeField] private float subBallDivisionOffset = 5f;
    [SerializeField] private float subBallSpeedMultiplier = 1.5f;
    [SerializeField] private int maxHitCount = 4;

    public DivisionPowerUp(int ballCount, int newMaxHitCount) {
        divisionBallCount = ballCount;
        maxHitCount = newMaxHitCount;

        DestroyAfterHitCount[] destroyCounters = FindObjectsOfType<DestroyAfterHitCount>();

        foreach (var counter in destroyCounters) {
            counter.maxHitCount = maxHitCount;
        }
    }

    protected override void PowerUpPayload() {
        DivideMainBall();
        base.PowerUpPayload();
    }

    public void DivideMainBall() {
        bool isDivisionEven = divisionBallCount % 2 == 0;
        float count = isDivisionEven ? (divisionBallCount - 1) * 0.5f : (divisionBallCount - 1) / 2f;
        float subBallSpeed = Ball.MainBall.Speed * subBallSpeedMultiplier;
        Ball[] subDivisionBalls = new Ball[divisionBallCount];

        Ball firstSubBall = InstantiateSubBall((Vector2)Ball.MainBall.transform.position + (Ball.MainBall.Direction * 0.5f),
            ((Ball.MainBall.Radius * 100f) * -count) + (subBallDivisionOffset * -count));
        subDivisionBalls[0] = firstSubBall;
        firstSubBall.Speed = subBallSpeed;
        firstSubBall.Direction = firstSubBall.transform.position - Ball.MainBall.transform.position;

        for (int i = 1; i < divisionBallCount; i++) {
            Ball newSubBall = InstantiateSubBall(firstSubBall.transform.position, i * ((firstSubBall.Radius * 100f) +
                subBallDivisionOffset));
            subDivisionBalls[i] = newSubBall;

            newSubBall.Speed = subBallSpeed;
            newSubBall.Direction = newSubBall.transform.position - Ball.MainBall.transform.position;
        }
    }

    private Ball InstantiateSubBall(Vector2 atPosition, float rotationAroundMainBall) {
        Ball newSubBall = Instantiate(subDivisionBall, atPosition, Quaternion.identity).GetComponent<Ball>();
        newSubBall.transform.RotateAround(Ball.MainBall.transform.position, Vector3.forward, rotationAroundMainBall);

        return newSubBall;
    }
}
