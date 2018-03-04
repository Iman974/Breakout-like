using UnityEngine;
using System.Collections;

//[CreateAssetMenu(fileName = "New Pattern Power", menuName = "Brick Power/Destroy Pattern Power")]
public class DestroyBrickPower : BrickPower {

    [SerializeField] private DestroyPattern destroyPattern;

    private int brickLayer;
    private DestroyPattern.BrickVector[] brickVectors; // Cache the pattern

    private void Awake() {
        if (destroyPattern == null) {
            throw new System.Exception("Destroy pattern not found");
        }
        brickVectors = destroyPattern.EnabledBrickVectors;
        brickLayer = LayerMask.NameToLayer("Brick");
    }

    public override bool TriggerPower() {
        //gameObject.layer = Physics2D.IgnoreRaycastLayer;
        for (int i = 0; i < brickVectors.Length; i++) {
            Debug.DrawRay(transform.position, brickVectors[i].Direction, Color.red, 4f);
            Physics2D.RaycastNonAlloc(transform.position, brickVectors[i].Direction, brickVectors[i].RayHits2D,
                brickVectors[i].Length, 1 << brickLayer);
        }
        //gameObject.layer = brickLayer;
        GameManager.Instance.StartCoroutine(DestroyBricks());
        return true;
    }

    private IEnumerator DestroyBricks() {
        foreach (var bgVector in brickVectors) {
            foreach (RaycastHit2D hit in bgVector.RayHits2D) {
                if (hit) {
                    GameManager.Instance.StartCoroutine(Brick.Bricks.Find(
                        brick => brick.gameObject == hit.collider.gameObject).DestroyBrick());
                }
                yield return null;
            }
        }
    }
}
