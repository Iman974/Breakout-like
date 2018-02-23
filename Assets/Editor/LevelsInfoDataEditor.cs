using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelsInfoData))]
public class LevelsInfoDataEditor : Editor {

    private struct CustomFoldout {
        public bool IsFoldout { get; private set; }

        public void Foldout(string label) {
            IsFoldout = EditorGUILayout.Foldout(IsFoldout, label, true);
        }
    }

    private static GUIStyle paddingLevel1;
    private static GUIStyle paddingLevel2;
    private static CustomFoldout[] levelFoldouts = new CustomFoldout[5];

    public override void OnInspectorGUI() {
        LevelsInfoData levelsInfo = (LevelsInfoData)target;

        if (levelsInfo == null) {
            return;
        }

        int worldIndex = 0;
        foreach (var world in levelsInfo.worlds) {
            for (int i = 0; i < world.levels.Count; i++) {
                EditorGUILayout.LabelField("World " + (worldIndex + 1), new GUIStyle() { fontStyle = FontStyle.Bold });

                EditorGUILayout.BeginVertical(paddingLevel1);
                //levelFoldout = EditorGUILayout.Foldout(levelFoldout, "Level " + (i + 1), true);

                levelFoldouts[i].Foldout("Level " + (i + 1));
                if (levelFoldouts[i].IsFoldout) {
                    EditorGUILayout.BeginVertical(paddingLevel1);

                    EditorGUILayout.LabelField(world.levels[i].levelName);
                    EditorGUILayout.LabelField(levelsInfo.worlds[RegexUtility.GetNumberInString(world.levels[i].levelName) - 1]
                        .levels[0].sceneIndex.ToString());

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndVertical();
            }
            worldIndex++;
        }
    }

    private void OnEnable() {
        paddingLevel1 = new GUIStyle { padding = new RectOffset(10, 0, 0, 0) };
        paddingLevel2 = new GUIStyle { padding = new RectOffset(20, 0, 0, 0) };
    }
}
