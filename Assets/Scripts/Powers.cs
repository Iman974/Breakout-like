using UnityEngine;
using System.Collections;

public class Powers : MonoBehaviour {

    [SerializeField] private float speedUpMultiplier;

    public static Powers Instance;

    private float invertedSpeedMultiplier;

    private void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }
        #endregion
        invertedSpeedMultiplier = 1f / speedUpMultiplier;
    }

    private IEnumerator SpeedUp(float powerUpDuration) {
        Ball.MainBall.DoSpeedUpOverTime = false;
        Ball.MainBall.Speed *= speedUpMultiplier;

        yield return new WaitForSeconds(powerUpDuration);

        Ball.MainBall.Speed *= invertedSpeedMultiplier;
        Ball.MainBall.DoSpeedUpOverTime = true;
    }
}
