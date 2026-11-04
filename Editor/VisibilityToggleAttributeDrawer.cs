using System.Reflection;
using System.Runtime.Remoting.Activation;
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
        if (!property.TryGetContainerPath(out string variablePath))
        {
            return true;
        }

        variablePath = variablePath.Equals("") ? attribute.Variable : variablePath + "." + attribute.Variable;

        SerializedProperty variable = property.serializedObject.FindProperty(variablePath);

        if (variable == null) { throw new System.Exception($"Variable '{attribute.Variable}' using path '{variablePath}' could not be found on serialized object."); }

        switch (attribute.ValueCondition)
        {
            case ValueCondition.IsNull:
                return variable.boxedValue == null;
            case ValueCondition.IsNotNull:
                return variable.boxedValue != null;

            case ValueCondition.IsDefault:
                return variable.boxedValue == default;
            case ValueCondition.IsNotDefault:
                return variable.boxedValue != default;
            
            case ValueCondition.None:
            default:
                break;
        }

        // if both variable and value are null => return true
        if (variable.boxedValue == null && attribute.Value == null) { return true; }

        // if one is null but not both => return false
        if ((variable.boxedValue == null) != (attribute.Value == null)) { return false; }

        // we know both are not null so now do type equivelance and value equivelance check
        if (variable.GetUnderlyingType().Equals(attribute.Value.GetType()))
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
