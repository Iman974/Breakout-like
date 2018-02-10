using UnityEngine;
using System.Collections;

public class PowerUpSpawner : MonoBehaviour {

    [SerializeField] private PowerUp[] powerUps;
    [SerializeField] private GameObject powerUpObject;
    [SerializeField] private int powerUpSpawnRate = 2;
    [SerializeField] private float powerUpSpawnTimeRange = 3f;
    [SerializeField] private Vector2 minSpawnArea, maxSpawnArea;
    [SerializeField] private float deadZoneRadius = 1.5f; // This is used to prevent powerUp from spawning too close to each other

    private Vector2 randomSpawnLocation;
    private Vector2[] spawnedPowerupsLocations;

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
                while (Mathf.Abs(spawnedPowerupsLocations[i - 1].x - randomSpawnLocation.x) < deadZoneRadius) {
                    randomSpawnLocation.x = Random.Range(minSpawnArea.x, maxSpawnArea.x);
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
}
