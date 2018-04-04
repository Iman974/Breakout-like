using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Destroy Pattern", menuName = "Breakout/Brick Power/Destroy Pattern")]
public class DestructionPattern : ScriptableObject {

    [SerializeField] private BrickVector[] brickVectors = new BrickVector[] {
        new BrickVector(0f, "Up"),
        new BrickVector(45f, "UpRight"),
        new BrickVector(90f, "Right"),
        new BrickVector(135f, "DownRight"),
        new BrickVector(180f, "Down"),
        new BrickVector(225f, "DownLeft"),
        new BrickVector(270f, "Left"),
        new BrickVector(315f, "UpLeft")
    };

    public BrickVector[] BrickVectors {
        get {
            return brickVectors;
        }
    }

    public BrickVector[] GetEnabledBrickVectors() {
        List<BrickVector> enabledVectors = new List<BrickVector>();

        for (int i = 0; i < brickVectors.Length; i++) {
            if (brickVectors[i].Enabled) {
                enabledVectors.Add(brickVectors[i]);
            }
        }
        return enabledVectors.ToArray();
    }
}

[System.Serializable]
public struct BrickVector {
    private string name;
    public string Name {
        get { return name; }
    }

#pragma warning disable 0414
    [SerializeField] [Range(0f, 359.99f)] private float angle;
#pragma warning restore 0414
    [SerializeField] private Vector2 direction;
    public Vector2 Direction {
        get { return direction; }
        set { direction = value; }
    }

    [Tooltip("How many bricks to destroy in the given direction.")]
    [SerializeField]
    private int count;
    public int Count {
        get { return count; }
    }

    [Tooltip("This is the length of the raycast used to detect bricks in the given direction.")]
    [SerializeField]
    private float length;
    public float Length {
        get { return length; }
    }

    [SerializeField] private bool enabled;
    public bool Enabled {
        get {
            return enabled;
        }
        set {
            enabled = value;
        }
    }

    //public RaycastHit2D[] RayHits2D { get; private set; }

    public BrickVector(float directionAngle, string directionName) {
        direction = GetDirectionFromAngle(directionAngle);
        angle = directionAngle;
        name = directionName;
        length = Mathf.Infinity;
        count = 1;
        enabled = false;
        //RayHits2D = new RaycastHit2D[count];
    }

    public static Vector2 GetDirectionFromAngle(float angle) {
        return new Vector2(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
    }
}
