using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CustomAnimation))]
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

    private void Start() {
        targetedPosition = transform.position;
    }

    public override void Animate() {
        transform.position += (Vector3)relativeStartPosition;
        StartCoroutine(Slide());
    }

    private IEnumerator Slide() {
        float startAnimationPosX = targetedPosition.x + relativeStartPosition.x;
        float startAnimationPosY = targetedPosition.y + relativeStartPosition.y;

        for (float timeToEval = 0f; timeToEval < totalTime; timeToEval += animationSpeed * Time.deltaTime) {
            float evaluatedPosX = slideCurves.xSlide.Evaluate(timeToEval);
            float evaluatedPosY = slideCurves.ySlide.Evaluate(timeToEval);

            if (evaluatedPosX > 0 || evaluatedPosY > 0) { // Separate these ?
                transform.position = new Vector2(Mathf.LerpUnclamped(startAnimationPosX, targetedPosition.x,
                    evaluatedPosX), Mathf.LerpUnclamped(startAnimationPosY, targetedPosition.y, evaluatedPosY));
            }
            yield return null;
        }
        transform.position = targetedPosition;
    }
}
