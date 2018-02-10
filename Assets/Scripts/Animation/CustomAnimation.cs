using UnityEngine;
using System.Collections;

public class CustomAnimation : MonoBehaviour {

    //[SerializeField] private SlideAnimation.SlideCurves slideAnimation;
    //[SerializeField] private AnimationCurve rotationAnimation = AnimationCurve.Constant(1f, 1f, 1f);
    //[SerializeField] private AnimationCurve scaleAnimation = AnimationCurve.Constant(1f, 1f, 1f);
    //[SerializeField] private float offScreenDistance;
    //[SerializeField] private Object[] customAnimations;
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
    //private float totalTime;
    private Animatable[] customAnimations;
    private Rigidbody2D rb2D;
    private bool wasRb2DKinematic;
    private static int animatedObjects;

    public static bool IsAnimationRunning { get; private set; }

    private void Awake() {
        // All the anim init was there
        rb2D = GetComponent<Rigidbody2D>();
        //customAnimations = new Animatable[3];
    }

    private void Start() {
        //transform.position = (Vector2)transform.position + new Vector2(transform.position.x, offScreenDistance);
        IsAnimationRunning = true;
        animatedObjects++;

        if (customAnimations[0] != null) {
            Debug.Log(customAnimations[0]);
            customAnimations[0] = customAnimations[0] as RotateAnimation;
            Debug.Log(customAnimations[0]);
        }
        /*linkedAnimations = GetComponents<Animatable>();
        Debug.Log(linkedAnimations.Length);*/

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
            wasRb2DKinematic = rb2D.isKinematic;
            rb2D.isKinematic = true;
        }
        foreach (var behaviour in disabledDuringAnimation) {
            behaviour.enabled = false;
        }

        Invoke("StartAnimations", 0.5f); // Only for debug purposes
    }

    private void StartAnimations() {
        for (int i = 0; i < customAnimations.Length; i++) {
            //customAnimations[i].BindAnimation(this);
            //customAnimations[i].Animate();
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
            rb2D.isKinematic = wasRb2DKinematic;
        }
        animatedObjects--;
        if (animatedObjects == 0) {
            IsAnimationRunning = false;
        }
    }
}
