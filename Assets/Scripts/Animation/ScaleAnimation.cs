using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New scale animation", menuName = "Custom Animations/Scale", order = 2)]
public class ScaleAnimation : Animatable {

    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private float relativeStartScale;

    private Vector3 targetedScale;

    private void Awake() {
        Debug.Log("demarré");
    }

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
