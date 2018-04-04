using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class GenericAnimation : MonoBehaviour {

    // Put this whole thing in animatedreveal ?
    // Use generic params ?
    [System.Serializable]
    private class Animation {
        public AnimationCurve animationCurve;
        public float animationSpeed = 0.01f;
        public Color toColor = Color.clear;
        public UnityEvent triggerAtEnd;
    }

    [SerializeField] private Animation thisAnimation;
    [SerializeField] private Graphic[] componentToAnimate;

    private float timeToEval;

    private void Start() {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate() {
        float animationSpeed = thisAnimation.animationSpeed;
        float totalTime = thisAnimation.animationCurve[thisAnimation.animationCurve.length - 1].time;
        Color nextColor = componentToAnimate[0].color;
        float startAlpha = componentToAnimate[0].color.a, lerpToAlpha = thisAnimation.toColor.a;

        for (float timeToEval = 0; timeToEval < totalTime; timeToEval += animationSpeed * Time.deltaTime) {
            nextColor.a = Mathf.Lerp(startAlpha, lerpToAlpha, thisAnimation.animationCurve.Evaluate(timeToEval));
            componentToAnimate[0].color = nextColor;
            yield return null;//new WaitForSeconds(animation.animationSpeed);
        }
        componentToAnimate[0].color += new Color(0f, 0f, 0f, thisAnimation.animationCurve.Evaluate(totalTime));
        if (thisAnimation.triggerAtEnd != null) {
            thisAnimation.triggerAtEnd.Invoke();
        }
    }
}
