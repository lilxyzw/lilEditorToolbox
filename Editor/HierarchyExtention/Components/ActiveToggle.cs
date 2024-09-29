using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal class ActiveToggle : IHierarchyExtensionComponent
    {
        private const int ICON_SIZE = 16;

        public int Priority => 100;

        public void OnGUI(ref Rect currentRect, GameObject gameObject, int instanceID, Rect fullRect)
        {
            currentRect.x -= ICON_SIZE;
            currentRect.width = ICON_SIZE;
            EditorGUI.BeginChangeCheck();
            var active = EditorGUI.Toggle(currentRect, gameObject.activeSelf);
            if(EditorGUI.EndChangeCheck())
            {
                using var so = new SerializedObject(gameObject);
                using var m_IsActive = so.FindProperty("m_IsActive");
                m_IsActive.boolValue = active;
                so.ApplyModifiedProperties();
            }
            currentRect.x -= 8;
        }
    }
}
