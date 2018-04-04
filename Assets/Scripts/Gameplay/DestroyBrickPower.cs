using UnityEngine;
using System.Collections;

//[CreateAssetMenu(fileName = "New Pattern Power", menuName = "Brick Power/Destroy Pattern Power")]
public class DestroyBrickPower : BrickPower {

    [SerializeField] private DestructionPattern destroyPattern;
    public DestructionPattern DestroyPattern {
        set {
            destroyPattern = value;
        }
    }

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

    public override void TriggerPower() {
        for (int i = 0; i < brickVectors.Length; i++) {
            //Debug.DrawRay(transform.position, brickVectors[i].Direction, Color.red, 4f);
            Physics2D.RaycastNonAlloc(transform.position, brickVectors[i].Direction, rayHits2D[i],
                brickVectors[i].Length, 1 << brickLayer);
        }
        GameManager.Instance.StartCoroutine(DestroyBricks());
    }

    private IEnumerator DestroyBricks() {
        foreach (RaycastHit2D[] rayHits2DArray in rayHits2D) {
            foreach (RaycastHit2D hit in rayHits2DArray) {
                if (hit) {
                    //IEnumerator destroyBrick = null; //= Brick.Bricks.Find(brick => brick.gameObject == hit.collider.gameObject).DestroyBrick();
                    //foreach (Brick brick in Brick.Bricks) {
                    //    if (brick == null) {
                    //        Debug.LogError("Brick is null");
                    //    }

                    //    Debug.Log("Brick: " + brick.gameObject.name + ", hit: " + hit.collider.gameObject.name);

                    //    if (brick.gameObject == hit.collider.gameObject) {
                    //        destroyBrick = brick.DestroyBrick();
                    //        if (destroyBrick == null) Debug.LogError("DestroyBrick IEnumerator is null");
                    //    }
                    //}
                    // The list search is to avoid the getComponent<Brick>() on the hit.collider.gameObject
                    // Research impact performance of GetComponent
                    hit.collider.GetComponent<Brick>().RemoveBrick(); // Error due to how fast we launch a level ??
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
