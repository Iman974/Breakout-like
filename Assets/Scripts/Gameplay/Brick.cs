using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Brick : MonoBehaviour {

    [SerializeField] private int scaleUpOverFrames = 50;
    [SerializeField] private float scaleUpFactor = 1.025f;
    [SerializeField] private int fadeOutOverFrames = 50;
    [SerializeField] private float fadeOutFactor = 0.95f;

    /// <summary>
    /// How many base (i.e. combo value is 1) points will be added to the score.
    /// </summary>
    [SerializeField] private int scoreValue = 7;
    //[SerializeField] private Vector2Int gridPosition;

    private BrickPower power;
    private Collider2D thisCollider;
    private SpriteRenderer thisRenderer;

    /// <summary>
    /// Has the brick finished its remove animation and thus is no longer visible ?
    /// </summary>
    private bool isDisappearOver;

    /// <summary>
    /// A list of every the colliders attached to the bricks in the current level. This avoids using GetComponent.
    /// </summary>
    public static List<Collider2D> BrickColliders { get; private set; }

    /// <summary>
    /// A list of every bricks in the current level. This avoid using GetComponent.
    /// </summary>
    public static List<Brick> Bricks { get; private set; }

    /// <summary>
    /// The score values of every bricks in the current level added together.
    /// </summary>
    public static int TotalScoreValue { get; private set; }

    private void Awake() {
        thisCollider = GetComponent<Collider2D>();
        thisRenderer = GetComponent<SpriteRenderer>();
        power = GetComponent<BrickPower>();

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

    //private void Start() {
    //    //BrickGrid.Instance.SetBrick(this, gridPosition.x, gridPosition.y);
    //}

    private void OnCollisionEnter2D(Collision2D collision) {
        if (power != null) {
            power.TriggerPower();
        }

        RemoveBrick();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (power != null) {
            power.TriggerPower();
        }

        RemoveBrick();
    }
    
    public void RemoveBrick() {
        thisCollider.enabled = false;
        enabled = true;

        BrickColliders.Remove(thisCollider);
        Bricks.Remove(this);
        GameManager.Instance.RemoveBrick(scoreValue);

        StartCoroutine(ScaleUp());
        StartCoroutine(FadeOut());
    }

    private void LateUpdate() {
        if (isDisappearOver) {
            Destroy(gameObject);
        }
    }

    private IEnumerator ScaleUp() {
        for (int i = 0; i < scaleUpOverFrames; ++i) {
            transform.localScale = new Vector2(transform.localScale.x * scaleUpFactor, transform.localScale.y * scaleUpFactor);

            yield return null;
        }
    }

    private IEnumerator FadeOut() {
        for (int i = 0; i < fadeOutOverFrames; ++i) {
            thisRenderer.color *= new Color(1f, 1f, 1f, fadeOutFactor);
            yield return null;
        }

        thisRenderer.enabled = false;
        isDisappearOver = true;
    }

    public static void ResetBricks() {
        BrickColliders.Clear();
        Bricks.Clear();
        TotalScoreValue = 0;
    }
}
