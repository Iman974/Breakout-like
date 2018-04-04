using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextAnimation : MonoBehaviour {

    [SerializeField] private AnimationCurve fontSizeAnimation;
    [SerializeField] private float sizeAnimationSpeed = 0.15f;
    [SerializeField] private AnimationCurve colorLerpAnimation;
    [SerializeField] private AnimationCurve alphaAnimation;

    private Text textToAnimate;
    private string[] stringToSet;
    private bool wasEnabledAtStart;
    private Color initialColor;
    private int initialFontSize;

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

    /// <summary>
    /// The string that is currently being animated.
    /// </summary>
    public string AnimatingString { get; private set; }
    public Color ColorToSet { private get; set; }
    public bool IsAnimationRunning { get; private set; }

	private void Awake() {
        ColorToSet = Color.white;
        textToAnimate = GetComponent<Text>();
        wasEnabledAtStart = textToAnimate.enabled;
        initialColor = textToAnimate.color;
        initialFontSize = textToAnimate.fontSize;
    }

    public void SetText(string textToSet) {
        textToAnimate.text = textToSet;
    }

    public IEnumerator StartAnimation(Animation animation, float delay = 0f, params string[] textToDisplay) {
        stringToSet = textToDisplay;
        textToAnimate.enabled = true;

        foreach (string currentString in stringToSet) {
            AnimatingString = currentString;
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
        float totalTime = fontSizeAnimation[fontSizeAnimation.length - 1].time;

        for (float time = 0f; time < totalTime; time += sizeAnimationSpeed) {
            textToAnimate.fontSize = initialFontSize + (int)fontSizeAnimation.Evaluate(time);
            yield return null;
        }
        AnimationsRunning--;
    }

    private IEnumerator AnimateColor() {
        float totalTime = colorLerpAnimation[colorLerpAnimation.length - 1].time;

        for (float time = 0f; time < totalTime; time += Time.deltaTime) {
            textToAnimate.color = Color.Lerp(initialColor, ColorToSet, colorLerpAnimation.Evaluate(time));
            yield return null;
        }

        AnimationsRunning--;
    }

    private IEnumerator AnimateAlpha() {
        float totalTime = alphaAnimation[alphaAnimation.length - 1].time;

        for (float time = 0f; time < totalTime; time += Time.deltaTime) {
            textToAnimate.color = new Color(textToAnimate.color.r, textToAnimate.color.g, textToAnimate.color.b,
                alphaAnimation.Evaluate(time));
            yield return null;
        }
    }
}

public enum Animation {
    COLOR = 1,
    SIZE,
    ALPHA = 4
}
