using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class for storing information about all the levels in the game, such as the level count, their scene indexes, their names etc.
/// </summary>
public class LevelsInfoData : ScriptableObject {

    [SerializeField] private List<LevelData> levelDatas = new List<LevelData>();

    [SerializeField] private World[] worlds;
    public World[] Worlds {
        get {
            return worlds;
        }
    }

    private List<string> scenesPaths = new List<string>();
    private List<string> scenesNames = new List<string>(); // Try static members for these (research :micro-optimization)

    public int TotalLevelCount {
        get {
            int count = 0;

            foreach (World world in worlds) {
                count += world.LevelCount;
            }

            return count;
        }
    }

    public Level this[string levelName] {
        get {
            return worlds[RegexUtility.GetNumberInString(levelName) - 1][RegexUtility.GetNumberInString(levelName, 2) - 1];
        }
    }

    public Level this[int levelIndex] {
        get {
            foreach (World world in worlds) {
                if (levelIndex < world.LevelCount) {
                    return world[levelIndex];
                }

                levelIndex -= world.LevelCount;
            }

            throw new System.ArgumentOutOfRangeException("levelIndex", "No such level was found");
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

        for (int worldIndex = 0; worldIndex < Worlds.Length; worldIndex++) {
            Worlds[worldIndex] = new World();
            int levelNumber = 1;

            while (sceneIndex < scenesNames.Count && RegexUtility.GetNumberInString(scenesNames[sceneIndex]) == worldIndex + 1) {
                currentLevelName = scenesNames[sceneIndex];

                Worlds[worldIndex].AddLevel(new Level(levelNumber, SceneUtility.GetBuildIndexByScenePath(scenesPaths[sceneIndex]),
                    currentLevelName, levelDatas[sceneIndex]));

                sceneIndex++;
                levelNumber++;
            }
        }
    }
}

[System.Serializable]
public class World {

    [SerializeField] private List<Level> levels = new List<Level>();

    public int LevelCount {
        get {
            return levels.Count;
        }
    }

    public Level this[int index] {
        get {
            return levels[index];
        }
        set {
            levels[index] = value;
        }
    }

    public void AddLevel(Level levelToAdd) {
        levels.Add(levelToAdd);
    }
}

/// <summary>
/// Class for storing data about a level, such as its name, stats etc.
/// </summary>
[System.Serializable]
public class Level {

    [SerializeField] private int number;
    public int Number {
        get { return number; }
        set { number = value; }
    }

    [SerializeField] private int sceneIndex;
    public int SceneIndex {
        get { return sceneIndex; }
        set { sceneIndex = value; }
    }

    [SerializeField] private string name;
    public string Name {
        get { return name; }
        set { name = value; }
    }

    [SerializeField] private LevelData data;
    public LevelData Data {
        get { return data; }
        set { data = value; }
    }

    [SerializeField] private LevelStats stats;
    public LevelStats Stats {
        get {
            return stats;
        }
        set {
            stats = value;
        }
    }

    public Level(int lvlNumber, int sceneIndexInBuild, string lvlName, LevelData lvlData) {
        number = lvlNumber;
        sceneIndex = sceneIndexInBuild;
        name = lvlName;
        data = lvlData;
        stats = new LevelStats();
    }
}

/// <summary>
/// Class for storing stats about a level, such as whether it has been finished, and if so, how many stars the player has won etc.
/// </summary>
[System.Serializable]
public class LevelStats {

    public bool IsDone { get; set; }
    //private bool isDone;
    //public bool IsDone {
    //    get { return isDone; }
    //    set { isDone = value; }
    //}

    public int StarsCount { get; set; }
    //private int stars;
    //public int Stars {
    //    get { return stars; }
    //    set { stars = value; }
    //}
}
