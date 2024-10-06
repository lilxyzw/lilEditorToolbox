using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    [Tooltip("Shows the icon if the object is EditorOnly.")]
    internal class EditorOnlyLabel : IHierarchyExtensionComponent
    {
        public int Priority => -800;
        private static GUIStyle m_EOIcon;
        private static GUIStyle EOIcon => m_EOIcon != null ? m_EOIcon : m_EOIcon = new GUIStyle(EditorStyles.miniLabel){fontStyle = FontStyle.Bold};

        public void OnGUI(ref Rect currentRect, GameObject gameObject, int instanceID, Rect fullRect)
        {
            if(IsEditorOnly(gameObject.transform))
            {
                var rectEO = fullRect;
                rectEO.x = 36;
                EditorGUI.LabelField(rectEO, "E", EOIcon);
            }
        }

        private static bool IsEditorOnly(Transform obj)
        {
            if(obj.tag == "EditorOnly") return true;
            if(obj.transform.parent == null) return false;
            return IsEditorOnly(obj.transform.parent);
        }
    }
}
