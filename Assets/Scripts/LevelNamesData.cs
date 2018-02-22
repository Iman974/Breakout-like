using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelNamesData : ScriptableObject {

    public class World {
        public List<Level> levels = new List<Level>();
        //public int worldNumber;
    }

    public class Level {
        public int levelNumber;
        public int sceneIndex;
    }

    [HideInInspector] public World[] worlds;
    [HideInInspector] public List<string> scenesPaths = new List<string>();
    [HideInInspector] public List<string> scenesNames = new List<string>();
    [HideInInspector] public int worldCount;

    public void ProcessInfos() {
        string currentSceneName;
        int sceneIndex = 0;

        worldCount = LevelManager.GetNumberInString(scenesNames[scenesNames.Count - 1]);
        worlds = new World[worldCount];

        for (int worldIndex = 0; worldIndex < worldCount; worldIndex++) {
            worlds[worldIndex] = new World();
            int levelIndex = 1;
            do {
                //worlds[0].worldNumber = 0;
                // Add more infos and maybe get back to info nomenclature
                currentSceneName = scenesNames[sceneIndex];

                worlds[worldIndex].levels.Add(new Level() { levelNumber = levelIndex,
                    sceneIndex = SceneUtility.GetBuildIndexByScenePath(scenesPaths[sceneIndex]) });

                sceneIndex++;
            } while (LevelManager.GetNumberInString(currentSceneName) == worldIndex + 1 && sceneIndex < scenesNames.Count);
        }
        Debug.Log("worlds : " + worlds.Length);
        Debug.Log("world 1 : " + worlds[0].levels.Count + " levels");
        Debug.Log("Level one : " + worlds[0].levels[0].levelNumber + worlds[0].levels[0].sceneIndex);
    }
}
