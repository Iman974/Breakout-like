using UnityEngine;

public class MinAttribute : PropertyAttribute {

    private float minimum;
    public float Minimum {
        get {
            return minimum;
        }
    }

    /// <summary>
    /// Attribute used to make a float or int variable restricted to be greater or equal than a specific number.
    /// </summary>
    /// <param name="min">
    /// The minimum allowed value.
    /// </param>
    public MinAttribute(float minValue) {
        minimum = minValue;
    }
}
