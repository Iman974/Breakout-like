using UnityEngine;

public abstract class Animatable : MonoBehaviour {

    [SerializeField] protected float animationSpeed = 1f;

    protected float totalTime;

    public abstract void Animate();
}
