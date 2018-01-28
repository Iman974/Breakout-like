using UnityEngine;
using System.Collections;

public class AnimatedReveal : MonoBehaviour {

    [System.Serializable]
    private class SlideAnimation {
        public AnimationCurve xSlide = AnimationCurve.Constant(0f, 1f, 0f);
        public AnimationCurve ySlide = AnimationCurve.Constant(0f, 1f, 0f);

        public float MaxTime {
            get {
                float xSlideDuration = xSlide.keys[xSlide.length - 1].time;
                float ySlideDuration = ySlide.keys[ySlide.length - 1].time;

                return xSlideDuration > ySlideDuration ? xSlideDuration : ySlideDuration;
            }
        }
    }

    [SerializeField] private SlideAnimation slideAnimation;
    [SerializeField] private AnimationCurve rotationAnimation = AnimationCurve.Constant(0f, 1f, 0f);
    [SerializeField] private AnimationCurve scaleAnimation = AnimationCurve.Constant(0f, 1f, 0f);
    //[SerializeField] private float offScreenDistance;
    [SerializeField] private MonoBehaviour[] disabledDuringAnimation;
    [SerializeField] private float maxRandomDelay = 0.25f;
    [SerializeField] private bool randomDelay = true;

    private Vector2 targetedPosition;
    private Quaternion targetedRotation;
    private Vector3 targetedScale;
    private Rigidbody2D rb2D;
    private bool initialRb2DState;
    private float totalTime;
    private const int curvesAmount = 3;
    private static int animatedObjects;

    public static bool IsAnimationRunning { get; private set; }

    private void Awake() {
        IsAnimationRunning = true;
        animatedObjects++;
        rb2D = GetComponent<Rigidbody2D>();
        targetedPosition = transform.position;
        targetedRotation = transform.rotation;
        targetedScale = transform.localScale;

        Keyframe[] lastKeyframes = new Keyframe[curvesAmount - 1];
        lastKeyframes[0] = rotationAnimation.keys[rotationAnimation.length - 1];
        lastKeyframes[1] = scaleAnimation.keys[scaleAnimation.length - 1];

        totalTime = slideAnimation.MaxTime;
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
            transform.position = targetedPosition + new Vector2(slideAnimation.xSlide.Evaluate(time), slideAnimation.ySlide.Evaluate(time));
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
            IsAnimationRunning = false;
        }
    }
}
