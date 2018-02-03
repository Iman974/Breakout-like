using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Brick : MonoBehaviour {

    [SerializeField] private int scaleUpOverFrames = 50;
    [SerializeField] private float scaleUpFactor = 1.025f;
    [SerializeField] private int fadeOutOverFrames = 50;
    [SerializeField] private float fadeOutFactor = 0.95f;
    [SerializeField] private int scoreValue = 7;

    private Collider2D thisCollider;
    private SpriteRenderer thisRenderer;

    public static List<Collider2D> brickColliders = new List<Collider2D>();

    private void Awake() {
        thisCollider = GetComponent<Collider2D>();
        thisRenderer = GetComponent<SpriteRenderer>();

        brickColliders.Add(thisCollider);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        DestroyBrick();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        DestroyBrick();
    }
    
    private void DestroyBrick() {
        thisCollider.enabled = false;
        StartCoroutine(ScaleUp());
        StartCoroutine(Disappear());
        brickColliders.Remove(thisCollider);
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
