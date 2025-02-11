using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BetterAttributes
{
    [CustomPropertyDrawer(typeof(DevVarAttribute))]
    public class DevVarAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DevVarAttribute devVarAttribute = attribute as DevVarAttribute;
            Rect rect = new Rect(position.position, position.size);
            EditorGUI.PropertyField(rect, property);
            //DevVarSettings settings = AssetDatabase.LoadAssetAtPath<DevVarSettings>(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:DevVarSettings")[0]));
            //DevVarSettings settings = AssetDatabase.LoadAssetAtPath<DevVarSettings>("Assets/Editor/DevVarSettings.asset");
            //EditorGUI.DrawRect(rect, settings.GetColor());
            Color col = new Color(0, 0, 0, .2f);
            EditorGUI.DrawRect(rect, col);
        }
    }
}
