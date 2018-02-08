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

    [SerializeField] new private Animation animation;
    [SerializeField] private Graphic[] componentToAnimate;

    private float timeToEval;

    private void Start() {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate() {
        float animationSpeed = animation.animationSpeed;
        float totalTime = animation.animationCurve[animation.animationCurve.length - 1].time;
        Color nextColor = componentToAnimate[0].color;
        float startAlpha = componentToAnimate[0].color.a, lerpToAlpha = animation.toColor.a;

        for (float timeToEval = 0; timeToEval < totalTime; timeToEval += animationSpeed * Time.deltaTime) {
            nextColor.a = Mathf.Lerp(startAlpha, lerpToAlpha, animation.animationCurve.Evaluate(timeToEval));
            componentToAnimate[0].color = nextColor;
            yield return null;//new WaitForSeconds(animation.animationSpeed);
        }
        componentToAnimate[0].color += new Color(0f, 0f, 0f, animation.animationCurve.Evaluate(totalTime));
        if (animation.triggerAtEnd != null) {
            animation.triggerAtEnd.Invoke();
        }
    }
}
