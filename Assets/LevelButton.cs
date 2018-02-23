using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

    private LevelManager levelManager;
    private int levelNumber;

    private void Start() {
        levelNumber = RegexUtility.GetNumberInString(GetComponentInChildren<Text>().text);
        levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
    }

    public void CallLoadLevel() {
        levelManager.PlayGameLevel(levelNumber);
    }
}
