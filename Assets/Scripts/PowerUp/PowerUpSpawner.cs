using UnityEngine;
using System.Collections;

public class PowerUpSpawner : MonoBehaviour {

    [SerializeField] private GameObject[] powerUps;
    //[SerializeField] private GameObject powerUpObject;
    [SerializeField] private int powerUpSpawnRate = 2;
    [SerializeField] private float powerUpSpawnTimeRange = 3f;
    [SerializeField] private Vector2 minSpawnArea, maxSpawnArea;
    //[SerializeField] private float deadZoneRadius = 1.5f; // This is used to prevent powerUp from spawning too close to each other
    [SerializeField] private LayerMask powerUpLayer;
    [SerializeField] private float spacingBetweenRays = 0.1f;

    private Vector2 randomSpawnLocation;
    private Vector2[] spawnedPowerupsLocations;
    //private int numberOfRayChecks;
    //private Vector2 ballDirection, ballDirectionNormal, ballPosition, raySpacing;
    //private float ballRadius;

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

    private void OnEnable() {
        StartCoroutine(WaitForLaunch());
    }

    private IEnumerator WaitForLaunch() {
        while (GameManager.Instance.GameState == GameManager.State.LAUNCH) {
            yield return null;
        }
        StartSpawning();
    }

    public void StartSpawning() {
        if (!IsInvoking("SpawnPowerUp")) {
            InvokeRepeating("SpawnPowerUp", powerUpSpawnTimeRange, powerUpSpawnTimeRange);
        }
    }

    public void StopSpawning() {
        CancelInvoke();
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

            int antiLoop = 0;
            while (/*Mathf.Abs(spawnedPowerupsLocations[i - 1].x - randomSpawnLocation.x) < deadZoneRadius ||*/
                CheckIfOnBallPath(randomSpawnLocation)) {

                randomSpawnLocation.x = Random.Range(minSpawnArea.x, maxSpawnArea.x); // Find another way, this is risky
                antiLoop++;
                if (antiLoop > 50) {
                    Debug.LogError(@"too big loop !!!! /!\");
                    break;
                }
            }
            Debug.Log("random spawn iterations: " + antiLoop);

            spawnedPowerupsLocations[i] = randomSpawnLocation;
            Instantiate(powerUps[Random.Range(0, powerUps.Length)], randomSpawnLocation, Quaternion.identity);
        }
    }

    private Vector2 GenerateRandomPosition() {
        return new Vector2(Random.Range(minSpawnArea.x, maxSpawnArea.x), Random.Range(minSpawnArea.y, maxSpawnArea.y));
    }

    private bool CheckIfOnBallPath(Vector2 positionToCheck) {
        //ballRadius = Ball.MainBall.Radius;
        //ballDirectionNormal = new Vector2(ballDirection.y, -ballDirection.x) * -ballRadius;
        //numberOfRayChecks = (int)(((ballRadius * 2) / spacingBetweenRays) / ballRadius);
        //raySpacing = ballDirectionNormal * spacingBetweenRays;

        //Debug.DrawLine(Ball.MainBall.transform.position, positionToCheck, Color.blue, 2f);
        //Debug.DrawRay(Ball.MainBall.transform.position, Ball.MainBall.Direction, Color.red, 2f);
        //Debug.DrawRay(positionToCheck, Vector2.down, Color.cyan, 3f);
        //Debug.Log(Mathf.Rad2Deg * Mathf.Acos(Vector2.Dot(Ball.MainBall.Direction, (positionToCheck - (Vector2)Ball.MainBall.transform.position).normalized)));
        return Vector2.Dot(Ball.MainBall.Direction, (positionToCheck -
            (Vector2)Ball.MainBall.transform.position).normalized) > 0.95f ? true : false;

        //Debug.DrawRay(ballPosition, ballDirectionNormal, Color.red, 3f);
        //for (int i = 0; i < 11; i++) { // Find out how to calculate the number of rays needed relatively to the ball radius
        //    //Debug.DrawRay(ballPosition + (i * raySpacing), ballDirection, Color.cyan, 3f);
        //    if (Physics2D.Raycast(ballPosition + (raySpacing * i), ballDirection, 15f, powerUpLayer).collider != null) {
        //        Debug.Log("Ontrajectory");
        //        return true;
        //    }
        //}
        //return false;
    }
}
