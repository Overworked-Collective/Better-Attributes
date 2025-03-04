using BetterAttributes;
using System.Collections.Generic;
using System.Reflection;
using Tooling.Reflection;
using Tooling.Extensions;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(UnityEngine.Object), true)]
public class OverrideInspector : Editor
{
    private IEnumerable<MethodInfo> _methodsWithButtonAttribute;
    private IEnumerable<FieldInfo> _serializedFields;

    protected virtual void OnEnable()
    {
        //Get all methods that have a button attribute attached
        _methodsWithButtonAttribute = ReflectionUtility.GetAllMethods(target, m => m.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0);

        _serializedFields = ReflectionUtility.GetAllFields(target, m => m != null);
    }

    public override void OnInspectorGUI()
    {
        /*        foreach (var item in _serializedFields)
                {
                    Debug.Log(item.Name);
                }*/

        using (var iterator = serializedObject.GetIterator())
        {
            if (iterator.NextVisible(true))
            {
                do
                {
                    /*if (iterator.isArray)
                    {
                        for (int i = 0; i < iterator.arraySize; i++)
                        {
                            ShowPropertyField(iterator.GetArrayElementAtIndex(i));
                        }
                        continue;
                    }*/

                    //Debug.Log(iterator.name + "  " + _serializedFields.First().Name);
                    //Debug.Log(iterator.name.Equals(_serializedFields.First().Name));
                    //FieldInfo info = _serializedFields.Single(f => f.Name.Equals(iterator.name));

                    ShowPropertyField(iterator);
                }
                while (iterator.NextVisible(false));
            }
        }

        //var iterator = serializedObject.GetIterator();

        //base.OnInspectorGUI();
        DrawButtons();
    }

    private void ShowPropertyField(SerializedProperty iterator)
    {
        FieldInfo info = SelectField(iterator.name);
        if (info == null) { return; }
        //Debug.Log(info.Name);
        VisibilityToggleAttribute atribute = info.GetAttribute<VisibilityToggleAttribute>();
        if (atribute != null)
        {
            if (VisibilityToggleAttributeDrawer.IsVisible(serializedObject.FindProperty(iterator.name), atribute))
            {
                return;
            }
        }
        EditorGUILayout.PropertyField(serializedObject.FindProperty(iterator.name));
    }

    private FieldInfo SelectField(string name)
    {
        foreach (var field in _serializedFields)
        {
            if (name.Equals(field.Name))
            {
                return field;
            }
        }
        return null;
    }

    private void DrawButtons()
    {
        foreach (var method in _methodsWithButtonAttribute)
        {
            ButtonAttributeDrawer.DrawButton(target, method);
        }
    }   
}