using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    [HideInInspector] public int currentWorld = 1;

    private class World {
        public Level[] levels;
    }

    private class Level {
        public int levelIndex;
    }

    private World[] worlds;

    private void PlayGameLevel(int level) {
        SceneManager.LoadSceneAsync("" + "-" + "");
    }
}
