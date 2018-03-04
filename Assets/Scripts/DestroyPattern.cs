using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Destroy Pattern", menuName = "Brick Power/Destroy Pattern")]
public class DestroyPattern : ScriptableObject {

    [System.Serializable]
    public struct BrickVector {
        private string name;
        public string Name {
            get { return name; }
        }

        [SerializeField] private Vector2 direction;
        public Vector2 Direction {
            get { return direction; }
        }

        [Tooltip("How many bricks to destroy in the given direction.")]
        [SerializeField] private int count;
        public int Count {
            get { return count; }
        }

        [Tooltip("This is the length of the raycast used to detect bricks in the given direction.")]
        [SerializeField] private float length;
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

        public RaycastHit2D[] RayHits2D { get; private set; }

        public BrickVector(Vector2 dir, string dirName) {
            direction = dir.normalized;
            name = dirName;
            length = Mathf.Infinity;
            count = 1;
            enabled = false;
            RayHits2D = new RaycastHit2D[count];
        }
    }

    [SerializeField] private BrickVector[] brickVectors = new BrickVector[] {
        new BrickVector(new Vector2(-1f, 0f), "Left"),
        new BrickVector(new Vector2(-0.7f, 0.7f), "UpLeft"),
        new BrickVector(new Vector2(0f, 1f), "Up"),
        new BrickVector(new Vector2(0.7f, 0.7f), "UpRight"),
        new BrickVector(new Vector2(1f, 0f), "Right"),
        new BrickVector(new Vector2(0.7f, -0.7f), "DownRight"),
        new BrickVector(new Vector2(0f, -1f), "Down"),
        new BrickVector(new Vector2(-0.7f, -0.7f), "DownLeft")
    };

    public BrickVector[] BrickVectors {
        get {
            return brickVectors;
        }
    }

    public BrickVector[] EnabledBrickVectors {
        get {
            List<BrickVector> enabledVectors = new List<BrickVector>();

            for (int i = 0; i < brickVectors.Length; i++) {
                if (brickVectors[i].Enabled) {
                    enabledVectors.Add(brickVectors[i]);
                }
            }
            return enabledVectors.ToArray();
        }
    }
}
