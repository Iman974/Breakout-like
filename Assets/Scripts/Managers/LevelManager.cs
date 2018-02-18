using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    [HideInInspector] public int currentWorld = 1;

    private class World {
        public Level[] levels;
        public int worldNumber;
    }

    private class Level {
        public int levelNumber;
    }

    private World[] worlds;

    public void SetCurrentWorld(Text senderText) {
        currentWorld = senderText.text[senderText.text.Length - 1];
    }

    public void PlayGameLevel(Text levelText) {
        SceneManager.LoadSceneAsync(currentWorld + "-" + levelText.text[levelText.text.Length - 1]);
    }
}
