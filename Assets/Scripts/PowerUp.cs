using UnityEngine;

[CreateAssetMenu(fileName = "New Power Up")]
public class PowerUp : ScriptableObject {

    public Sprite powerUpSprite;
    public string powerUpName;
    public GameObject pickUpEffect;
    public AnimationCurve fallSpeed = AnimationCurve.Constant(0f, 1f, 0.25f);
    public AnimationCurve horizontalMovement = AnimationCurve.Constant(0f, 1f, 0f);
    public float colliderRadius = 0.3f;
    public float lifetime = 10f;
}
