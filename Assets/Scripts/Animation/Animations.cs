using UnityEngine;
using System.Collections;

public class Animations : MonoBehaviour {

    // In here the set of classes that implements IAnimatable that represents algorithms

    // These are the default animation : but is this really necessary ?
    /*SlideAnimation slideAnimation;
    RotateAnimation rotateAnimation;
    ScaleAnimation scaleAnimation;*/

    public class SlideAnimation : IAnimatable {

        [System.Serializable]
        public class SlideCurves {
            public AnimationCurve xSlide = AnimationCurve.Constant(1f, 1f, 1f);
            public AnimationCurve ySlide = AnimationCurve.Constant(1f, 1f, 1f);

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

        /*public SlideAnimation() {
            totalTime = slideAnimation.MaxTime;
        }*/

        public SlideAnimation(SlideCurves curves, Vector2 relativeStartPos, Vector2 targetPos, AnimatedReveal animation) {
            slideCurves = curves;
            relativeStartPosition = relativeStartPos;
            totalTime = slideCurves.MaxTime;
            targetedPosition = targetPos;
            attachedAnimation = animation;
        }

        public override void Animate() {
            attachedAnimation.StartCoroutine(Slide());
        }

        private IEnumerator Slide() {
            float startAnimationPosX = targetedPosition.x + relativeStartPosition.x;
            float startAnimationPosY = targetedPosition.y + relativeStartPosition.y;

            for (float timeToEval = 0f; timeToEval < totalTime; timeToEval += animationSpeed * Time.deltaTime) {
                float evaluatedPosX = slideCurves.xSlide.Evaluate(timeToEval);
                float evaluatedPosY = slideCurves.ySlide.Evaluate(timeToEval);

                if (evaluatedPosX > 0 || evaluatedPosY > 0) { // Separate these ?
                    attachedAnimation.transform.position = new Vector2(Mathf.LerpUnclamped(startAnimationPosX, targetedPosition.x,
                        evaluatedPosX), Mathf.LerpUnclamped(startAnimationPosY, targetedPosition.y, evaluatedPosY));
                }
                yield return null;
            }
            attachedAnimation.transform.position = targetedPosition;
        }
    }

    public class RotateAnimation : IAnimatable {

        [SerializeField] private AnimationCurve rotationCurve;
        [SerializeField] private float relativeStartRotation;

        private Quaternion targetedRotation;

        public RotateAnimation(AnimationCurve curve, float relativeStartRot, Quaternion targetRot, AnimatedReveal animation) {
            rotationCurve = curve;
            relativeStartRotation = relativeStartRot;
            targetedRotation = targetRot;
            attachedAnimation = animation;
        }

        public override void Animate() {
            attachedAnimation.StartCoroutine(Rotate());
        }

        private IEnumerator Rotate() {
            Quaternion startAnimationRotation = Quaternion.Euler(new Vector3(0f, 0f, relativeStartRotation));
            for (float timeToEval = 0f; timeToEval < totalTime; timeToEval += animationSpeed * Time.deltaTime) {
                float evaluatedRotation = rotationCurve.Evaluate(timeToEval);

                if (evaluatedRotation > 0) {
                    attachedAnimation.transform.rotation = Quaternion.LerpUnclamped(startAnimationRotation, targetedRotation,
                        evaluatedRotation);
                }
                yield return null;
            }
            attachedAnimation.transform.rotation = targetedRotation;
        }
    }

    public class ScaleAnimation : IAnimatable {

        [SerializeField] private AnimationCurve scaleAnimation = AnimationCurve.Constant(1f, 1f, 1f);
        [SerializeField] private float relativeStartScale;

        private Vector3 targetedScale;

        public override void Animate() {
            attachedAnimation.StartCoroutine(Scale());
        }

        private IEnumerator Scale() {
            Vector3 startAnimationScale = new Vector3(relativeStartScale, relativeStartScale, 1f);

            for (float timeToEval = 0f; timeToEval < totalTime; timeToEval += animationSpeed * Time.deltaTime) {
                float evaluatedScale = scaleAnimation.Evaluate(timeToEval);

                if (evaluatedScale > 0) {
                    attachedAnimation.transform.localScale = Vector3.LerpUnclamped(startAnimationScale, targetedScale, evaluatedScale);
                }
                yield return null;
            }
            attachedAnimation.transform.localScale = targetedScale;
        }
    }
}
