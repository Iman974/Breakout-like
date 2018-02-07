using UnityEngine;

public class DestroyBehaviour : IPowerUp {

    public void ActivatePower() {
        foreach (Collider2D collider in Brick.brickColliders) {
            collider.isTrigger = true;
        }

        /*yield return new WaitForSeconds(powerUpDuration);

        foreach (Collider2D collider in Brick.brickColliders) {
            collider.isTrigger = false;
        }*/
    }
}
