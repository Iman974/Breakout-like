using UnityEngine;

public class DivisionPowerUp : PowerUp {

    [SerializeField] private Ball subBallPrefab;
    [SerializeField] private int divisionBallCount = 3;
    //[SerializeField] private float subBallDistanceFromMainBall = 0.5f;
    [SerializeField] private float subBallDivisionOffset = 5f;
    [SerializeField] private float subBallSpeedMultiplier = 1.5f;
    [SerializeField] private int maxHitCount = 4;

    public DivisionPowerUp(int ballCount, int newMaxHitCount) {
        divisionBallCount = ballCount;
        maxHitCount = newMaxHitCount;

        TimedDestroy[] destroyCounters = FindObjectsOfType<TimedDestroy>();

        foreach (var counter in destroyCounters) {
            counter.HitCount = maxHitCount;
        }
    }

    protected override void PowerUpPayload() {
        DivideMainBall();
        base.PowerUpPayload();
    }

    public void DivideMainBall() {
        bool isDivisionEven = divisionBallCount % 2 == 0;
        float count = isDivisionEven ? (divisionBallCount - 1) * 0.5f : (divisionBallCount - 1) / 2f;
        float subBallSpeed = Ball.Main.Speed * subBallSpeedMultiplier;
        Ball[] subDivisionBalls = new Ball[divisionBallCount];

        Ball firstSubBall = InstantiateSubBall(Ball.Main.Position + (Ball.Main.Direction * 0.5f),
            ((Ball.Main.Radius * 100f) * -count) + (subBallDivisionOffset * -count));
        subDivisionBalls[0] = firstSubBall;
        firstSubBall.Speed = subBallSpeed;
        firstSubBall.Direction = firstSubBall.Position - Ball.Main.Position;

        for (int i = 1; i < divisionBallCount; i++) {
            Ball newSubBall = InstantiateSubBall(firstSubBall.transform.position, i * ((firstSubBall.Radius * 100f) +
                subBallDivisionOffset));
            subDivisionBalls[i] = newSubBall;

            newSubBall.Speed = subBallSpeed;
            newSubBall.Direction = newSubBall.Position - Ball.Main.Position;
        }
    }

    private Ball InstantiateSubBall(Vector2 atPosition, float rotationAroundMainBall) {
        Ball newSubBall = Instantiate(subBallPrefab, atPosition, Quaternion.identity);

        newSubBall.transform.RotateAround(Ball.Main.Position, Vector3.forward, rotationAroundMainBall);
        return newSubBall;
    }
}
