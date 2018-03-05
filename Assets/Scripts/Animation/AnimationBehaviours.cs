using UnityEngine;
using System.Collections;

//[CreateAssetMenu(fileName = "New scale animation", menuName = "Custom Animations/Scale", order = 2)]
public class AnimationBehaviours : MonoBehaviour {

    public static AnimationBehaviours Instance { get; private set; }

    public SlideAnimation slideAnimation;
    public RotateAnimation rotateAnimation;
    public ScaleAnimation scaleAnimation;

    private void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }
        #endregion
    }
}

public class SlideAnimation : Animatable {

    [System.Serializable]
    public class SlideCurves {
        public AnimationCurve xSlide = AnimationCurve.Constant(0f, 1f, 1f);
        public AnimationCurve ySlide = AnimationCurve.Constant(0f, 1f, 1f);

        public float MaxTime {
            get {
                float xSlideDuration = xSlide[xSlide.length - 1].time;
                float ySlideDuration = ySlide[ySlide.length - 1].time;

                return xSlideDuration > ySlideDuration ? xSlideDuration : ySlideDuration;
            }
        }
    }

    [SerializeField] private SlideCurves slideCurves;
    [SerializeField] private Vector2 relativeStartPosition;

    private Vector2 targetedPosition;
    private Vector2 nextPosition;

    public SlideAnimation(SlideAnimation toCopy) {

    }

    public override void Animate() {
        targetedPosition = attachedCustomAnimation.transform.position;
        attachedCustomAnimation.transform.position += (Vector3)relativeStartPosition;
        totalTime = slideCurves.MaxTime;

        attachedCustomAnimation.StartCoroutine(Slide());
    }

    private IEnumerator Slide() {
        float startAnimationPosX = targetedPosition.x + relativeStartPosition.x;
        float startAnimationPosY = targetedPosition.y + relativeStartPosition.y;

        for (float timeToEval = 0f; timeToEval < totalTime; timeToEval += animationSpeed * Time.deltaTime) {
            float evaluatedPosX = slideCurves.xSlide.Evaluate(timeToEval);
            float evaluatedPosY = slideCurves.ySlide.Evaluate(timeToEval);

            if (evaluatedPosX > 0) {
                nextPosition.x = Mathf.LerpUnclamped(startAnimationPosX, targetedPosition.x, evaluatedPosX);
            }
            if (evaluatedPosY > 0) {
                nextPosition.y = Mathf.LerpUnclamped(startAnimationPosY, targetedPosition.y, evaluatedPosY);
            }

            attachedCustomAnimation.transform.position = nextPosition;
            /*if (evaluatedPosX > 0 || evaluatedPosY > 0) { // Separate these ?
                attachedCustomAnimation.transform.position = new Vector2(Mathf.LerpUnclamped(startAnimationPosX, targetedPosition.x,
                    evaluatedPosX), Mathf.LerpUnclamped(startAnimationPosY, targetedPosition.y, evaluatedPosY));
            }*/
            yield return null;
        }
        attachedCustomAnimation.transform.position = targetedPosition;
    }
}

public class RotateAnimation : Animatable {

    [SerializeField] private AnimationCurve rotationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private float relativeStartRotation;

    private Quaternion targetedRotation;

    public override void Animate() {
        targetedRotation = attachedCustomAnimation.transform.rotation;
        attachedCustomAnimation.transform.rotation *= Quaternion.Euler(new Vector3(0f, 0f, relativeStartRotation));
        totalTime = rotationCurve[rotationCurve.length - 1].time;

        attachedCustomAnimation.StartCoroutine(Rotate());
    }

    private IEnumerator Rotate() {
        Quaternion startAnimationRotation = Quaternion.Euler(new Vector3(0f, 0f, relativeStartRotation));
        for (float timeToEval = 0f; timeToEval < totalTime; timeToEval += animationSpeed * Time.deltaTime) {
            float evaluatedRotation = rotationCurve.Evaluate(timeToEval);

            if (evaluatedRotation > 0) {
                attachedCustomAnimation.transform.rotation = Quaternion.LerpUnclamped(startAnimationRotation, targetedRotation,
                    evaluatedRotation);
            }
            yield return null;
        }
        attachedCustomAnimation.transform.rotation = targetedRotation;
    }
}

public class ScaleAnimation : Animatable {

    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private float relativeStartScale;

    private Vector3 targetedScale;

    public override void Animate() {
        targetedScale = attachedCustomAnimation.transform.localScale;
        attachedCustomAnimation.transform.localScale += new Vector3(relativeStartScale, relativeStartScale, 0f);
        totalTime = scaleCurve[scaleCurve.length - 1].time;

        attachedCustomAnimation.StartCoroutine(Scale());
    }

    private IEnumerator Scale() {
        Vector3 startAnimationScale = new Vector3(relativeStartScale, relativeStartScale, 1f);

        for (float timeToEval = 0f; timeToEval < totalTime; timeToEval += animationSpeed * Time.deltaTime) {
            float evaluatedScale = scaleCurve.Evaluate(timeToEval);

            if (evaluatedScale > 0) {
                attachedCustomAnimation.transform.localScale = Vector3.LerpUnclamped(startAnimationScale, targetedScale, evaluatedScale);
            }
            yield return null;
        }
        attachedCustomAnimation.transform.localScale = targetedScale;
    }
}
