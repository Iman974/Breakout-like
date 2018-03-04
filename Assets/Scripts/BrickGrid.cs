using UnityEngine;

public class BrickGrid : MonoBehaviour {

    private Brick[][] brickGrid;

    public static BrickGrid Instance;

    private int width, height;
    public int Width {
        get {
            return width;
        }
        set {
            width = value;
        }
    }
    public int Height {
        get {
            return height;
        }
        set {
            height = value;
        }
    }

    private void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
            return;
        }
        #endregion
        brickGrid = new Brick[width][];
        for (int i = 0; i < width; i++) {
            brickGrid[i] = new Brick[height];
        }
    }

    public void SetBrick(Brick brick, int x, int y) {
        brickGrid[x][y] = brick;
    }
}
