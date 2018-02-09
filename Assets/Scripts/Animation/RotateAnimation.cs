using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CustomAnimation))]
public class RotateAnimation : Animatable {

    [SerializeField] private AnimationCurve rotationCurve;
    [SerializeField] private float relativeStartRotation;

    private Quaternion targetedRotation;

    private void Start() {
        targetedRotation = transform.rotation;
    }

    public override void Animate() {
        transform.rotation *= Quaternion.Euler(new Vector3(0f, 0f, relativeStartRotation));
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate() {
        Quaternion startAnimationRotation = Quaternion.Euler(new Vector3(0f, 0f, relativeStartRotation));
        for (float timeToEval = 0f; timeToEval < totalTime; timeToEval += animationSpeed * Time.deltaTime) {
            float evaluatedRotation = rotationCurve.Evaluate(timeToEval);

            if (evaluatedRotation > 0) {
                transform.rotation = Quaternion.LerpUnclamped(startAnimationRotation, targetedRotation,
                    evaluatedRotation);
            }
            yield return null;
        }
        transform.rotation = targetedRotation;
    }
}
