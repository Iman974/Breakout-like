using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CustomAnimation))]
public class ScaleAnimation : Animatable {

    [SerializeField] private AnimationCurve scaleAnimation = AnimationCurve.Constant(0f, 1f, 1f);
    [SerializeField] private float relativeStartScale;

    private Vector3 targetedScale;

    private void Start() {
        targetedScale = transform.localScale;
    }

    public override void Animate() {
        transform.localScale += new Vector3(relativeStartScale, relativeStartScale, 0f);
        StartCoroutine(Scale());
    }

    private IEnumerator Scale() {
        Vector3 startAnimationScale = new Vector3(relativeStartScale, relativeStartScale, 1f);

        for (float timeToEval = 0f; timeToEval < totalTime; timeToEval += animationSpeed * Time.deltaTime) {
            float evaluatedScale = scaleAnimation.Evaluate(timeToEval);

            if (evaluatedScale > 0) {
                transform.localScale = Vector3.LerpUnclamped(startAnimationScale, targetedScale, evaluatedScale);
            }
            yield return null;
        }
        transform.localScale = targetedScale;
    }
}
