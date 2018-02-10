using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New slide animation", menuName = "Custom Animations/Slide", order = 0)]
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
            if (attachedCustomAnimation.debug) { Debug.Log(nextPosition); }
        }
        attachedCustomAnimation.transform.position = targetedPosition;
    }
}
