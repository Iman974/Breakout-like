using UnityEngine;

public abstract class Animatable : Object {

    [SerializeField] protected float animationSpeed = 1f;

    protected float totalTime = 1f;
    protected CustomAnimation attachedCustomAnimation;

    public abstract void Animate();

    public void BindAnimation(CustomAnimation customAnimation) {
        attachedCustomAnimation = customAnimation;
        //targetedScale = transform.localScale;
    }
}
