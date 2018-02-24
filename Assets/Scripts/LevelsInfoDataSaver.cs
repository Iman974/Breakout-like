using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

public class LevelsInfoDataSaver {

    [MenuItem("Scenes Info/Save Scenes Names")]
    private static void SaveScenesNames() {
        // First, try to load the list if it already exists
        LevelsInfoData levelsInfo = (LevelsInfoData)AssetDatabase.LoadAssetAtPath("Assets/Scenes/LevelsInfo.asset", typeof(LevelsInfoData));

        // Otherwise, create it
        if (levelsInfo == null) {
            levelsInfo = ScriptableObject.CreateInstance<LevelsInfoData>();
            AssetDatabase.CreateAsset(levelsInfo, "Assets/Scenes/LevelsInfo.asset");
        }

        EditorBuildSettingsScene[] buildSettingsScenes = EditorBuildSettings.scenes;
        levelsInfo.scenesNames.Clear();
        levelsInfo.scenesPaths.Clear();

        // Fills the list of names in the asset if the scene name matches
        for (int i = 0; i < buildSettingsScenes.Length; i++) {
            Match match = RegexUtility.LevelSceneNameMatch(buildSettingsScenes[i].path);

            if (!match.Success) {
                continue;
            }
            levelsInfo.scenesNames.Add(match.Groups[1].Value);
            levelsInfo.scenesPaths.Add(buildSettingsScenes[i].path);
        }

        levelsInfo.ProcessInfos();
        EditorUtility.SetDirty(levelsInfo);

        // Writes the unsaved asset changes to disk
        AssetDatabase.SaveAssets();
    }
}
