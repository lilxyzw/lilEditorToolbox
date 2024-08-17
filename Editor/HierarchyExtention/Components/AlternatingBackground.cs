using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal class AlternatingBackground : IHierarchyExtentionConponent
    {
        public int Priority => -1500;

        public void OnGUI(ref Rect currentRect, GameObject gameObject, int instanceID, Rect fullRect)
        {
            if((int)fullRect.y % (int)(fullRect.height*2) >= (int)fullRect.height)
                EditorGUI.DrawRect(fullRect, EditorToolboxSettings.instance.backgroundColor);
        }
    }
}
