using UnityEngine;

[CreateAssetMenu(fileName = "New scene data")]
public class LevelData : ScriptableObject {

    public Vector2 gamepadPosition;
    public Vector2 mainBallPosition;

    [HideInInspector] public bool hasWon;
    [HideInInspector] public int stars;
}
