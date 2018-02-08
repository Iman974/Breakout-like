using UnityEngine;

public abstract class IAnimatable {

    [SerializeField] protected float animationSpeed = 1f;

    protected AnimatedReveal attachedAnimation;
    protected float totalTime;

    public abstract void Animate();
}
