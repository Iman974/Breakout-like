using UnityEngine;
using System.Collections;

//[CreateAssetMenu(fileName = "New Pattern Power", menuName = "Brick Power/Destroy Pattern Power")]
public class DestroyBrickPower : BrickPower {

    [SerializeField] private DestroyPattern destroyPattern;

    private int brickLayer;
    private BrickVector[] brickVectors; // Cache the pattern
    private RaycastHit2D[][] rayHits2D;

    private void Awake() {
        if (destroyPattern == null) {
            throw new System.Exception("Destroy pattern not found");
        }
        brickVectors = destroyPattern.GetEnabledBrickVectors();
        brickLayer = LayerMask.NameToLayer("Brick");
        rayHits2D = new RaycastHit2D[brickVectors.Length][];

        for (int i = 0; i < rayHits2D.Length; i++) {
            rayHits2D[i] = new RaycastHit2D[brickVectors[i].Count];
        }
    }

    public override bool TriggerPower() {
        for (int i = 0; i < brickVectors.Length; i++) {
            //Debug.DrawRay(transform.position, brickVectors[i].Direction, Color.red, 4f);
            Physics2D.RaycastNonAlloc(transform.position, brickVectors[i].Direction, rayHits2D[i],
                brickVectors[i].Length, 1 << brickLayer);
        }
        GameManager.Instance.StartCoroutine(DestroyBricks());
        return true;
    }

    private IEnumerator DestroyBricks() {
        foreach (var rayHits2DArray in rayHits2D) {
            foreach (RaycastHit2D hit in rayHits2DArray) {
                if (hit) {
                    GameManager.Instance.StartCoroutine(Brick.Bricks.Find(
                        brick => brick.gameObject == hit.collider.gameObject).DestroyBrick());
                }
                yield return null;
            }
        }
    }

    private void OnDrawGizmosSelected() {
        if (destroyPattern == null) {
            return;
        }
        Gizmos.color = Color.magenta;
        BrickVector[] enabledBrickVectors = destroyPattern.GetEnabledBrickVectors();
        foreach (BrickVector brickVector in enabledBrickVectors) {
            Gizmos.DrawRay(transform.position, brickVector.Direction);
        }
    }
}
