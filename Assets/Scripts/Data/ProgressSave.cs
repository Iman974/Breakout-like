using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Class for handling game saves.
/// </summary>
[System.Serializable]
public class ProgressSave {

    private Dictionary<string, LevelStats> levelStats = new Dictionary<string, LevelStats>(); // The key is the level name

    public int TotalStarsCount { get; private set; }

    /// <summary>
    /// Returns stats about a specific level in the game.
    /// </summary>
    public LevelStats this[string levelName] {
        get {
            return levelStats[levelName];
        }
    }

    /// <summary>
    /// Create a save object.
    /// </summary>
    /// <param name="levelsInfo">
    /// LevelsInfoData object to get info to save from.
    /// </param>
    private static ProgressSave CreateSave(LevelsInfoData levelsInfo) {
        ProgressSave progressSave = new ProgressSave();

        for (int i = 0; i < levelsInfo.TotalLevelCount; i++) {
            progressSave.levelStats.Add(levelsInfo[i].Name, levelsInfo[i].Stats);
        }
        progressSave.TotalStarsCount = GameManager.TotalStarsCount;

        return progressSave;
    }

    /// <summary>
    /// Saves the game progress from the game into a save file.
    /// </summary>
    public static void SaveProgress(string saveFilePath) {
        ProgressSave save = CreateSave(LevelManager.LevelsInfo);

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        System.IO.FileStream saveFileStream = System.IO.File.Create(saveFilePath);

        binaryFormatter.Serialize(saveFileStream, save);
        saveFileStream.Close();
    }

    /// <summary>
    /// Gets the save file from the given path.
    /// </summary>
    /// <returns>
    /// Returns the save object if the save file was found, null otherwise.
    /// </returns>
    public static ProgressSave GetProgressSave(string saveFilePath) {
        if (System.IO.File.Exists(saveFilePath)) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            System.IO.FileStream saveFileStream = System.IO.File.OpenRead(saveFilePath);

            ProgressSave progressSave = (ProgressSave)binaryFormatter.Deserialize(saveFileStream);
            saveFileStream.Close();

            return progressSave;
        }
        return null;
    }
}
