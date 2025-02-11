using Tooling.Editor;
using Tooling.Extensions;
using UnityEditor;
using UnityEngine;

namespace BetterAttributes
{
    [CustomPropertyDrawer(typeof(AnimatedToggleAttribute))]
    public class AnimatedToggleAttributeDrawer : PropertyDrawer
    {
        Material _animatedMat;
        Color _activeColor;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            AnimatedToggleAttribute toggleAttribute = attribute as AnimatedToggleAttribute;

            GUISkin skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);

            if (_animatedMat == null) 
            { 
                _animatedMat = new Material(Shader.Find("Hidden/RectToggle"));
                _animatedMat.SetFloat("_AnimationPercentage", property.boolValue.ToInt());
                _activeColor = _animatedMat.GetColor("_ActiveColor");
            }
            _animatedMat.SetFloat("_Width", position.width);
            _animatedMat.SetFloat("_Height", position.height);

            GUIStyle labelStyle = new GUIStyle();
            labelStyle.alignment = TextAnchor.MiddleCenter;

            labelStyle.normal.textColor = Color.Lerp(new Color(0.3960785f, 0.3960785f, 0.3960785f, 1), new Color(1, 1, 1, 0.9f), 1 - _animatedMat.GetFloat("_AnimationPercentage"));

            Rect lable1Rect = new Rect(position);
            lable1Rect.width -= position.width / 2f;
            Rect lable2Rect = new Rect(lable1Rect);
            lable2Rect.x += lable2Rect.width;

            if ((lable1Rect.Contains(Event.current.mousePosition) && !property.boolValue) || (lable2Rect.Contains(Event.current.mousePosition) && property.boolValue))
            {
                _animatedMat.SetColor("_ActiveColor", _activeColor + new Color(0.1f, 0.1f, 0.1f, 0f));
            } 
            else
            {
                _animatedMat.SetColor("_ActiveColor", _activeColor);
            }

            property.boolValue = AnimatedEditor.DrawAnimatedToggle(position, property.boolValue, _animatedMat, "_AnimationPercentage", toggleAttribute.AnimationSpeed);

            EditorGUI.LabelField(lable1Rect, new GUIContent(toggleAttribute.Label1), labelStyle);

            labelStyle.normal.textColor = Color.Lerp(new Color(0.3960785f, 0.3960785f, 0.3960785f, 1), new Color(1, 1, 1, 0.9f), _animatedMat.GetFloat("_AnimationPercentage"));
            EditorGUI.LabelField(lable2Rect, new GUIContent(toggleAttribute.Label2), labelStyle);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + 5;
        }
    }
}
