using UnityEngine;

//[CreateAssetMenu(fileName = "New Power Up")]
public class OldPowerUp : ScriptableObject {

    //private IPower power;

    //public Sprite powerUpSprite;
    //public string powerUpName;
    //public GameObject pickUpEffect;
    public AnimationCurve verticalMovement = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    public AnimationCurve horizontalMovement = AnimationCurve.Constant(0f, 1f, 0f);
    //public float colliderRadius = 0.35f;
    public float lifetime = 10f;
    public Vector2 targetedRelativePosition = new Vector2(0f, -10f);
    public float yMovementSpeed = 0.0007f;
    public float xMovementSpeed = 0.0007f;
    public float xMovementMaxBound = 2f;
    //public PowersName powerName;

    //public void InitPower() {
    //    ////power = (IPower)typeof(Powers).GetField(powerName.ToString(), BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Powers.Instance);
    //    //power = Powers.Instance.powers[(int)powerName];
    //}

    //public void TriggerPower() {
    //    power.ActivatePower();
    //}
}

//public abstract class IPower {

//    [SerializeField] protected float powerDuration = 2f;

//    public abstract void ActivatePower();
//}
