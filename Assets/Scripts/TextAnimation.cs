using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextAnimation : MonoBehaviour {

    [SerializeField] private AnimationCurve fontSizeAnimation;
    [SerializeField] private float animationSpeed = 0.2f;

    private Text textToAnimate;

	private void Awake() {
        textToAnimate = GetComponent<Text>();
	}

    public void StartAnimation(float delay, params string[] textToDisplay) {
        textToAnimate.enabled = true;
        StartCoroutine(Animate(delay, textToDisplay));
    }
	
    private IEnumerator Animate(float delay, string[] textToDisplay) {
        float totalTime = fontSizeAnimation.keys[fontSizeAnimation.length - 1].time;

        foreach (string txt in textToDisplay) {
            textToAnimate.text = txt;
            float timeBeforeAnimation = Time.time;

            for (float time = 0f; time < totalTime; time += animationSpeed) {
                textToAnimate.fontSize = (int)fontSizeAnimation.Evaluate(time);
                yield return null;
            }

            yield return new WaitForSeconds(delay - (Time.time - timeBeforeAnimation));
        }
        textToAnimate.enabled = false;
    }
}
