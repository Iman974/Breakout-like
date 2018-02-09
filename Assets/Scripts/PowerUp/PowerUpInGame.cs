﻿using UnityEngine;

public class PowerUpInGame : MonoBehaviour {
    
    public PowerUp powerUp;

    //private float timeSinceInstantiation;
    private Vector2 startPosition;
    private AnimationCurve powerUpXMovement;
    private AnimationCurve powerUpYMovement;
    private Vector2 relativeTargetedPosition;
    private float timeToEval;
    private float movementSpeed;

    private void Start() {
        GetComponent<CircleCollider2D>().radius = powerUp.colliderRadius;
        GetComponent<SpriteRenderer>().sprite = powerUp.powerUpSprite;
        startPosition = transform.position;
        powerUp.InitPower();

        powerUpXMovement = powerUp.horizontalMovement;
        powerUpYMovement = powerUp.verticalMovement;
        relativeTargetedPosition = powerUp.targetedRelativePosition;
        movementSpeed = powerUp.movementSpeed;
    }

    private void FixedUpdate() {
        timeToEval += movementSpeed;
        transform.position = new Vector2(Mathf.Lerp(startPosition.x, relativeTargetedPosition.x, powerUpXMovement.Evaluate(timeToEval)),
            Mathf.Lerp(startPosition.y, relativeTargetedPosition.y, powerUpYMovement.Evaluate(timeToEval)));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Ball")) {
            Destroy(gameObject, 1f);
            return;
        }
        powerUp.TriggerPower();
        // Play pick up anim
        Destroy(gameObject);
    }
}