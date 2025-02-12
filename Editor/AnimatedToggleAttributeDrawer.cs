using System.Collections.Generic;
using Tooling.Editor;
using Tooling.Extensions;
using UnityEditor;
using UnityEngine;

namespace BetterAttributes
{
    [CustomPropertyDrawer(typeof(AnimatedToggleAttribute))]
    public class AnimatedToggleAttributeDrawer : PropertyDrawer
    {
        Color _activeColor;
        Material _animatedMaterial;
        Dictionary<string, Material> _animatedMaterialPerElement = new Dictionary<string, Material>();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.IsArrayElement())
            {
                _animatedMaterialPerElement.TryGetValue(property.propertyPath, out _animatedMaterial);


                if (_animatedMaterial == null)
                {
                    InitMaterial(property);
                    _animatedMaterialPerElement.Add(property.propertyPath, _animatedMaterial);
                }
            }

            if (_animatedMaterial == null)
            {
                InitMaterial(property);
            }

            AnimatedToggleAttribute toggleAttribute = attribute as AnimatedToggleAttribute;

            GUISkin skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);

            _animatedMaterial.SetFloat("_Width", position.width);
            _animatedMaterial.SetFloat("_Height", position.height);

            GUIStyle labelStyle = new GUIStyle();
            labelStyle.alignment = TextAnchor.MiddleCenter;

            labelStyle.normal.textColor = Color.Lerp(new Color(0.3960785f, 0.3960785f, 0.3960785f, 1), new Color(1, 1, 1, 0.9f), 1 - _animatedMaterial.GetFloat("_AnimationPercentage"));

            Rect lable1Rect = new Rect(position);
            lable1Rect.width -= position.width / 2f;
            Rect lable2Rect = new Rect(lable1Rect);
            lable2Rect.x += lable2Rect.width;

            if ((lable1Rect.Contains(Event.current.mousePosition) && !property.boolValue) || (lable2Rect.Contains(Event.current.mousePosition) && property.boolValue))
            {
                _animatedMaterial.SetColor("_ActiveColor", _activeColor + new Color(0.1f, 0.1f, 0.1f, 0f));
            } 
            else
            {
                _animatedMaterial.SetColor("_ActiveColor", _activeColor);
            }

            property.boolValue = AnimatedEditor.DrawAnimatedToggle(position, property.boolValue, _animatedMaterial, "_AnimationPercentage", toggleAttribute.AnimationSpeed);

            EditorGUI.LabelField(lable1Rect, new GUIContent(toggleAttribute.Label1), labelStyle);

            labelStyle.normal.textColor = Color.Lerp(new Color(0.3960785f, 0.3960785f, 0.3960785f, 1), new Color(1, 1, 1, 0.9f), _animatedMaterial.GetFloat("_AnimationPercentage"));
            EditorGUI.LabelField(lable2Rect, new GUIContent(toggleAttribute.Label2), labelStyle);
        }

        private void InitMaterial(SerializedProperty property)
        {
            _animatedMaterial = new Material(Shader.Find("Hidden/RectToggle"));
            _animatedMaterial.SetFloat("_AnimationPercentage", property.boolValue.ToInt());
            _activeColor = _animatedMaterial.GetColor("_ActiveColor");
            _animatedMaterial.SetInt("_UsingLinearColorSpace", (PlayerSettings.colorSpace == ColorSpace.Linear).ToInt());
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + 5;
        }
    }
}
