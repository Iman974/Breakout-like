using UnityEngine;
using System.Collections;

public class AnimatedAppear : MonoBehaviour {

    [SerializeField] private AnimationCurve appearPosition;
    [SerializeField] private Ball ball;

	private void Start() {
        Invoke("StartSlideScreen", 0.1f);
	}

    private void StartSlideScreen() { // Only for debug purposes
        StartCoroutine(SlideScreen());
    }
	
    private IEnumerator SlideScreen() {
        float totalTime = appearPosition.keys[appearPosition.length - 1].time;
        for (float time = 0f; time < totalTime; time += Time.deltaTime) {
            transform.position = new Vector2(0f, appearPosition.Evaluate(time));
            yield return null;
        }
        ball.LaunchBall();
    }
}
