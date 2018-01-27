using UnityEngine;
using System.Collections;

public class AnimatedAppear : MonoBehaviour {

    [SerializeField] private AnimationCurve slideAnimation;
    [SerializeField] private AnimationCurve rotationAnimation;
    [SerializeField] private AnimationCurve scaleAnimation;
    //[SerializeField] private float offScreenDistance;
    [SerializeField] private MonoBehaviour[] disabledDuringAnimation;
    [SerializeField] private float maxRandomDelay = 0.25f;
    [SerializeField] private bool randomDelay = true;
    //[SerializeField] private Ball ball;

    private Vector2 targetedPosition;
    private Quaternion targetedRotation;
    private Vector3 targetedScale;
    private Rigidbody2D rb2D;
    private bool initialRb2DState;
    private float totalTime;
    private const int curvesAmount = 3;
    private int animatedObjects = 0;

    public static bool AnimationsOver { get; private set; }

    private void Awake() {
        animatedObjects++;
        rb2D = GetComponent<Rigidbody2D>();
        targetedPosition = transform.position;
        targetedRotation = transform.rotation;
        targetedScale = transform.localScale;

        Keyframe[] lastKeyframes = new Keyframe[curvesAmount];
        lastKeyframes[0] = slideAnimation.keys[slideAnimation.length - 1];
        lastKeyframes[1] = rotationAnimation.keys[rotationAnimation.length - 1];
        lastKeyframes[2] = scaleAnimation.keys[scaleAnimation.length - 1];

        totalTime = lastKeyframes[0].time;
        foreach (Keyframe keyframe in lastKeyframes) {
            if (keyframe.time > totalTime) {
                totalTime = keyframe.time;
            }
        }

        if (rb2D != null) {
            initialRb2DState = rb2D.isKinematic;
            rb2D.isKinematic = true;
        }
        foreach (var behaviour in disabledDuringAnimation) {
            behaviour.enabled = false;
        }
    }

    private void Start() {
        //transform.position = (Vector2)transform.position + new Vector2(transform.position.x, offScreenDistance);
        Invoke("StartAnimations", 0.5f); // Only for debug purposes
    }

    private void StartAnimations() {
        StartCoroutine(SlideScreen());
        StartCoroutine(ScaleAndRotate());
    }
	
    private IEnumerator SlideScreen() {
        if (randomDelay) {
            yield return new WaitForSeconds(Random.Range(0f, maxRandomDelay));
        }
        for (float time = 0f; time < totalTime; time += Time.deltaTime) {
            transform.position = targetedPosition + new Vector2(0f, slideAnimation.Evaluate(time));
            yield return null;
        }
        transform.position = targetedPosition;
    }

    private IEnumerator ScaleAndRotate() {
        for (float time = 0f; time < totalTime; time += Time.deltaTime) {
            transform.rotation =  targetedRotation * Quaternion.Euler(new Vector3(0f, 0f, rotationAnimation.Evaluate(time)));

            float evaluatedScale = scaleAnimation.Evaluate(time);
            transform.localScale = targetedScale + new Vector3(evaluatedScale, evaluatedScale, 0f);
            yield return null;
        }
        transform.rotation = targetedRotation;

        foreach (var behaviour in disabledDuringAnimation) {
            behaviour.enabled = true;
        }
        if (rb2D != null) {
            rb2D.isKinematic = initialRb2DState;
        }
        animatedObjects--;
        if (animatedObjects == 0) {
            AnimationsOver = true;
        }
    }
}
