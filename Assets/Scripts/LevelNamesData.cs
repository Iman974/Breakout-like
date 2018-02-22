using System.Collections.Generic;
using UnityEngine;

public class LevelNamesData : ScriptableObject {

    public class World {
        public List<Level> levels = new List<Level>();
        //public int worldNumber;
    }

    public class Level {
        //public int levelNumber;
        public int sceneIndex;
    }

    private World[] worlds;

    [HideInInspector] public List<string> scenesNames = new List<string>();
    [HideInInspector] public int worldCount;

    public void ProcessInfos() {
        string currentSceneName;
        int currentLevelIndex = 0;

        worldCount = LevelManager.GetNumberInString(scenesNames[scenesNames.Count - 1]);
        worlds = new World[worldCount];

        for (int i = 0; i < worldCount; i++) {
            worlds[i] = new World();
            do {
                //worlds[0].worldNumber = 0;
                // Add more infos and maybe get back to info nomenclature
                worlds[i].levels.Add(new Level() { sceneIndex = currentLevelIndex });
                currentSceneName = scenesNames[currentLevelIndex];

                currentLevelIndex++;
            } while (LevelManager.GetNumberInString(currentSceneName) == i + 1 && currentLevelIndex < scenesNames.Count);
        }
        Debug.Log("worlds : " + worlds.Length);
        Debug.Log("world 1 : " + worlds[0].levels.Count + " levels");
    }
}
