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
            var active = EditorGUI.Toggle(currentRect, gameObject.activeSelf);
            if(gameObject.activeSelf != active) gameObject.SetActive(active);
            currentRect.x -= 8;
        }
    }
}
