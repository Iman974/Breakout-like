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
        Ball.Main.DoSpeedUpOverTime = false;
        speedBeforePowerUp = Ball.Main.Speed;
        Ball.Main.Speed *= speedUpMultiplier;

        yield return new WaitForSeconds(powerUpDuration);
        PowerUpHasExpired();
    }

    protected override void PowerUpHasExpired() {
        Ball.Main.Speed = speedBeforePowerUp;
        Ball.Main.DoSpeedUpOverTime = true;
        base.PowerUpHasExpired();
    }
}
