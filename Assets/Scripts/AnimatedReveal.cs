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
    [SerializeField] private Behaviour[] disabledDuringAnimation;
    [SerializeField] private float maxRandomDelay = 0.25f;
    [SerializeField] private bool randomDelay = true;
    [SerializeField] private Vector2 startPositionOffset;
    [SerializeField] private float startRotationOffset;
    [SerializeField] private float startScaleOffset;

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
        // All the anim init was there
    }

    private void Start() {
        //transform.position = (Vector2)transform.position + new Vector2(transform.position.x, offScreenDistance);
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

        float startAnimationPosX = targetedPosition.x + startPositionOffset.x;
        float startAnimationPosY = targetedPosition.y + startPositionOffset.y;

        for (float time = 0f; time < totalTime; time += Time.deltaTime) {
            transform.position = new Vector2(Mathf.Lerp(startAnimationPosX, targetedPosition.x, slideAnimation.xSlide.Evaluate(time)),
                Mathf.Lerp(startAnimationPosY, targetedPosition.y, slideAnimation.ySlide.Evaluate(time)));
            yield return null;
        }
    }

    private IEnumerator ScaleAndRotate() {
        Quaternion startAnimationRotation = Quaternion.Euler(new Vector3(0f, 0f, startRotationOffset));
        Vector3 startAnimationScale = new Vector3(0f, 0f, startScaleOffset);

        for (float time = 0f; time < totalTime; time += Time.deltaTime) {
            transform.rotation = Quaternion.Lerp(startAnimationRotation, targetedRotation, rotationAnimation.Evaluate(time));

            transform.localScale = Vector3.Lerp(startAnimationScale, targetedScale, scaleAnimation.Evaluate(time));
            yield return null;
        }

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
