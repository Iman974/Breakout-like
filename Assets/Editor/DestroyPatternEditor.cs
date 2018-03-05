using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DestroyPattern))]
public class DestroyPatternEditor : Editor {

    private SerializedProperty brickVectorArray;
    private SerializedProperty[] areVectorsEnabled;
    private DestroyPattern pattern;
    private SerializedProperty[] dirAngles, directions, counts, lengths;

    private void OnEnable() {
        brickVectorArray = serializedObject.FindProperty("brickVectors");
        areVectorsEnabled = new SerializedProperty[brickVectorArray.arraySize];

        for (int i = 0; i < areVectorsEnabled.Length; i++) {
            areVectorsEnabled[i] = brickVectorArray.GetArrayElementAtIndex(i).FindPropertyRelative("enabled");
        }

        pattern = (DestroyPattern)target;

        dirAngles = new SerializedProperty[brickVectorArray.arraySize];
        directions = new SerializedProperty[brickVectorArray.arraySize];
        counts = new SerializedProperty[brickVectorArray.arraySize];
        lengths = new SerializedProperty[brickVectorArray.arraySize];
        for (int i = 0; i < brickVectorArray.arraySize; i++) {
            dirAngles[i] = brickVectorArray.GetArrayElementAtIndex(i).FindPropertyRelative("angle");
            directions[i] = brickVectorArray.GetArrayElementAtIndex(i).FindPropertyRelative("direction");
            counts[i] = brickVectorArray.GetArrayElementAtIndex(i).FindPropertyRelative("count");
            lengths[i] = brickVectorArray.GetArrayElementAtIndex(i).FindPropertyRelative("length");
        }
    }

    public override void OnInspectorGUI() {
        if (pattern == null || brickVectorArray == null) {
            return;
        }

        serializedObject.Update();
        for (int i = 0; i < pattern.BrickVectors.Length; i++) {
            EditorGUILayout.BeginHorizontal();

            areVectorsEnabled[i].boolValue = EditorGUILayout.ToggleLeft(pattern.BrickVectors[i].Name, areVectorsEnabled[i].boolValue,
                GUIStyleUtility.leftToggleLabel);

            EditorGUILayout.EndHorizontal();

            if (!pattern.BrickVectors[i].Enabled) {
                continue;
            }

            EditorGUILayout.BeginVertical(GUIStyleUtility.rightPadding2);
            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(dirAngles[i], GUILayout.Width(255f));
            if (EditorGUI.EndChangeCheck()) {
                directions[i].vector2Value =
                    BrickVector.GetDirectionFromAngle(dirAngles[i].floatValue);
            }

            EditorGUILayout.LabelField(pattern.BrickVectors[i].Direction.ToString());
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(counts[i]);
            EditorGUILayout.PropertyField(lengths[i]);

            EditorGUILayout.EndVertical(); 
        }
        serializedObject.ApplyModifiedProperties();
    }

    //public override bool RequiresConstantRepaint() {
    //    if (pattern == null) {
    //        return base.RequiresConstantRepaint();
    //    }

    //    for (int i = 0; i < pattern.BrickVectors.Length; i++) {
    //        if (!pattern.BrickVectors[i].Enabled) {
    //            continue;
    //        }

    //        int previousVectorIndex = i - 1;
    //        while (previousVectorIndex >= 0 && !pattern.BrickVectors[previousVectorIndex].Enabled) {
    //            DestroyPattern.BrickVector previousSegment = pattern.BrickVectors[previousVectorIndex];
    //            pattern.BrickVectors[previousVectorIndex] = pattern.BrickVectors[i];
    //            pattern.BrickVectors[i] = previousSegment;
    //            previousVectorIndex--;
    //        }
    //    }
    //    return base.RequiresConstantRepaint();
    //}
}