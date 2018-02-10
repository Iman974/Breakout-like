using UnityEngine;
using System.Collections;

//[CreateAssetMenu(fileName = "New rotation animation", menuName = "Custom Animations/Rotate", order = 1)]
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
