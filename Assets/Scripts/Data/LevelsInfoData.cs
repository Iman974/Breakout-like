using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsInfoData : ScriptableObject {


    [HideInInspector] public World[] worlds;
    [HideInInspector] private List<string> scenesPaths = new List<string>();
    [HideInInspector] private List<string> scenesNames = new List<string>(); // Try static members for these (research :micro-optimization)
    [HideInInspector] private List<LevelData> levelDatas = new List<LevelData>();

    public Level this[string levelName] {
        get {
            return worlds[RegexUtility.GetNumberInString(levelName) - 1].levels[RegexUtility.GetNumberInString(levelName, 2) - 1];
        }
    }

    public void ResetData() {
        scenesPaths.Clear();
        scenesNames.Clear();
        levelDatas.Clear();
    }

    public void AddData(string sceneName, string scenePath, LevelData levelData) {
        scenesPaths.Add(scenePath);
        scenesNames.Add(sceneName);
        levelDatas.Add(levelData);
    }

    public void ProcessData() {
        string currentLevelName;
        int sceneIndex = 0;

        worlds = new World[RegexUtility.GetNumberInString(scenesNames[scenesNames.Count - 1])];

        for (int worldIndex = 0; worldIndex < worlds.Length; worldIndex++) {
            worlds[worldIndex] = new World();
            int levelIndex = 1;
            do {
                // Add more infos and maybe get back to info nomenclature
                //worlds[worldIndex].worldNumber = worldIndex + 1;
                currentLevelName = scenesNames[sceneIndex];

                worlds[worldIndex].levels.Add(new Level(levelIndex, SceneUtility.GetBuildIndexByScenePath(scenesPaths[sceneIndex]),
                    currentLevelName, levelDatas[sceneIndex]));

                sceneIndex++;
                levelIndex++;
            } while (RegexUtility.GetNumberInString(currentLevelName) == worldIndex + 1 && sceneIndex < scenesNames.Count);
        }
    }
}

[System.Serializable]
public class World {
    public List<Level> levels = new List<Level>();
    //public int worldNumber;
}

[System.Serializable]
public struct Level {
    public int levelNumber;
    public int sceneIndex; // Not needed since levelName holds a related value ?
    public string levelName;
    public LevelData levelData;

    public Level(int lvlNumber, int sceneIndexInBuild, string lvlName, LevelData lvlData) {
        levelNumber = lvlNumber;
        sceneIndex = sceneIndexInBuild;
        levelName = lvlName;
        levelData = lvlData;
    }
}
