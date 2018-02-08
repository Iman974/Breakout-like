using UnityEngine;
using System.Collections;

public enum PowersName {
    defaultSpeedUpPower,
    defaultDestroyPower,
    defaultDividePower
}

public class Powers : MonoBehaviour {

    public static Powers Instance;

    public SpeedUpPower defaultSpeedUpPower;
    public DestroyPower defaultDestroyPower;
    public DividePower defaultDividePower;

    private void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }
        #endregion
    }

    [System.Serializable]
    public class SpeedUpPower : IPower {

        [SerializeField] private float speedUpMultiplier = 1.25f;

        private float speedBeforePowerUp;

        public SpeedUpPower(float speedMul, float duration) {
            speedUpMultiplier = speedMul;
            powerDuration = duration;
        }

        public override void ActivatePower() {
            GameManager.Instance.StartCoroutine(SpeedUp());
        }

        private IEnumerator SpeedUp() {
            Ball.MainBall.DoSpeedUpOverTime = false;
            speedBeforePowerUp = Ball.MainBall.Speed;
            Ball.MainBall.Speed *= speedUpMultiplier;

            yield return new WaitForSeconds(powerDuration);

            Ball.MainBall.Speed = speedBeforePowerUp;
            Ball.MainBall.DoSpeedUpOverTime = true;
        }
    }

    [System.Serializable]
    public class DestroyPower : IPower {

        public override void ActivatePower() {
            GameManager.Instance.StartCoroutine(SetBricksAsDestroyable());
        }

        private IEnumerator SetBricksAsDestroyable() {
            foreach (Collider2D collider in Brick.brickColliders) {
                collider.isTrigger = true;
            }

            yield return new WaitForSeconds(powerDuration);

            foreach (Collider2D collider in Brick.brickColliders) {
                collider.isTrigger = false;
            }
        }
    }

    [System.Serializable]
    public class DividePower : IPower {

        [SerializeField] private GameObject subDivisionBall;
        [SerializeField] private int divisionBallCount = 3;
        [SerializeField] private float subBallDistanceFromMainBall = 0.5f;
        [SerializeField] private float subBallDivisionOffset = 5f;
        [SerializeField] private float subBallSpeedMultiplier = 1.5f;
        [SerializeField] private int maxHitCount = 4;

        public DividePower(int ballCount, int newMaxHitCount) {
            divisionBallCount = ballCount;
            maxHitCount = newMaxHitCount;

            DestroyAfterHitCount[] destroyCounters = FindObjectsOfType<DestroyAfterHitCount>();

            foreach (var counter in destroyCounters) {
                counter.maxHitCount = maxHitCount;
            }
        }

        public override void ActivatePower() {
            DivideMainBall();
        }

        public void DivideMainBall() {
            bool isDivisionEven = divisionBallCount % 2 == 0;
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

            /*int destroyedSubBalls = 0;
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

        private Ball InstantiateSubBall(Vector2 atPosition, float rotationAroundMainBall) {
            Ball newSubBall = Instantiate(subDivisionBall, atPosition, Quaternion.identity).GetComponent<Ball>();
            newSubBall.transform.RotateAround(Ball.MainBall.transform.position, Vector3.forward, rotationAroundMainBall);

            return newSubBall;
        }
    }
}
