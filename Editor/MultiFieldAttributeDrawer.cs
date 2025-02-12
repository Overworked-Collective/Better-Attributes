using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BetterAttributes
{
    [CustomPropertyDrawer(typeof(MultiFieldAttribute))]
    public class MultiFieldAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            FieldInfo[] fields = property.serializedObject.targetObject.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            List<FieldInfo> selectedFields = new List<FieldInfo>();
            for (int i = 0; i < fields.Length; i++)
            {
                List<System.Attribute> attributes = new List<System.Attribute>(fields[i].GetCustomAttributes());

                Debug.Log(fields[i].Name);

                if (attributes.Contains(attribute))
                {
                }
            }
        }
    }
}
