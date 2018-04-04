using UnityEngine;

public static class VectorUtility {

    /// <summary>
    /// Creates a vector with the x and y components taken from a vector2 and the given z component.
    /// </summary>
    /// <param name="xy">
    /// The vector2 to build from.
    /// </param>
    /// <param name="z">
    /// What value to assign to z component.
    /// </param>
    /// <returns></returns>
    public static Vector3 BuildVector(Vector2 xy, float z) {
        return new Vector3(xy.x, xy.y, z);
    }
}
