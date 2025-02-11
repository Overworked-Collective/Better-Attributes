using UnityEditor;
using UnityEngine;

namespace BetterAttributes
{
    [CustomPropertyDrawer(typeof(VisualTooltipAttribute))]
    public class VisualTooltipAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            VisualTooltipAttribute visualTooltipAttribute = attribute as VisualTooltipAttribute;
            Vector2 tooltipTextScale = visualTooltipAttribute.Tooltip.GetTextScale();

            Rect rect = new Rect(position.position, position.size);
            EditorGUI.PropertyField(rect, property);

            GUIStyle tooltipStyle = new GUIStyle();
            tooltipStyle.normal.textColor = Color.grey;
            tooltipStyle.fontSize = 12;
            tooltipStyle.wordWrap = true;

            RectOffset padding = new RectOffset();
            padding.left = 15;
            padding.top = 3;
            padding.bottom = 3;
            tooltipStyle.padding = padding;

            tooltipStyle.normal.background = MakeTex((int)position.width, (int)position.height, new Color(0, 0, 0, .2f));

            EditorGUILayout.LabelField(new GUIContent(visualTooltipAttribute.Tooltip), tooltipStyle);
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            try
            {
                Color[] pix = new Color[width * height];


            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
            } 
            catch
            {
                return null;
            }
        }
    }
}
