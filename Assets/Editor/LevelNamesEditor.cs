using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelNamesData))]
public class LevelsNamesEditor : Editor {

    public override void OnInspectorGUI() {
        LevelNamesData levelsNames = (LevelNamesData)target;

        if (levelsNames == null) {
            return;
        }

        base.OnInspectorGUI();

        foreach (string levelName in levelsNames.scenesNames) {
            if (levelName != string.Empty) {
                EditorGUILayout.LabelField(levelName);
            }
        }
    }
}
