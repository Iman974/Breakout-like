using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {

    [SerializeField] private int scaleUpOverFrames = 50;
    [SerializeField] private float scaleUpFactor = 1.025f;
    [SerializeField] private int fadeOutOverFrames = 50;
    [SerializeField] private float fadeOutFactor = 0.95f;
    [SerializeField] private int scoreValue = 10;

    private Collider2D thisCollider;
    private SpriteRenderer thisRenderer;

    private void Awake() {
        thisCollider = GetComponent<Collider2D>();
        thisRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        thisCollider.enabled = false;
        StartCoroutine(ScaleUp());
        StartCoroutine(Disappear());
        GameManager.Instance.RemoveBrick(scoreValue);
    }

    private IEnumerator ScaleUp() {
        for (int i = 0; i < scaleUpOverFrames; ++i) {
            transform.localScale *= scaleUpFactor;

            yield return null;
        }
    }

    private IEnumerator Disappear() {
        for (int i = 0; i < fadeOutOverFrames; ++i) {
            thisRenderer.color *= new Color(1f, 1f, 1f, fadeOutFactor);
            yield return null;
        }
        Destroy(gameObject);
    }
}
