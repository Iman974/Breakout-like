using UnityEngine;
using System.Collections;

public class CustomAnimation : MonoBehaviour {

    //[SerializeField] private SlideAnimation.SlideCurves slideAnimation;
    //[SerializeField] private AnimationCurve rotationAnimation = AnimationCurve.Constant(1f, 1f, 1f);
    //[SerializeField] private AnimationCurve scaleAnimation = AnimationCurve.Constant(1f, 1f, 1f);
    //[SerializeField] private float offScreenDistance;
    [SerializeField] private Behaviour[] disabledDuringAnimation;
    [SerializeField] private bool randomDelay = true;
    [SerializeField] private bool fixedDelay;
    [SerializeField] private float maxRandomDelay = 0.25f;
    //[SerializeField] private Vector2 relativeStartPosition;
    //[SerializeField] private float relativeStartRotation;
    //[SerializeField] private float relativeStartScale;
    //[SerializeField] private float animationSpeed = 1f;

    //private Vector2 targetedPosition;
    //private Quaternion targetedRotation;
    //private Vector3 targetedScale;
    private Rigidbody2D rb2D;
    private bool initialRb2DState;
    //private float totalTime;
    private static int animatedObjects;

    private Animatable[] linkedAnimations;

    public static bool IsAnimationRunning { get; private set; }

    private void Awake() {
        // All the anim init was there
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        //transform.position = (Vector2)transform.position + new Vector2(transform.position.x, offScreenDistance);
        IsAnimationRunning = true;
        animatedObjects++;


        linkedAnimations = GetComponents<Animatable>();
        Debug.Log(linkedAnimations.Length);

        //Keyframe[] lastKeyframes = new Keyframe[curvesAmount - 1];
        //lastKeyframes[0] = rotationAnimation[rotationAnimation.length - 1];
        //lastKeyframes[1] = scaleAnimation[scaleAnimation.length - 1];

        //totalTime = slideAnimation.MaxTime;
        //foreach (Keyframe keyframe in lastKeyframes) {
        //    if (keyframe.time > totalTime) {
        //        totalTime = keyframe.time;
        //    }
        //}

        if (rb2D != null) {
            initialRb2DState = rb2D.isKinematic;
            rb2D.isKinematic = true;
        }
        foreach (var behaviour in disabledDuringAnimation) {
            behaviour.enabled = false;
        }

        Invoke("StartAnimation", 0.5f); // Only for debug purposes
    }

    private void StartAnimation() {
        //StartCoroutine(SlideScaleRotate());
        foreach (var animation in linkedAnimations) {
            animation.Animate();
        }
    }
	
    private IEnumerator SlideScaleRotate() {
        if (randomDelay) {
            yield return new WaitForSeconds(Random.Range(0f, maxRandomDelay));
        }

        //Quaternion startAnimationRotation = Quaternion.Euler(new Vector3(0f, 0f, relativeStartRotation));
        //Vector3 startAnimationScale = new Vector3(relativeStartScale, relativeStartScale, 1f);
        //float startAnimationPosX = targetedPosition.x + relativeStartPosition.x;
        //float startAnimationPosY = targetedPosition.y + relativeStartPosition.y;
        // find a way not to process not wanted animation instead of just setting the animation to a constant 0 ?
        //for (float timeToEval = 0f; timeToEval < totalTime; timeToEval += animationSpeed * Time.deltaTime) {
            //float evaluatedPosX = slideAnimation.xSlide.Evaluate(timeToEval);
            //float evaluatedPosY = slideAnimation.ySlide.Evaluate(timeToEval);
            //float evaluatedRotation = rotationAnimation.Evaluate(timeToEval);
            //float evaluatedScale = scaleAnimation.Evaluate(timeToEval);

            //if (evaluatedPosX > 0 || evaluatedPosY > 0) { // Separate these ?
            //    transform.position = new Vector2(Mathf.LerpUnclamped(startAnimationPosX, targetedPosition.x, evaluatedPosX),
            //        Mathf.LerpUnclamped(startAnimationPosY, targetedPosition.y, evaluatedPosY));
            //}
            //if (evaluatedRotation > 0) {
            //    transform.rotation = Quaternion.LerpUnclamped(startAnimationRotation, targetedRotation, evaluatedRotation);
            //}
            //if (evaluatedScale > 0) {
            //    transform.localScale = Vector3.LerpUnclamped(startAnimationScale, targetedScale, evaluatedScale);
            //}
            //yield return null;
        //}
        //transform.position = targetedPosition;
        //transform.rotation = targetedRotation;
        //transform.localScale = targetedScale;

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
