using UnityEngine;

public class DivideBehaviour : MonoBehaviour, IPowerUp {

    public void ActivatePower() {
        /*bool isDivisionEven = divisionBallCount % 2 == 0;
        float count = isDivisionEven ? (divisionBallCount - 1) * 0.5f : (divisionBallCount - 1) / 2f;
        float subBallSpeed = Ball.MainBall.Speed * subBallSpeedMultiplier;
        Ball[] subDivisionBalls = new Ball[divisionBallCount];

        Ball firstSubBall = InstantiateSubBall((Vector2)Ball.MainBall.transform.position + (Ball.MainBall.Direction *
                subBallDistanceFromMainBall), ((Ball.MainBall.Radius * 100f) * -count) + (subBallDivisionOffset * -count));
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

        int destroyedSubBalls = 0;
        while (destroyedSubBalls < divisionBallCount) {
            for (int i = 0; i < divisionBallCount; i++) {
                if (subDivisionBalls[i] == null) {
                    continue;
                }

                if (subDivisionBalls[i].CollisionCount >= subBallMaxHitCount) {
                    Destroy(subDivisionBalls[i].gameObject);
                    subDivisionBalls[i] = null;
                    destroyedSubBalls++;
                }
            }
            yield return null;
        }*/
    }

    /*private Ball InstantiateSubBall(Vector2 atPosition, float rotationAroundMainBall) {
        Ball newSubBall = Instantiate(subDivisionBall, atPosition, Quaternion.identity).GetComponent<Ball>();
        newSubBall.transform.RotateAround(Ball.MainBall.transform.position, Vector3.forward, rotationAroundMainBall);

        return newSubBall;
    }*/
}
