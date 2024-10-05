using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal class HierarchySpacer : IHierarchyExtensionComponent
    {
        public int Priority => EditorToolboxSettings.instance.hierarchySpacerPriority;

        public void OnGUI(ref Rect currentRect, GameObject gameObject, int instanceID, Rect fullRect)
        {
            currentRect.x = fullRect.xMax - EditorToolboxSettings.instance.hierarchySpacerWidth;
        }
    }
}
