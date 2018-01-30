using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextAnimation : MonoBehaviour {

    [SerializeField] private AnimationCurve fontSizeAnimation;
    [SerializeField] private float sizeAnimationSpeed = 0.15f;
    [SerializeField] private AnimationCurve colorLerpAnimation;
    [SerializeField] private float fadeOutSpeed = 0.01f;

    private Text textToAnimate;
    private string[] stringToSet;
    private bool wasEnabledAtStart;
    private Color initialColor;

    private int animationsRunning;
    private int AnimationsRunning {
        get {
            return animationsRunning;
        }
        set {
            animationsRunning = value;
            if (animationsRunning == 0) {
                IsAnimationRunning = false;
            }
        }
    }

    public string CurrentAnimatingString { get; private set; }
    public Color ColorToSet { private get; set; }
    public bool IsAnimationRunning { get; private set; }

	private void Awake() {
        ColorToSet = Color.white;
        textToAnimate = GetComponent<Text>();
        wasEnabledAtStart = textToAnimate.enabled;
        initialColor = textToAnimate.color;
    }

    public void SetText(string textToSet) {
        textToAnimate.text = textToSet;
    }

    public IEnumerator StartAnimation(Animation animation, float delay = 0f, params string[] textToDisplay) {
        stringToSet = textToDisplay;
        textToAnimate.enabled = true;

        foreach (string currentString in stringToSet) {
            CurrentAnimatingString = currentString;
            textToAnimate.text = currentString;

            float timeBeforeAnimation = Time.time;

            if ((animation & Animation.SIZE) != 0) {
                animationsRunning++;
                StartCoroutine(AnimateSize());
            }
            if ((animation & Animation.COLOR) != 0) {
                if (ColorToSet != textToAnimate.color) {
                    animationsRunning++;
                    StartCoroutine(AnimateColor());
                }
            }
            if ((animation & Animation.ALPHA) != 0) {
                StartCoroutine(AnimateAlpha());
            }

            while (animationsRunning > 0) {
                yield return null;
            }

            float timeToWaitBeforeNextAnimation = delay - (Time.time - timeBeforeAnimation);
            if (timeToWaitBeforeNextAnimation > 0f) {
                yield return new WaitForSeconds(timeToWaitBeforeNextAnimation);
            }
        }

        textToAnimate.enabled = wasEnabledAtStart;
    }
	
    private IEnumerator AnimateSize() {
        float totalTime = fontSizeAnimation.keys[fontSizeAnimation.length - 1].time;

        for (float time = 0f; time < totalTime; time += sizeAnimationSpeed) {
            textToAnimate.fontSize = (int)fontSizeAnimation.Evaluate(time);
            yield return null;
        }
        AnimationsRunning--;
    }

    private IEnumerator AnimateColor() {
        float totalTime = colorLerpAnimation.keys[colorLerpAnimation.length - 1].time;

        for (float time = 0f; time < totalTime; time += Time.deltaTime) {
            textToAnimate.color = Color.Lerp(initialColor, ColorToSet, colorLerpAnimation.Evaluate(time));
            yield return null;
        }

        for (float time = 0f; time < totalTime; time += Time.deltaTime) {
            textToAnimate.color = Color.Lerp(ColorToSet, initialColor, colorLerpAnimation.Evaluate(time));
            yield return null;
        }
        AnimationsRunning--;
    }

    private IEnumerator AnimateAlpha() {
        float totalTime = 1f / fadeOutSpeed;

        for (int i = 0; i < totalTime; i++) {
            textToAnimate.color += new Color(0f, 0f, 0f, -fadeOutSpeed);
            yield return null;
        }
    }
}

public enum Animation {
    COLOR = 1,
    SIZE,
    ALPHA = 4
}
