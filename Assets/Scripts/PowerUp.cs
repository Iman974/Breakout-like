using UnityEngine;

[CreateAssetMenu(fileName = "New Power Up")]
public class PowerUp : ScriptableObject {

    public Sprite powerUpSprite;
    public string powerUpName;
    public GameObject pickUpEffect;
    public AnimationCurve verticalMovement = AnimationCurve.Linear(0f, 0f, 5f, -10f);
    public AnimationCurve horizontalMovement = AnimationCurve.Constant(0f, 1f, 0f);
    public float colliderRadius = 0.3f;
    public float lifetime = 10f;
}
