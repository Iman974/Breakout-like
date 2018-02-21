using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsInfo : ScriptableObject {

    public class World {
        public List<Level> levels = new List<Level>();
        //public int worldNumber;
    }

    public class Level {
        //public int levelNumber;
        public int sceneIndex;
    }

    private World[] worlds;

    [HideInInspector] public string[] levelScenesNames;
    [HideInInspector] public int worldCount;

    public void ProcessInfos() {
        int levelCount = SceneManager.sceneCountInBuildSettings - 1;
        string currentSceneName;
        int currentLevelIndex = 0;

        worldCount = LevelManager.GetNumberInString(levelScenesNames[levelScenesNames.Length - 1]);
        worlds = new World[worldCount];

        for (int i = 0; i < worldCount; i++) {
            worlds[i] = new World();
            do {
                //worlds[0].worldNumber = 0;
                worlds[i].levels.Add(new Level() { sceneIndex = currentLevelIndex });
                currentSceneName = levelScenesNames[currentLevelIndex];

                currentLevelIndex++;
            } while (LevelManager.GetNumberInString(currentSceneName) == i + 1 && currentLevelIndex < levelCount);
        }
        Debug.Log("worlds : " + worlds.Length);
        Debug.Log("world 1 : " + worlds[0].levels.Count + " levels");
    }
}
