using UnityEngine;
using System.Collections;

public class PowerUpSpawner : MonoBehaviour {

    [SerializeField] private PowerUp[] powerUps;
    [SerializeField] private GameObject powerUpObject;
    [SerializeField] private int powerUpSpawnRate = 2;
    [SerializeField] private float powerUpSpawnTimeRange = 3f;
    [SerializeField] private Vector2 minSpawnArea, maxSpawnArea;
    [SerializeField] private float deadZoneRadius = 1.5f; // This is used to prevent powerUp from spawning too close to each other
    [SerializeField] private LayerMask powerUpLayer;
    [SerializeField] private float spacingBetweenRays = 0.1f;

    private Vector2 randomSpawnLocation;
    private Vector2[] spawnedPowerupsLocations;
    private int numberOfRayChecks;
    private Vector2 ballDirection, ballDirectionNormal, ballPosition, raySpacing;
    private float ballRadius;

    public static PowerUpSpawner Instance { get; private set; }

    private void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }
        #endregion
        spawnedPowerupsLocations = new Vector2[powerUpSpawnRate];
    }

    private IEnumerator Start() {
        while (GameManager.Instance.GameState == GameManager.State.LAUNCH) {
            yield return null;
        }

        InvokeRepeating("SpawnPowerUp", powerUpSpawnTimeRange, powerUpSpawnTimeRange);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(minSpawnArea, new Vector2(maxSpawnArea.x, minSpawnArea.y));
        Gizmos.DrawLine(new Vector2(maxSpawnArea.x, minSpawnArea.y), maxSpawnArea);
        Gizmos.DrawLine(maxSpawnArea, new Vector2(minSpawnArea.x, maxSpawnArea.y));
        Gizmos.DrawLine(new Vector2(minSpawnArea.x, maxSpawnArea.y), minSpawnArea);
    }

    private void SpawnPowerUp() {
        for (int i = 0; i < powerUpSpawnRate; i++) {
            randomSpawnLocation = GenerateRandomPosition();
            if (i > 0) {
                int antiLoop = 0;
                while (Mathf.Abs(spawnedPowerupsLocations[i - 1].x - randomSpawnLocation.x) < deadZoneRadius || CheckIfOnBallPath()) {
                    randomSpawnLocation.x = Random.Range(minSpawnArea.x, maxSpawnArea.x); // Find another way, this is risky
                    antiLoop++;
                    if (antiLoop > 250) {
                        Debug.LogError(@"infinite loop !!!! /!\");
                        break;
                    }
                }
            }

            spawnedPowerupsLocations[i] = randomSpawnLocation;
            PowerUpInGame newPowerUp = Instantiate(powerUpObject, randomSpawnLocation, Quaternion.identity).GetComponent<PowerUpInGame>();

            newPowerUp.powerUp = powerUps[Random.Range(0, powerUps.Length)];
        }
    }

    private Vector2 GenerateRandomPosition() {
        return new Vector2(Random.Range(minSpawnArea.x, maxSpawnArea.x), Random.Range(minSpawnArea.y, maxSpawnArea.y));
    }

    private bool CheckIfOnBallPath() {
        ballDirection = Ball.MainBall.Direction;
        ballRadius = Ball.MainBall.Radius;
        ballDirectionNormal = new Vector2(ballDirection.y, -ballDirection.x) * -ballRadius;
        ballPosition = (Vector2)Ball.MainBall.transform.position - ballDirectionNormal;
        //numberOfRayChecks = (int)(((ballRadius * 2) / spacingBetweenRays) / ballRadius);
        raySpacing = ballDirectionNormal * spacingBetweenRays;

        //Debug.DrawRay(ballPosition, ballDirectionNormal, Color.red, 3f);
        for (int i = 0; i < 11; i++) { // Find out how to calculate the number of rays needed relatively to the ball radius
            //Debug.DrawRay(ballPosition + (i * raySpacing), ballDirection, Color.cyan, 3f);
            if (Physics2D.Raycast(ballPosition + (raySpacing * i), ballDirection, 15f, powerUpLayer).collider != null) {
                Debug.Log("Ontrajectory");
                return true;
            }
        }
        return false;
    }
}
