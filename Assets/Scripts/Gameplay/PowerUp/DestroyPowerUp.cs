using System.Collections;
using UnityEngine;

public class DestroyPowerUp : PowerUp {

    protected override void PowerUpPayload() {
        StartCoroutine(SetBricksAsDestroyable());
    }

    private IEnumerator SetBricksAsDestroyable() {
        foreach (Collider2D collider in Brick.BrickColliders) {
            collider.isTrigger = true;
        }

        yield return new WaitForSeconds(powerUpDuration);

        PowerUpHasExpired();
    }

    protected override void PowerUpHasExpired() {
        foreach (Collider2D collider in Brick.BrickColliders) {
            collider.isTrigger = false;
        }

        base.PowerUpHasExpired();
    }
}
