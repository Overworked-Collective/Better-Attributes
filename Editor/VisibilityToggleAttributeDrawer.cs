using Tooling.Extensions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


//[CustomPropertyDrawer(typeof(VisibilityToggleAttribute))]
public class VisibilityToggleAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (IsVisible(property, attribute as VisibilityToggleAttribute))
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (IsVisible(property, attribute as VisibilityToggleAttribute))
        {
            return base.GetPropertyHeight(property, label);
        }
        return -2;
    }

    public static bool IsVisible(SerializedProperty property, VisibilityToggleAttribute attribute)
    {
        string variablePath = property.GetContainerPath();
        variablePath = variablePath.Equals("") ? attribute.Variable : variablePath + "." + attribute.Variable;
        SerializedProperty variable = property.serializedObject.FindProperty(variablePath);

        if (variable != null && variable.GetUnderlyingType().Equals(attribute.Value.GetType()))
        {
            if (variable.GetUnderlyingValue().Equals(attribute.Value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return true;
    }
}
