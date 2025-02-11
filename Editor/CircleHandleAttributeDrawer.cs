using System;
using Tooling.Editor;
using UnityEditor;
using UnityEngine;

namespace BetterAttributes
{
    [CustomPropertyDrawer(typeof(CircleHandleAttribute))]
    public class CircleHandleAttributeDrawer : PropertyDrawer
    {

        bool _initialized;
        float _initializedTime;

        SerializedProperty _property;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);
            _property = property;
            Init();
        }

        void Init()
        {
            if (_initialized) { return; }
            _initialized = true;
            _initializedTime = GetTimeInSeconds();
            SceneView.duringSceneGui += OnSceneGUI;
            Selection.selectionChanged += Unsubscribe;
        }

        private void Unsubscribe()
        {
            if ((GetTimeInSeconds() - _initializedTime) < 0.01f) { return; }
            Selection.selectionChanged -= Unsubscribe;
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnSceneGUI(SceneView view)
        {
            if (_property == null) { return; }

            CircleHandleAttribute circleHandleAttribute = (CircleHandleAttribute)attribute;

            Transform objectTransform;
            float min = circleHandleAttribute.Min;
            float max = circleHandleAttribute.Max;
            try
            {
                object source = _property.serializedObject.targetObject;
                var type = source.GetType();
                objectTransform = (Transform)type.GetProperty("transform").GetValue(source);
            }
            catch
            {
                return;
            }

            try
            {
                if (!circleHandleAttribute.MinByReference.Equals(""))
                {
                    min = _property.serializedObject.FindProperty(circleHandleAttribute.MinByReference).floatValue;
                }
            }
            catch
            {
                Debug.LogError($"Could not find property: {circleHandleAttribute.MinByReference}");
            }

            try
            {
                if (!circleHandleAttribute.MaxByReference.Equals(""))
                {
                    max = _property.serializedObject.FindProperty(circleHandleAttribute.MaxByReference).floatValue;
                }
            }
            catch
            {
                Debug.LogError($"Could not find property: {circleHandleAttribute.MaxByReference}");
            }

            _property.floatValue = HandleRenderer.DrawCircularHandle(HandleRenderer.GetHandleIDs(_property.name, 4), objectTransform, _property.floatValue, circleHandleAttribute.FillCircle);
            if (_property.floatValue > max) { _property.floatValue = max; }
            if (_property.floatValue < min) { _property.floatValue = min; }
            _property.serializedObject.ApplyModifiedProperties();
        }
        private float GetTimeInSeconds()
        {
            return DateTime.Now.Minute * 60 + DateTime.Now.Second + DateTime.Now.Millisecond * 0.001f;
        }
    }
}
