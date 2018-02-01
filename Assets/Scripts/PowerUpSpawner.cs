using UnityEngine;

public class PowerUpSpawner : MonoBehaviour {

    [SerializeField] private PowerUp[] powerUps;
    [SerializeField] private GameObject powerUpObject;
    [SerializeField] private int powerUpSpawnRate = 2;
    [SerializeField] private float powerUpSpawnTimeRange = 3f;
    [SerializeField] private Vector2 minSpawnArea, maxSpawnArea;

    private void Start() {
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
        PowerUpInGame newPowerUp = Instantiate(powerUpObject, new Vector2(Random.Range(minSpawnArea.x, maxSpawnArea.x),
            Random.Range(minSpawnArea.y, maxSpawnArea.y)), Quaternion.identity).GetComponent<PowerUpInGame>();

        newPowerUp.powerUp = powerUps[Random.Range(0, powerUps.Length)];
    }
}
