using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BetterAttributes
{
    public static class ButtonAttributeDrawer
    {
        public static void DrawButton(object target, MethodInfo method)
        {
            ButtonAttribute attribute = (ButtonAttribute)method.GetCustomAttributes(typeof(ButtonAttribute), true)[0];

            bool buttonEnabled = false;
            GUIStyle warningLabelStyle = new GUIStyle();
            warningLabelStyle.alignment = TextAnchor.MiddleCenter;
            warningLabelStyle.normal.textColor = Color.yellow;
            warningLabelStyle.wordWrap = true;

            if (!method.GetParameters().All(p => p.IsOptional))
            {
                buttonEnabled = true;
                GUILayout.Label(new GUIContent("Connected method arguments must have a defualt value."), warningLabelStyle);
            }

            if (method.ReturnType == typeof(IEnumerator) && !Application.isPlaying)
            {
                buttonEnabled = true;
                GUILayout.Label(new GUIContent("Method with IEnumerator return type can only be called in play mode."), warningLabelStyle);
            }

            EditorGUI.BeginDisabledGroup(buttonEnabled);

            if (GUILayout.Button(attribute.ButtonText.Equals("") ? method.Name : attribute.ButtonText))
            {
                object[] defaultParams = method.GetParameters().Select(p => p.DefaultValue).ToArray();
                
                if (method.ReturnType == typeof(IEnumerator))
                {
                    (target as MonoBehaviour).StartCoroutine(method.Name, defaultParams);
                }
                else
                {
                    method.Invoke(target, defaultParams);
                }
            }

            EditorGUI.EndDisabledGroup();
        }
    }
}
