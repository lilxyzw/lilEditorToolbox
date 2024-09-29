using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal class MaterialQueue : IProjectExtensionComponent
    {
        public int Priority => 1;

        public void OnGUI(ref Rect currentRect, string guid, string path, string name, string extension, Rect fullRect)
        {
            if(extension != ".mat" || ProjectExtension.GUIDToObject(guid) is not Material material) return;

            GUIHelper.DrawLabel(ref currentRect, $"Q: {material.renderQueue}");
        }
    }
}
