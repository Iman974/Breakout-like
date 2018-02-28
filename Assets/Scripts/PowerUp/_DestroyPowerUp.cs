using System.Collections;
using UnityEngine;

public class _DestroyPowerUp : _PowerUp {

    protected override void PowerUpPayload() {
        GameManager.Instance.StartCoroutine(SetBricksAsDestroyable());
    }

    private IEnumerator SetBricksAsDestroyable() {
        foreach (Collider2D collider in Brick.brickColliders) {
            collider.isTrigger = true;
        }

        yield return new WaitForSeconds(powerUpDuration);

        foreach (Collider2D collider in Brick.brickColliders) {
            collider.isTrigger = false;
        }
    }
}
