using UnityEngine;

public class LevelButton : MonoBehaviour {

    private LevelManager levelManager;

    public Level level;

    private void Start() {
        levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
    }

    public void CallLoadLevel() {
        levelManager.PlayGameLevel(level.levelName);
    }
}
