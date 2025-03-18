using System.Reflection;
using Tooling.Extensions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(VisibilityToggleAttribute))]
public class VisibilityToggleAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (IsVisibleAll(property, fieldInfo, attribute as VisibilityToggleAttribute))
        {
            //if has text area attribute change the way the feild render is handled
/*            if (property.isArray)
            {
                *//*base.OnGUI(position, property, label);
                GUIContent emptyContent = label;
                emptyContent.text = "";
                EditorGUI.PropertyField(position, property, emptyContent, true);*//*
                return;
            }*/

            //if has text area attribute change the way the feild render is handled
            if (fieldInfo.GetCustomAttribute<TextAreaAttribute>() != null)
            {
                base.OnGUI(position, property, label);
                GUIContent emptyContent = label;
                emptyContent.text = "";
                EditorGUI.PropertyField(position, property, emptyContent, true);
                return;
            }

            //defualt field
            EditorGUI.PropertyField(position, property, label, true);
        } 
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (IsVisibleAll(property, fieldInfo, attribute as VisibilityToggleAttribute))
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }

        return -2;
    }

    public static bool IsVisibleAll(SerializedProperty property, FieldInfo fieldInfo, VisibilityToggleAttribute attribute)
    {
        object[] attributes = fieldInfo.GetCustomAttributes(typeof(VisibilityToggleAttribute), true);
        foreach (object attributeSelected in attributes)
        {
            VisibilityToggleAttribute attributeCurrent = (VisibilityToggleAttribute)attributeSelected;
            if (!IsVisible(property, attributeCurrent))
            {
                return false;
            }
        }
        return true;
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
