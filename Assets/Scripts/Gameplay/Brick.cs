using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Brick : MonoBehaviour {

    [SerializeField] private int scaleUpOverFrames = 50;
    [SerializeField] private float scaleUpFactor = 1.025f;
    [SerializeField] private int fadeOutOverFrames = 50;
    [SerializeField] private float fadeOutFactor = 0.95f;
    [SerializeField] private int scoreValue = 7;
    //[SerializeField] private Vector2Int gridPosition;

    private BrickPower power;
    private Collider2D thisCollider;
    private SpriteRenderer thisRenderer;
    private bool isDisappearOver, destroyOnTriggerPower = true;

    public static List<Collider2D> BrickColliders { get; private set; }
    public static List<Brick> Bricks { get; private set; }
    public static int TotalScoreValue { get; private set; }

    private void Awake() {
        thisCollider = GetComponent<Collider2D>();
        thisRenderer = GetComponent<SpriteRenderer>();

        if (BrickColliders == null) {
            BrickColliders = new List<Collider2D>();
        }
        if (Bricks == null) {
            Bricks = new List<Brick>();
        }

        TotalScoreValue += scoreValue;
        BrickColliders.Add(thisCollider);
        Bricks.Add(this);
    }

    private void Start() {
        power = GetComponent<BrickPower>();
        //BrickGrid.Instance.SetBrick(this, gridPosition.x, gridPosition.y);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (power != null) {
            destroyOnTriggerPower = power.TriggerPower();
        }
        if (destroyOnTriggerPower) {
            StartCoroutine(DestroyBrick());
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (power != null) {
            power.TriggerPower();
        }
        if (destroyOnTriggerPower) {
            StartCoroutine(DestroyBrick()); 
        }
    }
    
    public IEnumerator DestroyBrick() {
        thisCollider.enabled = false;
        StartCoroutine(ScaleUp());
        StartCoroutine(Disappear());
        BrickColliders.Remove(thisCollider);
        Bricks.Remove(this);
        GameManager.Instance.RemoveBrick(scoreValue);
        while (!isDisappearOver) {
            yield return null;
        }
        Destroy(gameObject);
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
        thisRenderer.enabled = false;
        isDisappearOver = true;
    }
}
