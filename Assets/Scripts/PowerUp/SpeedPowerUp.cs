using UnityEngine;
using System.Collections;

public class SpeedPowerUp : PowerUp {
    [SerializeField] private float speedUpMultiplier = 1.25f;

    private float speedBeforePowerUp;

    protected override void PowerUpPayload() {
        StartCoroutine(SpeedUp());
        base.PowerUpPayload();
    }

    private IEnumerator SpeedUp() {
        Ball.MainBall.DoSpeedUpOverTime = false;
        speedBeforePowerUp = Ball.MainBall.Speed;
        Ball.MainBall.Speed *= speedUpMultiplier;

        yield return new WaitForSeconds(powerUpDuration);
        PowerUpHasExpired();
    }

    protected override void PowerUpHasExpired() {
        Ball.MainBall.Speed = speedBeforePowerUp;
        Ball.MainBall.DoSpeedUpOverTime = true;
        base.PowerUpHasExpired();
    }
}
