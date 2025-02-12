using Tooling.Extensions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(VisibilityToggleAttribute))]
public class VisibilityToggleAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        VisibilityToggleAttribute toggleAttribute = (VisibilityToggleAttribute)attribute;

        string variablePath = property.GetContainerPath();
        variablePath = variablePath.Equals("") ? toggleAttribute.Variable : variablePath + "." + toggleAttribute.Variable;
        SerializedProperty variable = property.serializedObject.FindProperty(variablePath);

        Rect propertyRect = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(property));

        if (variable != null && variable.GetUnderlyingType().Equals(toggleAttribute.Value.GetType()))
        {
            if (variable.GetUnderlyingValue().Equals(toggleAttribute.Value))
            {
                EditorGUI.PropertyField(propertyRect, property, label);
            }
            else
            {
                return;
            }
        }
        else
        {
            EditorGUI.PropertyField(propertyRect, property, label);
        }
    }


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        VisibilityToggleAttribute toggleAttribute = (VisibilityToggleAttribute)attribute;

        string variablePath = property.GetContainerPath();
        variablePath = variablePath.Equals("") ? toggleAttribute.Variable : variablePath + "." + toggleAttribute.Variable;
        SerializedProperty variable = property.serializedObject.FindProperty(variablePath);

        if (variable != null && variable.GetUnderlyingType().Equals(toggleAttribute.Value.GetType()))
        {
            if (variable.GetUnderlyingValue().Equals(toggleAttribute.Value))
            {
                return base.GetPropertyHeight(property, label);
            }
            else
            {
                return -2;
            }
        }
        else
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}
