using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tooling.Editor;
using Tooling.Extensions;
using UnityEditor;
using UnityEngine;

namespace BetterAttributes
{
    [CustomPropertyDrawer(typeof(ObjectPreviewAttribute))]
    public class ObjectPreviewAttributeDrawer : PropertyDrawer
    {
        float previewSize = 45;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
/*            VisibilityToggleAttribute toggleAttribute = fieldInfo.GetAttribute<VisibilityToggleAttribute>();
            if (toggleAttribute != null && !VisibilityToggleAttributeDrawer.IsVisible(property, toggleAttribute))
            {
                return;
            }*/

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue == null)
                {
                    EditorGUI.PropertyField(position, property);
                    return;
                }

                Rect fieldRect = new Rect(position);
                fieldRect.height = EditorGUI.GetPropertyHeight(property);
                fieldRect.width = position.width - previewSize - 7;
                fieldRect.x += previewSize + 7;
                fieldRect.y += position.height / 3f;

                EditorGUI.PropertyField(fieldRect, property);

                Rect previewRect = new Rect(position);
                previewRect = EditorGUI.IndentedRect(previewRect);
                previewRect.width = previewSize;
                previewRect.height = previewSize;

                fieldRect.y += EditorGUI.GetPropertyHeight(property);
                EditorGUI.DrawRect(previewRect, new Color(.15f, .15f, .15f, 1));

                previewRect.width -= 2;
                previewRect.height -= 2;
                previewRect.x += 1;
                previewRect.y += 1;

                Texture2D texture = AssetPreview.GetAssetPreview(property.objectReferenceValue);
                if (texture != null)
                {
                    EditorGUI.DrawPreviewTexture(previewRect, texture);
                }
                //GLLib.DrawQuad(position, )
            }
            //base.OnGUI(position, property, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
/*            VisibilityToggleAttribute toggleAttribute = fieldInfo.GetAttribute<VisibilityToggleAttribute>();
            if (toggleAttribute != null && !VisibilityToggleAttributeDrawer.IsVisible(property, toggleAttribute))
            {
                return -2;
            }*/

            if (base.GetPropertyHeight(property, label) <= 0) { base.GetPropertyHeight(property, label); }
            return base.GetPropertyHeight(property, label) + (property.objectReferenceValue == null ? 0 : previewSize - EditorGUI.GetPropertyHeight(property));
        }
    }
}
