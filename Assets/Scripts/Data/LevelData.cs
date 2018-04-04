using UnityEngine;

/// <summary>
/// Class for storing level data. It can be created an asset via the unity toolbar.
/// </summary>
[CreateAssetMenu(fileName = "New scene data", menuName = "Breakout/Level Data")]
public class LevelData : ScriptableObject {

    [SerializeField] private Vector2 gamepadPosition = new Vector2(0f, -4f);
    public Vector2 GamepadPosition {
        get { return gamepadPosition; }
        set { gamepadPosition = value; }
    }

    [SerializeField] private Vector2 mainBallPosition = new Vector2(0f, -3f);
    public Vector2 MainBallPosition {
        get { return mainBallPosition; }
        set { mainBallPosition = value; }
    }

    [SerializeField] private int levelLives = 2;
    public int LevelLives {
        get {
            return levelLives;
        }
    }

    [Tooltip("This array describes the score multipliers that need to be reached in order to get a star. It is multiplied with the " +
        "total score value of the bricks.")]
    [SerializeField] private int[] starLevels = new int[3];
    public int[] StarLevels {
        get { return starLevels; }
        set { starLevels = value; }
    }

    [Tooltip("How many bricks have a power.")]
    [SerializeField] private int poweredBrickCount = 2;
    /// <summary>
    /// How many bricks have a power.
    /// </summary>
    public int PoweredBrickCount {
        get { return poweredBrickCount; }
        set { poweredBrickCount = value; }
    }

    [Tooltip("The possible brick power that can 'spawn'.")]
    [SerializeField] private BrickPower[] brickPower;
    public BrickPower[] BrickPowers {
        get { return brickPower; }
        set { brickPower = value; }
    }

    //private Vector2Int brickGridSize;
    //public Vector2Int BrickGridSize {
    //    get { return brickGridSize; }
    //    set { brickGridSize = value; }
    //}
}
