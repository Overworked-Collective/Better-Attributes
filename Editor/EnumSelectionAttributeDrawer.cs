using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tooling.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(EnumSelectionAttribute))]
public class EnumSelectionAttributeDrawer : PropertyDrawer
{
    static float _buttonHeight = 22;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        GUIStyle titleStyle = GUI.skin.label;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        return base.GetPropertyHeight(property, label) + 6 + GetRowCount(EditorGUIUtility.currentViewWidth - (EditorGUI.indentLevel+1) * 22, property, titleStyle) * (_buttonHeight + 2);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (fieldInfo.FieldType.GetCustomAttributes<FlagsAttribute>().ToArray().Length == 0)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        //background
        EditorGUI.DrawRect(position, new Color(0, 0, 0, .2f));

        Rect titleRect = new Rect(position);
        titleRect.height = 18;
        titleRect.y += 2;

        GUIStyle titleStyle = GUI.skin.label;
        titleStyle.alignment = TextAnchor.MiddleCenter;

        EditorGUI.LabelField(titleRect, label.text, titleStyle);
        List<Rect> buttonRects = GetButtonRects(position, property, titleStyle);

        for (int i = 0; i < buttonRects.Count; i++)
        {
            DrawButton(buttonRects[i], property, i);
            EditorGUI.LabelField(buttonRects[i], property.enumDisplayNames[i], titleStyle);
        }

        //EditorGUI.DrawRect(position, new Color(1, 1, 1, 0.5f));
        //Replace with GLLib.DrawQuad
    }

    private static float GetRowCount(float elementWidth, SerializedProperty property, GUIStyle titleStyle)
    {
        List<string> enumDisplayNames = new List<string>(property.enumDisplayNames);
        int indexer = 0;

        int currentRow = 1;
        while (true)
        {
            float remainingWidth = elementWidth;
            List<float> buttonWidths = new List<float>();

            for (int i = 0; i < enumDisplayNames.Count; i++)
            {
                float displayWidth = titleStyle.CalcSize(new GUIContent(enumDisplayNames[i])).x + 20;//temp padding val
                if (displayWidth <= remainingWidth)
                {
                    //fits
                    remainingWidth -= displayWidth;
                    buttonWidths.Add(displayWidth);
                }
                else
                {
                    break;
                }
            }

            if (buttonWidths.Count == 0)
            {
                break;
            }

            enumDisplayNames.RemoveRange(0, buttonWidths.Count);
            currentRow++;

            if (enumDisplayNames.Count == 0 || indexer >= 100)
            {
                break;
            }
            indexer++;
        }

        return (currentRow - 1);
    }

    private static List<Rect> GetButtonRects(Rect position, SerializedProperty property, GUIStyle titleStyle)
    {
        List<Rect> buttonRects = new List<Rect>();
        List<string> enumDisplayNames = new List<string>(property.enumDisplayNames);
        int indexer = 0;

        int currentRow = 1;
        while (true)
        {
            float remainingWidth = position.width;
            List<float> buttonWidths = new List<float>();

            for (int i = 0; i < enumDisplayNames.Count; i++)
            {
                float displayWidth = titleStyle.CalcSize(new GUIContent(enumDisplayNames[i])).x + 20;//temp padding val
                if (displayWidth <= remainingWidth)
                {
                    //fits
                    remainingWidth -= displayWidth;
                    buttonWidths.Add(displayWidth);
                }
                else
                {
                    break;
                }
            }

            float summedButtonWidth = 0;
            foreach (float width in buttonWidths)
            {
                summedButtonWidth += width;
            }
            float padding = 3;
            float additionalWidthPerRect = (position.width - summedButtonWidth) / (float)buttonWidths.Count - padding;

            float xPos = position.x;
            for (int i = 0; i < buttonWidths.Count; i++)
            {
                Rect buttonRect = new Rect();
                buttonRect.width = buttonWidths[i] + additionalWidthPerRect;
                buttonRect.height = _buttonHeight;

                float adjustedPadding = (padding * buttonWidths.Count) / (buttonWidths.Count + 1);
                adjustedPadding = Mathf.RoundToInt(adjustedPadding);

                buttonRect.x = xPos + adjustedPadding;
                xPos += buttonRect.width + adjustedPadding;

                buttonRect.y = (_buttonHeight + 2) * currentRow + position.y;

                buttonRects.Add(buttonRect);
            }

            enumDisplayNames.RemoveRange(0, buttonWidths.Count);
            currentRow++;

            if (enumDisplayNames.Count == 0 || indexer >= 100)
            {
                break;
            }
            indexer++;
        }

        return buttonRects;
    }

    public void DrawButton(Rect rect, SerializedProperty property, int index)
    {
        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            if (index == 0)
            {
                property.enumValueFlag = 0;
            }
            else if ((property.enumValueFlag & (1 << index)) == (1 << index))
            {
                property.enumValueFlag = property.enumValueFlag - (1 << index);
            } 
            else
            {
                property.enumValueFlag = property.enumValueFlag + (1 << index);
            }
            
            //mouse down on button
        }

        if ((property.enumValueFlag & (1<<index)) == (1 << index))
        {
            EditorGUI.DrawRect(rect, new Color(1, 1, 1, 0.3f));
        } 
        else
        {
            EditorGUI.DrawRect(rect, new Color(1, 1, 1, 0.1f));
        }
    }
}
