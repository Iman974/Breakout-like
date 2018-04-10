using UnityEngine;
using System.Collections;

public static class AnimationUtility {

    /// <summary>
    /// Creates a coroutine that plays an animation.
    /// </summary>
    /// <param name="speed">
    /// The animation speed.
    /// </param>
    /// <param name="curve">
    /// The curve to play the animation from.
    /// </param>
    public static IEnumerator DoAnimation(float speed, AnimationCurve curve) {
        for (float time = 0f; time < 1f; time += speed * Time.deltaTime) {
            //backButtonImg.color = Color.Lerp(startColor, endColor, backBtnFadeAnimation.Evaluate(time)); //delegates ? but because of speed, just totally useless ?
            yield return null;
        }
    }

    /// <summary>
    /// Creates a coroutine that moves the given transform to a position using lerp with the given curve.
    /// </summary>
    /// <param name="toMove">
    /// The object's transform to move.
    /// </param>
    /// <param name="destination">
    /// The position to move the transform to.
    /// </param>
    /// <param name="movement">
    /// The curve that describes the movement.
    /// </param>
    /// <param name="speed">
    /// How fast the movement will be.
    /// </param>
    public static IEnumerator MoveToPosition(Transform toMove, Vector2 destination, AnimationCurve movement, float speed = 1f) {
        Vector3 startPosition = toMove.position;

        for (float time = 0f; time < 1f; time += speed * Time.deltaTime) {
            Vector2 result = Vector2.LerpUnclamped(startPosition, destination, movement.Evaluate(time));
            toMove.position = new Vector3(result.x, result.y, startPosition.z);

            yield return null;
        }
        toMove.position = new Vector3(destination.x, destination.y, startPosition.z);
    }
}
