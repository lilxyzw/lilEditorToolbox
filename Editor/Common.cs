using System.Linq;
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal static class Common
    {
        internal const string MENU_HEAD = "Tools/lilEditorToolbox/";

        internal static bool SkipScan(Object obj)
        {
            return obj is GameObject ||
                // Skip - Component
                obj is Transform ||
                // Skip - Asset
                obj is Mesh ||
                obj is Texture ||
                obj is Shader ||
                obj is TextAsset ||
                obj.GetType() == typeof(Object);
        }

        internal static string ToDisplayName(string name)
            => string.Concat(name.Select(c => char.IsUpper(c) ? $" {c}" : $"{c}")).TrimStart();
    }

    internal class ToggleLeftAttribute : PropertyAttribute { }

    [CustomPropertyDrawer(typeof(ToggleLeftAttribute))]
    internal class ToggleLeftDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            var boolValue = EditorGUI.ToggleLeft(position, label, property.boolValue);
            if(EditorGUI.EndChangeCheck()) property.boolValue = boolValue;
        }
    }
}
