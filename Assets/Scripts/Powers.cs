using UnityEngine;
using System.Collections;

public class Powers : MonoBehaviour {

    [SerializeField] private float speedUpMultiplier;

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
}
