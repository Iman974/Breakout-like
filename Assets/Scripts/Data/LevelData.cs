using UnityEngine;

[CreateAssetMenu(fileName = "New scene data")]
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

    //private Vector2Int brickGridSize;
    //public Vector2Int BrickGridSize {
    //    get { return brickGridSize; }
    //    set { brickGridSize = value; }
    //}

    private bool isDone;
    public bool IsDone {
        get { return isDone; }
        set { isDone = value; }
    }

    private int stars;
    public int Stars {
        get { return stars; }
        set { stars = value; }
    }
}
