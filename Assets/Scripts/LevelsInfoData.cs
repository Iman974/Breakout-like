using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsInfoData : ScriptableObject {

    [System.Serializable]
    public class World {
        public List<Level> levels = new List<Level>();
        public int worldNumber;
    }

    [System.Serializable]
    public class Level {
        public int levelNumber;
        public int sceneIndex;
        public string levelName;
    }

    [HideInInspector] public World[] worlds;
    [HideInInspector] public List<string> scenesPaths = new List<string>();
    [HideInInspector] public List<string> scenesNames = new List<string>(); // Try static members for these

    public void ProcessInfos() {
        string currentLevelName;
        int sceneIndex = 0;

        worlds = new World[RegexUtility.GetNumberInString(scenesNames[scenesNames.Count - 1])];

        for (int worldIndex = 0; worldIndex < worlds.Length; worldIndex++) {
            worlds[worldIndex] = new World();
            int levelIndex = 1;
            do {
                // Add more infos and maybe get back to info nomenclature
                worlds[worldIndex].worldNumber = worldIndex + 1;
                currentLevelName = scenesNames[sceneIndex];

                worlds[worldIndex].levels.Add(new Level() { levelNumber = levelIndex,
                    sceneIndex = SceneUtility.GetBuildIndexByScenePath(scenesPaths[sceneIndex]), levelName = currentLevelName });

                sceneIndex++;
                levelIndex++;
            } while (RegexUtility.GetNumberInString(currentLevelName) == worldIndex + 1 && sceneIndex < scenesNames.Count);
        }
        //Debug.Log("worlds : " + worlds.Length);
        //Debug.Log("world 1 : " + worlds[0].levels.Count + " levels");
        //Debug.Log("Level one : " + worlds[0].levels[0].levelNumber + worlds[0].levels[0].sceneIndex);
    }
}
