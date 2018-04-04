using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpSpawner : MonoBehaviour {

    [System.Serializable]
    private class ConstrainedRandom {

        [SerializeField] [Range(0f, 1f)] private float factor = 0.3f;
        //public float Factor {
        //    get { return factor; }
        //    set { factor = value; }
        //}

        [SerializeField] [Range(0f, 1f)] private float negativeConstraint = 1f;
        //public float MinConstraint {
        //    get { return minConstraint; }
        //    set { minConstraint = value; }
        //}

        [SerializeField] [Range(0f, 1f)] private float positiveConstraint = 1f;
        //public float MaxConstraint {
        //    get { return maxConstraint; }
        //    set { maxConstraint = value; }
        //}

        public float GetRandomValue(float baseValue) {
            return Random.Range(baseValue * -factor * negativeConstraint, baseValue * factor * positiveConstraint);
        }
    }

#pragma warning disable 0649
    [SerializeField] private GameObject[] powerUpPrefabs;
    //[SerializeField] private GameObject powerUpObject;
    [SerializeField] [Min(0f)] private float spawnRateOverTime = 0.1f;
    [SerializeField] private ConstrainedRandom rateRandomizer;

    [Tooltip("The number of power ups in the level will be limited by this number.")]
    [SerializeField] private int maxPowerUpCount = 3;

    [SerializeField] private Vector2 minSpawnArea, maxSpawnArea;
    [SerializeField] private float deadZoneRadius = 1.25f; // This is used to prevent powerUp from spawning too close to each other

    [Tooltip("This is used to determine whether the ball will reach a position in the world based on its velocity direction.")]
    [SerializeField] [Range(0f, 1f)] private float onBallPathFactor = 0.92f;
    //[SerializeField] private LayerMask powerUpLayer;
    //[SerializeField] private float spacingBetweenRays = 0.1f;

    private Vector2 randomSpawnPosition;
    //private List<Vector2> spawnedPowerupsPos;
    private float waitDuration;
    private bool isSpawning;
    private bool instantiated;
    private List<GameObject> spawnedPowerups;
    private float deadZoneRadiusSquared;

    //private int numberOfRayChecks;
    //private Vector2 ballDirection, ballDirectionNormal, ballPosition, raySpacing;
    //private float ballRadius;

    private void Awake() {
        if (instantiated == true) {
            Destroy(gameObject);
            return;
        }
        instantiated = true;

        waitDuration = 1f / spawnRateOverTime;
        deadZoneRadiusSquared = deadZoneRadius * deadZoneRadius;

        spawnedPowerups = new List<GameObject>(maxPowerUpCount);

        enabled = false;
    }

    public void OnBallLaunch() {
        StartSpawning();
    }

    public void StartSpawning() {
        if (!isSpawning) {
            isSpawning = true;
            StartCoroutine(SpawnPowerUps());
        }
        //if (!IsInvoking("SpawnPowerUp")) {
        //    InvokeRepeating("SpawnPowerUp", powerUpSpawnTimeRange, powerUpSpawnTimeRange);
        //}
    }

    public void StopSpawning() {
        if (!isSpawning) {
            return;
        }

        isSpawning = false;
        spawnedPowerups.Clear();

        StopAllCoroutines();
    }

    public void DestroyPowerUps() {
        GameObject[] currentSpawnedPowerUps = GameObject.FindGameObjectsWithTag("PowerUp");

        foreach (GameObject powerUp in currentSpawnedPowerUps) {
            Destroy(powerUp);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;

        Gizmos.DrawLine(minSpawnArea, new Vector2(maxSpawnArea.x, minSpawnArea.y));
        Gizmos.DrawLine(new Vector2(maxSpawnArea.x, minSpawnArea.y), maxSpawnArea);
        Gizmos.DrawLine(maxSpawnArea, new Vector2(minSpawnArea.x, maxSpawnArea.y));
        Gizmos.DrawLine(new Vector2(minSpawnArea.x, maxSpawnArea.y), minSpawnArea);
    }

    private IEnumerator SpawnPowerUps() {
        while (true) {
            while (spawnedPowerups.Count >= maxPowerUpCount) {
                yield return null;
            }

            randomSpawnPosition = GenerateRandomPosition();
            while (!IsPositionValid(randomSpawnPosition)) {
                randomSpawnPosition = GenerateRandomPosition();
            }

            spawnedPowerups.Add(InstantiateRandomPowerUp());

            float rdmValue = rateRandomizer.GetRandomValue(waitDuration);
            //Debug.Log(rdmValue);
            yield return new WaitForSeconds(waitDuration + rdmValue);
        }
    }

    private void Update() {
        if (isSpawning) {
            for (int i = 0; i < spawnedPowerups.Count; i++) {
                if (spawnedPowerups[i] == null) {
                    spawnedPowerups.RemoveAt(i);
                }
            }
        }
    }

    private GameObject InstantiateRandomPowerUp() {
        return Instantiate(powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)], randomSpawnPosition, Quaternion.identity);
    }

    //private void SpawnPowerUp() {
    //    //spawnedPowerupsPos.Clear();

    //    for (int i = 0; i < spawnRateOverTime; i++) {
    //        randomSpawnPosition = GenerateRandomPosition();

    //        int antiLoop = 0;
    //        while (/*Mathf.Abs(spawnedPowerupsLocations[i - 1].x - randomSpawnLocation.x) < deadZoneRadius ||*/
    //            IsPositionValid(randomSpawnPosition) || (i > 0 ? CheckDeadZone() : false)) {

    //            randomSpawnPosition.x = Random.Range(minSpawnArea.x, maxSpawnArea.x); // Find another way, this is risky
    //            antiLoop++;
    //            if (antiLoop > 50) {
    //                Debug.LogError(@"too big loop !!!! /!\");
    //                break;
    //            }
    //        }
    //        //Debug.Log("random spawn iterations: " + antiLoop);

    //        //spawnedPowerupsPos.Add(randomSpawnLocation);
    //        Instantiate(powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)], randomSpawnPosition, Quaternion.identity);
    //    }
    //}

    private Vector2 GenerateRandomPosition() {
        return new Vector2(Random.Range(minSpawnArea.x, maxSpawnArea.x), Random.Range(minSpawnArea.y, maxSpawnArea.y));
    }

    /// <summary>
    /// Checks if the position is not on the ball path, based on the onBallPathFactor, and is not too close to the ball,
    /// based on the deadZoneRadius.
    /// </summary>
    private bool IsPositionValid(Vector2 positionToCheck) {
        bool isOnBallPath = Vector2.Dot(Ball.Main.Direction, (positionToCheck - Ball.Main.Position).normalized) > onBallPathFactor;

        bool isCloseToBall = (positionToCheck - Ball.Main.Position).sqrMagnitude < deadZoneRadiusSquared;

        return !isOnBallPath && !isCloseToBall;
    }

    //private bool CheckDeadZone() {
    //    float deadZoneRadiusSquared = deadZoneRadius * deadZoneRadius;

    //for (int i = 0; i < spawnedPowerupsPos.Count; i++) {
    //    if (Mathf.Abs(randomSpawnLocation.x - spawnedPowerupsPos[i].x) < deadZoneRadius ||
    //        (randomSpawnLocation - (Vector2)Ball.Main.transform.position).sqrMagnitude < deadZoneRadiusSquared) {
    //        return true;
    //    }
    //}

    //    return false;
    //}
}
