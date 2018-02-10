using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomAnimation))]
public class AnimationEditor : Editor {

    private SerializedProperty slideAnimation, rotationAnimation, scaleAnimation;
    private SerializedProperty disabledBehaviours, randomDelay;
    private SerializedProperty slideBool, rotateBool, scaleBool;

    private void OnEnable() {
        /*
        slideAnimation = serializedObject.FindProperty("slideAnimation");
        rotationAnimation = serializedObject.FindProperty("rotationAnimation");
        scaleAnimation = serializedObject.FindProperty("scaleAnimation");
        disabledBehaviours = serializedObject.FindProperty("disabledDuringAnimation");
        slideBool = serializedObject.FindProperty("doSliding");
        rotateBool = serializedObject.FindProperty("doRotation");
        scaleBool = serializedObject.FindProperty("doScaling");
        */
    }

    /*public override void OnInspectorGUI() {
        //CustomAnimation customAnimation = (CustomAnimation)target;

        base.OnInspectorGUI();

        //if (slideBool != null) {
        //    EditorGUILayout.PropertyField(slideBool);
        //}
        //if (slideAnimation != null && slideBool.boolValue) {
        //    EditorGUILayout.PropertyField(slideAnimation);
        //    EditorGUILayout.Vector2Field("Start Position", customAnimation.relativeStartPosition);
        //}

        //if (rotateBool != null) {
        //    EditorGUILayout.PropertyField(rotateBool);
        //}
        //if (rotationAnimation != null && rotateBool.boolValue) {
        //    EditorGUILayout.PropertyField(rotationAnimation);
        //    EditorGUILayout.FloatField("Start Rotation", customAnimation.relativeStartRotation);
        //}

        //if (scaleBool != null) {
        //    EditorGUILayout.PropertyField(scaleBool);
        //}
        //if (scaleAnimation != null && scaleBool.boolValue) {
        //    EditorGUILayout.PropertyField(scaleAnimation);
        //    EditorGUILayout.FloatField("Start Scale", customAnimation.relativeStartScale);
        //}

        //if (randomDelay != null) {
        //    EditorGUILayout.PropertyField(randomDelay);
        //}
        //EditorGUILayout.FloatField("Max Random Delay", customAnimation.maxRandomDelay);

        //serializedObject.ApplyModifiedProperties();
    }*/
}
