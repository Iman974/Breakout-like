using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MinAttribute))]
public class MinDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        MinAttribute minAttribute = (MinAttribute)attribute;

        if (property.propertyType == SerializedPropertyType.Float) {
            float fieldValue = EditorGUI.FloatField(position, label, property.floatValue);

            if (fieldValue < minAttribute.Minimum) {
                fieldValue = minAttribute.Minimum;
            }

            property.floatValue = fieldValue;

        } else if (property.propertyType == SerializedPropertyType.Integer) {
            int fieldValue = EditorGUI.IntField(position, label, property.intValue);

            if (fieldValue < minAttribute.Minimum) {
                fieldValue = (int)minAttribute.Minimum;
            }

            property.intValue = fieldValue;
        } else {
            EditorGUI.LabelField(position, label.text, "Use Min with float or int.");
        }
    }
}