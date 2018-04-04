using UnityEngine;

public class LevelButton : MonoBehaviour {

    //private LevelManager levelManager;

    public Level Level { get; set; }

    //private void Start() {
    //    levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
    //}

    public void CallLoadLevel() {
        LevelManager.LoadLevelAsync(Level.Name);
    }
}
